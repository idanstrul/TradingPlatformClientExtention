using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Forms;
using IB_TradingPlatformExtention1.Interfaces;

namespace IB_TradingPlatformExtention1
{
    public class IBApiClient : IBrokerApiClient
    {
        public LastTickDetails LastTickDetails { get; set; } = new LastTickDetails();
        public List<Position> OpenPositions { get; private set; } = new List<Position>();
        public List<OpenOrder> OpenOrders { get; private set; } = new List<OpenOrder>();
        public List<Contract> USContracts { get; private set; } = new List<Contract>();
        private Contract CurrContract;
        private double? TrailStopPrice = null;
        private EWrapperImpl wrapper;
        private EReader reader;
        private Thread apiThread;
        public int orderId = 0;


        // Events to notify the form when data changes
        public event Action<string, string> OnTickPriceUpdated;
        public event Action OnPositionChanged;
        public event Action<object[]> OnContractSamplesReceived;
        public event Action<int, HashSet<string>, HashSet<double>> OnOptionChainDetailsReceived;
        public event Action OnOptionChainDetailsReceivedEnd;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action OnContractSelected;

        public IBApiClient()
        {
            // Instantiate the ibClient and pass the wrapper
            this.wrapper = new EWrapperImpl(this);
        }

        public void Connect(string host, int port, int clientId)
        {
            // Connect to TWS
            wrapper.ClientSocket.eConnect(host, port, clientId);

            // Start the EReader to process messages
            reader = new EReader(wrapper.ClientSocket, wrapper.Signal);
            reader.Start();

            apiThread = new Thread(() =>
            {
                while (wrapper.ClientSocket.IsConnected())
                {
                    wrapper.Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true };
            apiThread.Start();

            // Wait for connection to complete
            while (wrapper.NextOrderId <= 0) { }

            orderId = wrapper.NextOrderId;
            wrapper.ClientSocket.reqPositions();

            // Notify the form that connection is established
            OnConnected?.Invoke();
        }

        public void Disconnect()
        {
            wrapper.ClientSocket.eDisconnect();
            OnDisconnected?.Invoke();
        }

        public void SearchContracts(string symbol)
        {
            wrapper.ClientSocket.reqMatchingSymbols(1, symbol);
        }

        public void SetCurrContract(int conId)
        {
            CurrContract = USContracts.Where(x => x.ConId == conId).FirstOrDefault();
            OnContractSelected?.Invoke();
        }

        public void GetDataForCurrContract()
        {
            if (CurrContract == null) return;
            wrapper.ClientSocket.cancelMktData(1); // cancel market data

            CurrContract.Exchange = CurrContract.Exchange ?? "SMART";
            // Create a new TagValueList object (for API version 9.71 and later) 
            List<TagValue> mktDataOptions = new List<TagValue>();

            // If using delayed market data subscription un-comment 
            // the line below to request delayed data
            wrapper.ClientSocket.reqMarketDataType(1);  // delayed data = 3 live = 1

            // Kick off the subscription for real-time data (add the mktDataOptions list for API v9.71)

            // For API v9.72 and higher, add one more parameter for regulatory snapshot
            wrapper.ClientSocket.reqMktData(1, CurrContract, "", false, false, mktDataOptions);

        }

        public Position GetPositionForCurrContract(Contract currContract)
        {
            if (currContract == null) return null;
            return OpenPositions.FirstOrDefault(p => p.Contract.Symbol == currContract.Symbol
                                                 && p.Contract.SecType == currContract.SecType
                                                 //&& p.Contract.Exchange == currContract.Exchange
                                                 && p.PositionAmount != 0);
        }

        public List<OpenOrder> GetStopLossOrdersForPosition(Position position)
        {
            if (position == null) return null;
            return OpenOrders.Where(order =>
                order.Contract.Symbol == position.Contract.Symbol &&
                order.Contract.SecType == position.Contract.SecType &&
                //order.Contract.Exchange == position.Contract.Exchange &&
                (order.Order.OrderType == "STP" ||
                 order.Order.OrderType == "STP LMT" ||
                 order.Order.OrderType == "TRAIL" ||
                 order.Order.OrderType == "TRAIL LIMIT") &&
                 ((order.Order.Action == "BUY" && position.PositionAmount < 0) ||
                 (order.Order.Action == "SELL" && position.PositionAmount > 0)))
                .ToList();
        }

        public void PlaceOrder(string side, Keys modifierKeys, decimal totalQuantity, double lmtPriceOffset, int stopType, bool isOutsideRth, double stopPrice)
        {
            if (CurrContract == null) return;

            double lmtPrice = ((side == "BUY" && modifierKeys != Keys.Alt) || (side == "SELL" && modifierKeys == Keys.Alt) ?
                LastTickDetails.Ask : LastTickDetails.Bid) + lmtPriceOffset;

            Position pos = GetPositionForCurrContract(CurrContract);
            List<OpenOrder> currStopLossOrders = GetStopLossOrdersForPosition(pos);

            Order order = new Order
            {
                OcaType = 2,
                OrderId = orderId,
                Action = side,
                OrderType = (modifierKeys == Keys.Control) ? "MKT" : "LMT",
                TotalQuantity = totalQuantity,
                LmtPrice = lmtPrice,
                OutsideRth = isOutsideRth,
            };

            Order stopLossOrder = null;

            if (stopType > 0)
            {
                order.Transmit = false;

                double trailStopPrice = side == "BUY" ?
                LastTickDetails.Bid - stopPrice : LastTickDetails.Ask + stopPrice;

                stopLossOrder = new Order
                {
                    OcaGroup = CurrContract.Symbol + "_" + CurrContract.SecType + "_" + orderId,
                    OcaType = 2,
                    TriggerMethod = 7,
                    ParentId = orderId,
                    OrderId = orderId + 1,
                    Action = side == "BUY" ? "SELL" : "BUY",
                    TotalQuantity = totalQuantity,
                    OutsideRth = isOutsideRth,
                    AuxPrice = stopPrice,
                };

                if (stopType == 2)
                {
                    stopLossOrder.OrderType = (isOutsideRth) ? "TRAIL LIMIT" : "TRAIL";
                    stopLossOrder.TrailStopPrice = trailStopPrice;
                    if (stopLossOrder.OrderType == "TRAIL LIMIT") stopLossOrder.LmtPriceOffset = -4 * lmtPriceOffset;
                }
                else if (stopType == 1)
                {
                    stopLossOrder.OrderType = (isOutsideRth) ? "STP LMT" : "STP";
                    if (stopLossOrder.OrderType == "STP LMT") stopLossOrder.LmtPrice = stopPrice - 4 * lmtPriceOffset;
                }
            }

            if (pos != null)
            {
                // Is this order is for increasing or decreasing position size
                bool isIncreasePos = (pos.PositionAmount > 0 && side == "BUY") || (pos.PositionAmount < 0 && side == "SELL");
                order.Transmit = true;

                if (!isIncreasePos && currStopLossOrders.Count > 0)
                {
                    Order currStopLossOrder = currStopLossOrders.First().Order;
                    if (currStopLossOrder.OrderType == "TRAIL" || currStopLossOrder.OrderType == "TRAIL LIMIT")
                    {
                        TrailStopPrice = currStopLossOrder.TrailStopPrice;
                    }
                    order.OcaGroup = currStopLossOrders.First().Order.OcaGroup;
                }
            }

            // Place the order
            wrapper.ClientSocket.placeOrder(order.OrderId, CurrContract, order);

            UpdateOrder(order, CurrContract);

            // increase the order id value
            orderId++;

            if (!order.Transmit)
            {
                //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
                //to activate all its predecessors
                stopLossOrder.Transmit = true;
                wrapper.ClientSocket.placeOrder(stopLossOrder.OrderId, CurrContract, stopLossOrder);

                UpdateOrder(stopLossOrder, CurrContract);

                orderId++;
            }
        }

        public void AdjustStopLoss(bool isOutsideRth, int stopType, double stopPrice, double limitPriceOffset)
        {
            if (CurrContract == null) return;

            var position = GetPositionForCurrContract(CurrContract);

            if (position == null || position.PositionAmount == 0) return;

            var stopLossOrders = GetStopLossOrdersForPosition(position);

            double trailStopPrice = TrailStopPrice != null ? (double)TrailStopPrice : position.PositionAmount > 0 ?
                LastTickDetails.Bid - stopPrice : LastTickDetails.Ask + stopPrice;

            Order stopLoss = new Order
            {
                OcaGroup = CurrContract.Symbol + "_" + CurrContract.SecType + "_" + orderId,
                OcaType = 2,
                OrderId = orderId,
                Action = position.PositionAmount > 0 ? "SELL" : "BUY",
                TriggerMethod = 7,
                TotalQuantity = Math.Abs(position.PositionAmount),
                OutsideRth = isOutsideRth,
                AuxPrice = stopPrice

            };

            if (stopType == 2)
            {
                stopLoss.OrderType = (isOutsideRth) ? "TRAIL LIMIT" : "TRAIL";
                stopLoss.TrailStopPrice = trailStopPrice;
                if (stopLoss.OrderType == "TRAIL LIMIT") stopLoss.LmtPriceOffset = -4 * limitPriceOffset;
            }
            else if (stopType == 1)
            {
                stopLoss.OrderType = (isOutsideRth) ? "STP LMT" : "STP"; //or "STP LMT"
                if (stopLoss.OrderType == "STP LMT") stopLoss.LmtPrice = stopPrice - 4 * limitPriceOffset;
            }

            //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
            //to activate all its predecessors
            stopLoss.Transmit = true;


            if (stopLossOrders.Count == 1)
            {
                //_stopLossOrder.ParentId = stopLossOrders.First().ParentId;
                stopLoss.OcaGroup = stopLossOrders.First().Order.OcaGroup;
                string currStopType = stopLossOrders.First().Order.OrderType;

                if ((currStopType == "STP" && stopType == 1 && !isOutsideRth) ||
                    (currStopType == "STP LMT" && stopType == 1 && isOutsideRth) ||
                    (currStopType == "TRAIL" && stopType == 2 && !isOutsideRth) ||
                    (currStopType == "TRAIL LIMIT" && stopType == 2 && isOutsideRth))
                {
                    stopLoss.TrailStopPrice = stopLossOrders.First().Order.TrailStopPrice;
                    stopLoss.OrderId = stopLossOrders.First().Order.OrderId;
                }
                else
                {
                    wrapper.ClientSocket.cancelOrder(stopLossOrders.First().Order.OrderId, new OrderCancel());
                }
            }
            else if (stopLossOrders.Count > 1)
            {
                stopLossOrders.ForEach(order => wrapper.ClientSocket.cancelOrder(order.Order.OrderId, new OrderCancel()));
            }

            if (stopType == 0) return;

            wrapper.ClientSocket.placeOrder(stopLoss.OrderId, CurrContract, stopLoss);

            TrailStopPrice = null;
            UpdateOrder(stopLoss, CurrContract);

            orderId++;

        }

        public void ClosePositionForCurrContract(double tradePriceOffset, bool isOutsideRth)
        {
            if (CurrContract == null) return;

            CancelAllOrdersForCurrContract(false);

            // Find the position for the given contract
            var positionToClose = GetPositionForCurrContract(CurrContract);

            // If a position exists, proceed to close it
            if (positionToClose != null && positionToClose.PositionAmount != 0)
            {
                string side = positionToClose.PositionAmount > 0 ? "SELL" : "BUY";

                double lmtPriceOffset = (double)((side == "BUY") ? 4 * tradePriceOffset : -4 * tradePriceOffset);
                double lmtPrice = (side == "BUY" ? LastTickDetails.Ask : LastTickDetails.Bid) + lmtPriceOffset;

                // Define a new order to close the position by taking the opposite action
                Order closeOrder = new Order
                {
                    OrderId = orderId,
                    Action = side,
                    OrderType = isOutsideRth ? "LMT" : "MKT",
                    TotalQuantity = Math.Abs(positionToClose.PositionAmount),
                    LmtPrice = lmtPrice,
                    OutsideRth = isOutsideRth,
                };
                // Place the order
                wrapper.ClientSocket.placeOrder(closeOrder.OrderId, CurrContract, closeOrder);

                UpdateOrder(closeOrder, CurrContract);

                // increase the order id value
                orderId++;
            }
            else
            {
                Console.WriteLine("No open position found for the specified contract.");
            }
        }

        public void CancelLastOrderForCurrContract()
        {
            if (CurrContract == null) return;

            OpenOrder prevOrder = OpenOrders
           .Where(order => order.Contract.Symbol == CurrContract.Symbol
                           && order.Contract.SecType == CurrContract.SecType
                           && order.Contract.Exchange == CurrContract.Exchange)
           .OrderByDescending(order => order.Order.OrderId)
           .FirstOrDefault();

            if (prevOrder != null)
                wrapper.ClientSocket.cancelOrder(prevOrder.Order.OrderId, new OrderCancel());
        }

        public void CancelAllOrdersForCurrContract(bool isGlobalCancel)
        {
            if (CurrContract == null) return;
            if (isGlobalCancel)
            {
                wrapper.ClientSocket.reqGlobalCancel(new OrderCancel());

            }
            else
            {
                var ordersToCancel = OpenOrders
                .Where(order => order.Contract.Symbol == CurrContract.Symbol &&
                                order.Contract.SecType == CurrContract.SecType &&
                                order.Contract.Exchange == CurrContract.Exchange)
                .ToList();

                foreach (var openOrder in ordersToCancel)
                {
                    wrapper.ClientSocket.cancelOrder(openOrder.Order.OrderId, new OrderCancel());
                }
            }

        }

        public void OnGetTickPrice(string tickPrice)
        {
            string[] tickerPrice = tickPrice.Split(',');

            if (Convert.ToInt32(tickerPrice[0]) == 1)
            {
                if (Convert.ToInt32(tickerPrice[1]) == 4)// Delayed Last quote 68, if you want realtime use tickerPrice == 4
                {
                    LastTickDetails.Last = Convert.ToDouble(tickerPrice[2]);
                    // Add the text string to the list box
                    OnTickPriceUpdated?.Invoke("Last", tickerPrice[2]);

                }
                else if (Convert.ToInt32(tickerPrice[1]) == 2)  // Delayed Ask quote 67, if you want realtime use tickerPrice == 2
                {
                    LastTickDetails.Ask = Convert.ToDouble(tickerPrice[2]);
                    // Add the text string to the list box
                    OnTickPriceUpdated?.Invoke("Ask", tickerPrice[2]);

                }
                else if (Convert.ToInt32(tickerPrice[1]) == 1)  // Delayed Bid quote 66, if you want realtime use tickerPrice == 1
                {

                    LastTickDetails.Bid = Convert.ToDouble(tickerPrice[2]);
                    // Add the text string to the list box
                    OnTickPriceUpdated?.Invoke("Bid", tickerPrice[2]);

                }
            }
        }

        public void OnGetContractSamples(ContractDescription[] contractDescriptions)
        {
            USContracts = contractDescriptions
                .Select(x => x.Contract)
                .Where(x => x.Currency == "USD").ToList();

            var ContractIdentifiers = USContracts.Select(x =>
            {
                string description = x.Symbol + " (" + x.Description + ", " + (x.Exchange ?? "SMART") + " / " + x.PrimaryExch + ")";

                return new { ConId = x.ConId, Description = description };
            }).ToArray();

            OnContractSamplesReceived.Invoke(ContractIdentifiers);
        }

        public void UpdatePosition(string account, Contract contract, decimal position, double avgCost)
        {
            var existingPosition = OpenPositions.Find(p => p.Contract.Symbol == contract.Symbol
                                                                    && p.Contract.SecType == contract.SecType
                                                                    && p.Contract.Exchange == contract.Exchange);
            if (existingPosition != null)
            {
                existingPosition.PositionAmount = position;
                existingPosition.AverageCost = avgCost;
            }
            else
            {
                OpenPositions.Add(new Position
                {
                    Account = account,
                    Contract = contract,
                    PositionAmount = position,
                    AverageCost = avgCost
                });
            }

            if (CurrContract == null) return;
            if (contract.Symbol == CurrContract.Symbol && contract.SecType == CurrContract.SecType)
            {
                OnPositionChanged?.Invoke();
            }
        }

        public void UpdateOrder(Order order, Contract contract)
        {
            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Order = order;
                existingOrder.Contract = contract;
            }
            else
            {
                OpenOrders.Add(new OpenOrder
                {
                    Order = order,
                    Contract = contract
                });
            }
        }

        public void UpdateOrder(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            if (status == "Filled" || status == "Cancelled" || status == "ApiCancelled" || status == "Inactive")
            {
                RemoveOrder(orderId);
                return;
            }

            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == orderId);
            if (existingOrder != null)
            {
                existingOrder.Status = status;
                existingOrder.Filled = filled;
                existingOrder.Remaining = remaining;
                existingOrder.AvgFillPrice = avgFillPrice;
                existingOrder.PermId = permId;
                existingOrder.ParentId = parentId;
                existingOrder.LastFillPrice = lastFillPrice;
                existingOrder.ClientId = clientId;
                existingOrder.WhyHeld = whyHeld;
                existingOrder.MktCapPrice = mktCapPrice;
            }
        }

        public void RemoveOrder(int orderId)
        {
            OpenOrders.RemoveAll(o => o.Order.OrderId == orderId);
        }

        // Options: 

        public void GetOptionChainForCurrContract()
        {
            if (CurrContract == null) return;
            wrapper.ClientSocket.reqSecDefOptParams(0, CurrContract.Symbol, "", CurrContract.SecType, CurrContract.ConId);
        }

        public void OnGetOptionChainDetails(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            int.TryParse(multiplier, out var multiplierVal);
            OnOptionChainDetailsReceived?.Invoke(multiplierVal, expirations, strikes);
        }

        public void OnGetOptionChainDetailsEnd()
        {
            OnOptionChainDetailsReceivedEnd?.Invoke();
        }

        public void GetOptionContractData(string right, DateTime expiration, double strike)
        {
            Contract optionContract = new Contract
            {
                Symbol = CurrContract.Symbol,
                SecType = "OPT",
                Exchange = "SMART",
                Currency = CurrContract.Currency,
                Right = right,
                LastTradeDate = "expiration",
                Strike = strike,
            };

            wrapper.ClientSocket.reqContractDetails(0, optionContract);
        }
    }

    public class LastTickDetails
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Last { get; set; }
    }

    public class Position
    {
        public string Account { get; set; }
        public Contract Contract { get; set; }
        public decimal PositionAmount { get; set; }
        public double AverageCost { get; set; }
    }

    public class OpenOrder
    {
        public Order Order { get; set; }
        public Contract Contract { get; set; }
        public string Status { get; set; }
        public decimal Filled { get; set; }
        public decimal Remaining { get; set; }
        public double AvgFillPrice { get; set; }
        public long PermId { get; set; }
        public int ParentId { get; set; }
        public double LastFillPrice { get; set; }
        public int ClientId { get; set; }
        public string WhyHeld { get; set; }
        public double MktCapPrice { get; set; }
    }
}


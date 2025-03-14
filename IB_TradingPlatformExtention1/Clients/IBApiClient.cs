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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Principal;

namespace IB_TradingPlatformExtention1
{
    public class IBApiClient
    {

        public List<Contract> USContracts { get; private set; } = new List<Contract>();
        private double? TrailStopPrice = null;
        private EWrapperImpl wrapper;
        private EReader reader;
        private Thread apiThread;
        public int orderId = 0;


        // Events to notify the form when data changes
        public event Action<int, string, string> OnTickPriceUpdated;
        public event Action<bool> OnDelayedMarketData;
        public event Action<int, double, double> OnTickOptionComputationUpdated;
        public event Action OnPositionChanged;
        public event Action<string, string> OnContractSelected;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<string> OnLogError;

        public TradeInstrumentDetails CurrTradeInstrument = null;

        public IBApiClient()
        {
            // Instantiate the ibClient and pass the wrapper
            this.wrapper = new EWrapperImpl(this);
        }

        public void Connect(string host, int port, int clientId, int displayGroupId)
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

            // Notify the form that connection is established
            wrapper.ClientSocket.subscribeToGroupEvents(9002, displayGroupId);
            OnConnected?.Invoke();
        }

        public void Disconnect()
        {
            wrapper.ClientSocket.eDisconnect();
        }

        public void PlaceOrder(int contractIdx, string side, Keys modifierKeys, decimal totalQuantity, double lmtPriceOffset, int stopType, bool isOutsideRth, double stopPrice)
        {
            Contract currContract = CurrTradeInstrument?.Contract;
            if (currContract == null) return;
            LastTickDetails lastTickDetails = CurrTradeInstrument.LastTickDetails;
            Position pos = CurrTradeInstrument.Position;

            if (currContract == null) return;

            double lmtPrice = ((side == "BUY" && modifierKeys != Keys.Alt) || (side == "SELL" && modifierKeys == Keys.Alt) ?
                lastTickDetails.Ask : lastTickDetails.Bid) + (side == "BUY" ? lmtPriceOffset : -lmtPriceOffset);

            List<OpenOrder> currStopLossOrders = this.CurrTradeInstrument.GetStopLossOrdersForPosition();

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
                lastTickDetails.Bid - stopPrice : lastTickDetails.Ask + stopPrice;

                stopLossOrder = new Order
                {
                    OcaGroup = currContract.Symbol + "_" + currContract.SecType + "_" + orderId,
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
                    TrailStopPrice = trailStopPrice;
                    if (stopLossOrder.OrderType == "TRAIL LIMIT") stopLossOrder.LmtPriceOffset = -4 * lmtPriceOffset;
                }
                else if (stopType == 1)
                {
                    stopLossOrder.OrderType = (isOutsideRth) ? "STP LMT" : "STP";
                    if (stopLossOrder.OrderType == "STP LMT") stopLossOrder.LmtPrice = stopPrice - 4 * lmtPriceOffset;
                }
            }

            if (pos != null && pos.PositionAmount != 0)
            {
                // Is this order is for increasing or decreasing position size
                bool isIncreasePos = (pos.PositionAmount > 0 && side == "BUY") || (pos.PositionAmount < 0 && side == "SELL");
                order.Transmit = true;

                if (!isIncreasePos && pos.PositionAmount < totalQuantity) order.TotalQuantity = pos.PositionAmount;

                if (currStopLossOrders.Count > 0)
                {
                    Order currStopLossOrder = currStopLossOrders.First().Order;

                    if (currStopLossOrder.OrderType == "TRAIL" || currStopLossOrder.OrderType == "TRAIL LIMIT")
                    {
                        TrailStopPrice = currStopLossOrder.TrailStopPrice;
                    }

                    if (!isIncreasePos)
                    {
                        order.OcaGroup = currStopLossOrder.OcaGroup;
                    }
                    else
                    if (currContract.SecType == "OPT")
                    {
                        wrapper.ClientSocket.cancelOrder(currStopLossOrders.First().Order.OrderId, new OrderCancel());
                    }
                }

                
            }

            // Place the order
            wrapper.ClientSocket.placeOrder(order.OrderId, currContract, order);

            this.CurrTradeInstrument.UpdateOrder(currContract, order);

            // increase the order id value
            orderId++;

            if (!order.Transmit)
            {
                //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
                //to activate all its predecessors
                stopLossOrder.Transmit = true;
                wrapper.ClientSocket.placeOrder(stopLossOrder.OrderId, currContract, stopLossOrder);

                this.CurrTradeInstrument.UpdateOrder(currContract, stopLossOrder);

                orderId++;
            }
        }

        public void AdjustStopLoss(int contractIdx, bool isOutsideRth, int stopType, double stopPrice, double limitPriceOffset, bool keepTrailStopPrice = false)
        {
            Contract currContract = CurrTradeInstrument?.Contract;
            if (currContract == null) return;
            Position position = CurrTradeInstrument.Position;
            LastTickDetails lastTickDetails = CurrTradeInstrument.LastTickDetails;

            if (currContract == null) return;

            if (position == null || position.PositionAmount == 0) return;

            var stopLossOrders = this.CurrTradeInstrument.GetStopLossOrdersForPosition();
            double? trailStopPrice;
            if (keepTrailStopPrice && TrailStopPrice != null)
            {
                trailStopPrice = TrailStopPrice;

            } else
            {
                if (lastTickDetails != null)
                {
                    trailStopPrice = position.PositionAmount > 0 ?
                    lastTickDetails.Bid - stopPrice : lastTickDetails.Ask + stopPrice;
                }
                else
                {
                    trailStopPrice = position.PositionAmount > 0 ?
                    position.AverageCost - stopPrice : position.AverageCost + stopPrice;
                }
            
                if (stopType == 2) TrailStopPrice = trailStopPrice;
            }

            Order stopLoss = new Order
            {
                OcaGroup = currContract.Symbol + "_" + currContract.SecType + "_" + orderId,
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
                stopLoss.TrailStopPrice = (double)trailStopPrice;
                if (stopLoss.OrderType == "TRAIL LIMIT") stopLoss.LmtPriceOffset = 4 * limitPriceOffset;
            }
            else if (stopType == 1)
            {
                stopLoss.OrderType = (isOutsideRth) ? "STP LMT" : "STP";
                if (stopLoss.OrderType == "STP LMT") stopLoss.LmtPrice = stopPrice + (stopLoss.Action == "BUY" ? 4 : -4) * limitPriceOffset;
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
                    if (keepTrailStopPrice) { 
                        stopLoss.TrailStopPrice = stopLossOrders.First().Order.TrailStopPrice;
                        TrailStopPrice = stopLossOrders.First().Order.TrailStopPrice;
                    }
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

            wrapper.ClientSocket.placeOrder(stopLoss.OrderId, currContract, stopLoss);

            this.CurrTradeInstrument.UpdateOrder(currContract, stopLoss);

            orderId++;

        }

        public void ClosePositionForContract(int contractIdx, double tradePriceOffset, bool isOutsideRth)
        {
            Contract currContract = CurrTradeInstrument?.Contract;
            if (currContract == null) return;
            Position positionToClose = CurrTradeInstrument.Position;
            LastTickDetails lastTickDetails = CurrTradeInstrument.LastTickDetails;

            CancelAllOrdersForContract(contractIdx, false);

            // If a position exists, proceed to close it
            if (positionToClose != null && positionToClose.PositionAmount != 0)
            {
                string side = positionToClose.PositionAmount > 0 ? "SELL" : "BUY";

                double lmtPriceOffset = (double)((side == "BUY") ? 4 * tradePriceOffset : -4 * tradePriceOffset);
                double lmtPrice = (side == "BUY" ? lastTickDetails.Ask : lastTickDetails.Bid) + lmtPriceOffset;

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
                wrapper.ClientSocket.placeOrder(closeOrder.OrderId, currContract, closeOrder);

                this.CurrTradeInstrument.UpdateOrder(currContract, closeOrder);

                // increase the order id value
                orderId++;
            }
            else
            {
                Console.WriteLine("No open position found for the specified contract.");
            }
        }

        public void CancelLastOrderForContract()
        {
            if (CurrTradeInstrument == null) { return; }
            List<OpenOrder> openOrders = CurrTradeInstrument.OpenOrders;
            if (openOrders.Count == 0) return;

            OpenOrder prevOrder = openOrders
           //.Where(order => order.Contract.Symbol == CurrContract.Symbol
           //                && order.Contract.SecType == CurrContract.SecType
           //                && order.Contract.Exchange == CurrContract.Exchange)
           .OrderByDescending(order => order.Order.OrderId)
           .FirstOrDefault();

            //if (prevOrder != null)
            wrapper.ClientSocket.cancelOrder(prevOrder.Order.OrderId, new OrderCancel());
        }

        public void CancelAllOrdersForContract(int contractIdx, bool isGlobalCancel)
        {
            //if (CurrContract == null) return;
            if (isGlobalCancel)
            {
                wrapper.ClientSocket.reqGlobalCancel(new OrderCancel());

            }
            else
            {
                if (CurrTradeInstrument == null) { return; }
                //var ordersToCancel = OpenOrders
                //.Where(order => order.Contract.Symbol == CurrContract.Symbol &&
                //                order.Contract.SecType == CurrContract.SecType &&
                //                order.Contract.Exchange == CurrContract.Exchange)
                //.ToList();

                foreach (var openOrder in CurrTradeInstrument.OpenOrders)
                {
                    wrapper.ClientSocket.cancelOrder(openOrder.Order.OrderId, new OrderCancel());
                }
            }

        }

        public void OnDisplayGroupUpdated(int reqId, string contractInfo)
        {
            if (contractInfo == "none") return;
            string[] parts = contractInfo.Split('@');

            int conId = int.Parse(parts[0]);
            string exchange = parts[1];

            Contract contract = new Contract
            {
                ConId = conId,
                Exchange = exchange,
            };
            wrapper.ClientSocket.reqContractDetails(reqId, contract);
        }

        public void OnGetContractDetails(ContractDetails contractDetails)
        {
            InitTradeInstrument(-1, contractDetails.Contract);
            if(contractDetails.Contract.SecType == "OPT")
            {
                string[] ConNameParts = contractDetails.Contract.LocalSymbol.Split(new string[] {"   ", "  ", " "}, StringSplitOptions.None);
                string[] ConDetailsParts = ConNameParts[1].Split(new string[] {"C", "P"}, StringSplitOptions.None);
                string longName = "Option " + (contractDetails.Contract.Right == "C" ? "*CALL*" : "*PUT*") + " " + contractDetails.Contract.Strike + " " + ConDetailsParts[0];
                OnContractSelected?.Invoke(ConNameParts[0], longName);
            }
            else
            {
                OnContractSelected?.Invoke(contractDetails.Contract.LocalSymbol, contractDetails.LongName);
            }
        }

        public void OnError(string errorMessage)
        {
            OnLogError?.Invoke(errorMessage);
        }

        //public void SetEquityContract(int conId)
        //{
        //    Contract selectedContract = USContracts.Where(x => x.ConId == conId).FirstOrDefault();
        //    InitTradeInstrument(-1, selectedContract);
        //    OnContractSelected?.Invoke();
        //}

        //public void SetComboContract(List<int> comboContractIdices, List<int> comboContractQuantities)
        //{
        //    if (comboContractIdices.Count <= 1)
        //    {
        //        CurrTradeInstrument.Combo = null;
        //        wrapper.ClientSocket.cancelMktData(-2);
        //        return;
        //    };

        //    Contract underline = CurrTradeInstrument.GetCurrTradeInstrumentByIdx(-1)?.Contract;
        //    if (underline == null) { throw new Exception(); }

        //    Contract comboContract = new Contract()
        //    {
        //        Symbol = underline.Symbol,
        //        Currency = underline.Currency,
        //        SecType = "BAG",
        //        Exchange = "SMART",
        //        ComboLegs = new List<ComboLeg>()
        //    };

        //    for (int i = 0; i < comboContractIdices.Count; i++)
        //    {
        //        Contract currContract = CurrTradeInstrument.GetCurrTradeInstrumentByIdx(comboContractIdices[i])?.Contract;
        //        if (currContract == null) throw new Exception();

        //        ComboLeg leg = new ComboLeg()
        //        {
        //            ConId = currContract.ConId,
        //            Exchange = "SMART",
        //            Ratio = Math.Abs(comboContractQuantities[i]),
        //            Action = comboContractQuantities[i] > 0 ? "BUY" : "SELL"
        //        };

        //        comboContract.ComboLegs.Add(leg);
        //    }

        //    InitTradeInstrument(-2, comboContract);
        //}

        public void InitTradeInstrument(int idx, Contract contract, bool reqData = true)
        {

            //public void InitTradeInstrument(Contract selectedContract)
            //{
            //    if (selectedContract == null) return;
            //    TradeInstrumentDetails newTradeInstrument = new TradeInstrumentDetails
            //    {
            //        Contract = selectedContract
            //    };
            //}
            CurrTradeInstrument = new TradeInstrumentDetails
            {
                Contract = contract,
            };
            wrapper.ClientSocket.cancelMktData(idx);
            if (!reqData) return;
            wrapper.ClientSocket.cancelPositions();
            RequestMarketDataForContract(idx);
            wrapper.ClientSocket.reqPositions();
            wrapper.ClientSocket.reqOpenOrders();
        }

        public void RequestMarketDataForContract(int contractIdx)
        {
            Contract currContract = CurrTradeInstrument?.Contract;
            if (currContract == null) return;

            currContract.Exchange = currContract.Exchange ?? "SMART";
            // Create a new TagValueList object (for API version 9.71 and later) 
            List<TagValue> mktDataOptions = new List<TagValue>();

            // If using delayed market data subscription un-comment 
            // the line below to request delayed data
            wrapper.ClientSocket.reqMarketDataType(3);  // delayed data = 3 live = 1

            // Kick off the subscription for real-time data (add the mktDataOptions list for API v9.71)

            // For API v9.72 and higher, add one more parameter for regulatory snapshot
            wrapper.ClientSocket.reqMktData(contractIdx, currContract, "", false, false, mktDataOptions);

        }

        //#####################################################

        public void OnGetTickPrice(int reqId, int fieldId, double price, TickAttrib attribs)
        {
            string fieldName = "";

            List<int> delayedTypes = new List<int> { 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 80, 81, 82, 83, 88, 103, 104};
            OnDelayedMarketData?.Invoke(delayedTypes.Contains(fieldId));

            switch (fieldId)
            {
                case 66:
                case 1: // Delayed Bid quote 66, if you want realtime use tickerPrice == 1
                    fieldName = "Bid";
                    break;
                case 67:
                case 2: // Delayed Ask quote 67, if you want realtime use tickerPrice == 2
                    fieldName = "Ask";
                    break;
                case 68:
                case 4: // Delayed Last quote 68, if you want realtime use tickerPrice == 4
                    fieldName = "Last";
                    break;
                default:
                    break;


                    //what tick types do we need? 
                    // Bid price - 1, ask price 2, last price - 4, close price -9 if nothing else is available,
                    // option computation: bid - 10, ask - 11, last - 12, model -13. needs one of them, probably model but need to verify.
                    // option implied volatility - 24, not sure what is that becuase there is IV for bid, ask, last, model I think.
                    // option call open interest - 27, option put open interest - 28. also not sure what is that. 
                    // option call volume - 29, option put volume - 30, maybe gonna need it. 
                    // mark price - 37, maybe. 
                    // custom option computation - 53, not think it's needed but just to know this is option computation based on some custom price.
                    // Last RTH Trade - 57, maybe if nothing else is available. 
                    // Delayed: Bid - 66, ask - 67, last - 68.
                    // Delayed close - 75, maybe if nothing else is available. 
                    // Delayed option computation: bid - 80, ask - 81, last - 82, model - 83, 

            }

            CurrTradeInstrument.UpdateLastTickDetails(fieldName, price);
            OnTickPriceUpdated?.Invoke(reqId, fieldName, price.ToString());
        }

        public void OnConnectionClosed()
        {
            OnDisconnected?.Invoke();
        }

        public void OnGetPositions(string account, Contract contract, decimal pos, double avgCost)
        {
            if (CurrTradeInstrument.Contract.ConId != contract.ConId) return;
            CurrTradeInstrument.UpdatePosition(account, contract, pos, avgCost);
            OnPositionChanged?.Invoke();
        }

        public void OnGetOpenOrders(Contract contract, Order order)
        {
            if (CurrTradeInstrument?.Contract?.ConId != contract.ConId) return;
            CurrTradeInstrument?.UpdateOrder(contract, order);
        }

        public void OnGetOrderStatus(int _orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            CurrTradeInstrument?.UpdateOrder(_orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld, mktCapPrice);
        }

        // ###################################### Options: 

        public void OnGetTickOptionComputation(int tickerId, int field, int tickAttrib, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {

            List<int> delayedTypes = new List<int> { 80, 81, 82, 83 };
            OnDelayedMarketData.Invoke(delayedTypes.Contains(field));

            //what tick types do we need? 
            // Bid price - 1, ask price 2, last price - 4, close price -9 if nothing else is available,
            // option computation: bid - 10, ask - 11, last - 12, model -13. needs one of them, probably model but need to verify.
            // option implied volatility - 24, not sure what is that becuase there is IV for bid, ask, last, model I think.
            // option call open interest - 27, option put open interest - 28. also not sure what is that. 
            // option call volume - 29, option put volume - 30, maybe gonna need it. 
            // mark price - 37, maybe. 
            // custom option computation - 53, not think it's needed but just to know this is option computation based on some custom price.
            // Last RTH Trade - 57, maybe if nothing else is available. 
            // Delayed: Bid - 66, ask - 67, last - 68.
            // Delayed close - 75, maybe if nothing else is available. 
            // Delayed option computation: bid - 80, ask - 81, last - 82, model - 83, 
            //if (field == 13 || field == 83)
            //{
            //    OnTickOptionComputationUpdated.Invoke(tickerId, impliedVolatility, delta);
            //}
        }
    }

    public class TradeInstrumentDetails
    {
        public Contract Contract { get; set; }
        public LastTickDetails LastTickDetails { get; set; }
        public Position Position { get; set; }
        public List<OpenOrder> OpenOrders { get; set; } = new List<OpenOrder>();

        public List<OpenOrder> GetStopLossOrdersForPosition()
        {
            if (Position == null) return null;
            return OpenOrders.Where(order =>
                //order.Contract.Symbol == position.Contract.Symbol &&
                //order.Contract.SecType == position.Contract.SecType &&
                //order.Contract.Exchange == position.Contract.Exchange &&
                (order.Order.OrderType == "STP" ||
                 order.Order.OrderType == "STP LMT" ||
                 order.Order.OrderType == "TRAIL" ||
                 order.Order.OrderType == "TRAIL LIMIT") &&
                 ((order.Order.Action == "BUY" && Position.PositionAmount < 0) ||
                 (order.Order.Action == "SELL" && Position.PositionAmount > 0)))
                .ToList();
        }

        public void UpdateLastTickDetails(string fieldName, double price)
        {
            if (LastTickDetails == null)
            {
                LastTickDetails = new LastTickDetails();
            }

            switch (fieldName)
            {
                case "Bid":
                    LastTickDetails.Bid = price;
                    break;
                case "Ask":
                    LastTickDetails.Ask = price;
                    break;
                case "Last":
                    LastTickDetails.Last = price;
                    break;
                default:
                    break;
            }
        }

        public void UpdatePosition(string account, Contract contract, decimal position, double avgCost)
        {
            if (Position != null)
            {
                Position.PositionAmount = position;
                Position.AverageCost = avgCost;
            }
            else
            {
                Position = new Position
                {
                    Account = account,
                    PositionAmount = position,
                    AverageCost = avgCost
                };
            }
        }

        public void UpdateOrder(Contract contract, Order order)
        {
            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Order = order;
            }
            else
            {
                OpenOrders
                    .Add(new OpenOrder
                    {
                        Order = order,
                    });
            }
        }

        public void UpdateOrder(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            var existingOrder = OpenOrders.Find(o => o.Order.OrderId == orderId);
            if (existingOrder == null) return;

            if (status == "Filled" || status == "Cancelled" || status == "ApiCancelled" || status == "Inactive")
            {
                RemoveOrder(orderId);
                return;
            }

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

        public void RemoveOrder(int orderId)
        {
            OpenOrders.RemoveAll(o => o.Order.OrderId == orderId);
        }

        // Override Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (TradeInstrumentDetails)obj;

            // Check if Contract is not null and compare relevant properties
            return Contract != null &&
                   other.Contract != null &&
                   Contract.Symbol == other.Contract.Symbol &&
                   Contract.SecType == other.Contract.SecType &&
                   Contract.Exchange == other.Contract.Exchange &&
                   Contract.LastTradeDateOrContractMonth == other.Contract.LastTradeDateOrContractMonth &&
                   Contract.Strike == other.Contract.Strike;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            if (Contract == null)
                return 0;

            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Contract.Symbol?.GetHashCode() ?? 0);
                hash = hash * 23 + (Contract.SecType?.GetHashCode() ?? 0);
                hash = hash * 23 + (Contract.Exchange?.GetHashCode() ?? 0);
                hash = hash * 23 + (Contract.LastTradeDateOrContractMonth?.GetHashCode() ?? 0);
                hash = hash * 23 + Contract.Strike.GetHashCode();
                return hash;
            }
        }

        // Overload == operator
        public static bool operator ==(TradeInstrumentDetails left, TradeInstrumentDetails right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        // Overload != operator
        public static bool operator !=(TradeInstrumentDetails left, TradeInstrumentDetails right)
        {
            return !(left == right);
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
        public decimal PositionAmount { get; set; }
        public double AverageCost { get; set; }
    }

    public class OpenOrder
    {
        public Order Order { get; set; }
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


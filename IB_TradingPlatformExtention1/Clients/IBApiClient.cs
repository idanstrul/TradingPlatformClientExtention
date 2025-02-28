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
        public event Action<int> OnPositionChanged;
        public event Action<object[]> OnContractSamplesReceived;
        public event Action OnContractSelected;
        public event Action<int, HashSet<string>, HashSet<double>> OnOptionChainDetailsReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public TradeInstruments CurrTradeInstruments = new TradeInstruments();

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

            // Notify the form that connection is established
            OnConnected?.Invoke();
        }

        public void Disconnect()
        {
            wrapper.ClientSocket.eDisconnect();
        }

        public void PlaceOrder(int contractIdx, string side, Keys modifierKeys, decimal totalQuantity, double lmtPriceOffset, int stopType, bool isOutsideRth, double stopPrice)
        {
            TradeInstrumentDetails details = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx);
            Contract currContract = details?.Contract;
            if (currContract == null) return;
            LastTickDetails lastTickDetails = details.LastTickDetails;
            Position pos = details.Position;

            if (currContract == null) return;

            double lmtPrice = ((side == "BUY" && modifierKeys != Keys.Alt) || (side == "SELL" && modifierKeys == Keys.Alt) ?
                lastTickDetails.Ask : lastTickDetails.Bid) + lmtPriceOffset;

            List<OpenOrder> currStopLossOrders = CurrTradeInstruments.GetStopLossOrdersForPosition(contractIdx);

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
                    OcaGroup = contractIdx.ToString(), // currContract.Symbol + "_" + currContract.SecType + "_" + orderId,
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
            wrapper.ClientSocket.placeOrder(order.OrderId, currContract, order);

            CurrTradeInstruments.UpdateOrder(currContract, order);

            // increase the order id value
            orderId++;

            if (!order.Transmit)
            {
                //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
                //to activate all its predecessors
                stopLossOrder.Transmit = true;
                wrapper.ClientSocket.placeOrder(stopLossOrder.OrderId, currContract, stopLossOrder);

                CurrTradeInstruments.UpdateOrder(currContract, stopLossOrder);

                orderId++;
            }
        }

        public void AdjustStopLoss(int contractIdx, bool isOutsideRth, int stopType, double stopPrice, double limitPriceOffset)
        {
            TradeInstrumentDetails details = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx);
            Contract currContract = details?.Contract;
            if (currContract == null) return;
            Position position = details.Position;
            LastTickDetails lastTickDetails = details.LastTickDetails;

            if (currContract == null) return;

            if (position == null || position.PositionAmount == 0) return;

            var stopLossOrders = CurrTradeInstruments.GetStopLossOrdersForPosition(contractIdx);

            double? trailStopPrice = TrailStopPrice;
            if (trailStopPrice == null)
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

            }

            Order stopLoss = new Order
            {
                OcaGroup = contractIdx.ToString(), //currContract.Symbol + "_" + currContract.SecType + "_" + orderId,
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

            wrapper.ClientSocket.placeOrder(stopLoss.OrderId, currContract, stopLoss);

            TrailStopPrice = null;
            CurrTradeInstruments.UpdateOrder(currContract, stopLoss);

            orderId++;

        }

        public void ClosePositionForContract(int contractIdx, double tradePriceOffset, bool isOutsideRth)
        {
            TradeInstrumentDetails details = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx);
            Contract currContract = details?.Contract;
            if (currContract == null) return;
            Position positionToClose = details.Position;
            LastTickDetails lastTickDetails = details.LastTickDetails;

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

                CurrTradeInstruments.UpdateOrder(currContract, closeOrder);

                // increase the order id value
                orderId++;
            }
            else
            {
                Console.WriteLine("No open position found for the specified contract.");
            }
        }

        public void CancelLastOrderForContract(int contractIdx)
        {
            TradeInstrumentDetails details = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx);
            if (details == null) { return; }
            List<OpenOrder> openOrders = details.OpenOrders;
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
            TradeInstrumentDetails details = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx);

            if (isGlobalCancel)
            {
                wrapper.ClientSocket.reqGlobalCancel(new OrderCancel());

            }
            else
            {
                if (details == null) { return; }
                //var ordersToCancel = OpenOrders
                //.Where(order => order.Contract.Symbol == CurrContract.Symbol &&
                //                order.Contract.SecType == CurrContract.SecType &&
                //                order.Contract.Exchange == CurrContract.Exchange)
                //.ToList();

                foreach (var openOrder in details.OpenOrders)
                {
                    wrapper.ClientSocket.cancelOrder(openOrder.Order.OrderId, new OrderCancel());
                }
            }

        }

        // ######################################

        public void SearchStockContracts(string symbol)
        {
            wrapper.ClientSocket.reqMatchingSymbols(-3, symbol);
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

        public void SetEquityContract(int conId)
        {
            Contract selectedContract = USContracts.Where(x => x.ConId == conId).FirstOrDefault();
            InitTradeInstrument(-1, selectedContract);
            OnContractSelected?.Invoke();
        }

        public void SetComboContract(List<int> comboContractIdices, List<int> comboContractQuantities)
        {
            if (comboContractIdices.Count <= 1)
            {
                CurrTradeInstruments.Combo = null;
                wrapper.ClientSocket.cancelMktData(-2);
                return;
            };

            Contract underline = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(-1)?.Contract;
            if (underline == null) { throw new Exception(); }

            Contract comboContract = new Contract()
            {
                Symbol = underline.Symbol,
                Currency = underline.Currency,
                SecType = "BAG",
                Exchange = "SMART",
                ComboLegs = new List<ComboLeg>()
            };

            for (int i = 0; i < comboContractIdices.Count; i++)
            {
                Contract currContract = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(comboContractIdices[i])?.Contract;
                if (currContract == null) throw new Exception();

                ComboLeg leg = new ComboLeg()
                {
                    ConId = currContract.ConId,
                    Exchange = "SMART",
                    Ratio = Math.Abs(comboContractQuantities[i]),
                    Action = comboContractQuantities[i] > 0 ? "BUY" : "SELL"
                };

                comboContract.ComboLegs.Add(leg);
            }

            InitTradeInstrument(-2, comboContract);
        }

        public void InitTradeInstrument(int idx, Contract contract, bool reqData = true)
        {
            CurrTradeInstruments.InitTradeInstrument(idx, contract);
            wrapper.ClientSocket.cancelMktData(idx);
            if (!reqData) return;
            wrapper.ClientSocket.cancelPositions();
            RequestMarketDataForContract(idx);
            wrapper.ClientSocket.reqPositions();
            wrapper.ClientSocket.reqOpenOrders();
        }

        public void RequestMarketDataForContract(int contractIdx)
        {
            Contract currContract = CurrTradeInstruments.GetCurrTradeInstrumentByIdx(contractIdx)?.Contract;
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

            List<int> delayedTypes = new List<int> { 66, 67, 68 };
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

            CurrTradeInstruments.UpdateLastTickDetails(reqId, fieldName, price);
            OnTickPriceUpdated?.Invoke(reqId, fieldName, price.ToString());
        }

        public void OnConnectionClosed()
        {
            OnDisconnected?.Invoke();
        }

        public void OnGetPositions(string account, Contract contract, decimal pos, double avgCost)
        {
            int posIdx = CurrTradeInstruments.GetCurrTradeInstrumentIdxForContract(contract);
            CurrTradeInstruments.UpdatePosition(account, contract, pos, avgCost);
            OnPositionChanged?.Invoke(posIdx);
        }

        public void OnGetOpenOrders(Contract contract, Order order)
        {
            CurrTradeInstruments.UpdateOrder(contract, order);
        }

        public void OnGetOrderStatus(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            CurrTradeInstruments.UpdateOrder(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld, mktCapPrice);
        }

        // ###################################### Options: 

        public void GetOptionChain()
        {
            Contract underline = CurrTradeInstruments.Equity?.Contract;
            if (underline == null) return;
            wrapper.ClientSocket.reqSecDefOptParams(0, underline.Symbol, "", underline.SecType, underline.ConId);
        }

        public void OnGetOptionChainDetails(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            if (exchange != "SMART") return;
            int.TryParse(multiplier, out var multiplierVal);
            OnOptionChainDetailsReceived?.Invoke(multiplierVal, expirations, strikes);
        }

        public void SetOptionLegContract(int reqId, string right, string expiration, double strike)
        {
            Contract optionContract = new Contract
            {
                Symbol = CurrTradeInstruments.Equity.Contract.Symbol,
                SecType = "OPT",
                Exchange = "SMART",
                Currency = CurrTradeInstruments.Equity.Contract.Currency,
                Right = right,
                LastTradeDateOrContractMonth = expiration,
                Strike = strike,
            };

            InitTradeInstrument(reqId, optionContract, false);
            wrapper.ClientSocket.reqContractDetails(reqId, optionContract);
        }

        public void OnGetOptionContractDetails(int reqId, ContractDetails contractDetails)
        {
            List<TradeInstrumentDetails> currOptionStrategy = CurrTradeInstruments.OptionLegs;
            if (reqId >= currOptionStrategy.Count) return;
            Contract c1 = currOptionStrategy[reqId].Contract;
            if (c1 == null) return;
            Contract c2 = contractDetails.Contract;
            if (c1.Symbol != c2.Symbol || c1.LastTradeDateOrContractMonth != c2.LastTradeDateOrContractMonth || c1.Strike != c2.Strike) return;
            // Replace the existing item at the index
            InitTradeInstrument(reqId, contractDetails.Contract);
        }

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
            if (field == 13 || field == 83)
            {
                OnTickOptionComputationUpdated.Invoke(tickerId, impliedVolatility, delta);
            }
        }

        public void RemoveOptionLeg(int legIdx)
        {
            if (legIdx >= CurrTradeInstruments.OptionLegs.Count) return;
            wrapper.ClientSocket.cancelMktData(legIdx);
            CurrTradeInstruments.OptionLegs.RemoveAt(legIdx);
        }
    }

    public class TradeInstruments
    {
        public TradeInstrumentDetails Equity { get; set; }
        public List<TradeInstrumentDetails> OptionLegs { get; set; } = new List<TradeInstrumentDetails>();
        public TradeInstrumentDetails Combo { get; set; }

        public List<TradeInstrumentDetails> GetCurrTradeInstrumentsAsList()
        {
            List<TradeInstrumentDetails> currTradeInstruments = new List<TradeInstrumentDetails>
            {
                Combo,
                Equity
            };
            if (OptionLegs != null) currTradeInstruments.AddRange(OptionLegs);
            return currTradeInstruments;
        }

        public int GetCurrTradeInstrumentIdxForContract(Contract contract)
        {
            List<TradeInstrumentDetails> currTradeInstruments = GetCurrTradeInstrumentsAsList();
            int idx = currTradeInstruments.FindIndex(x => CheckIfContractsEqual(x?.Contract, contract));
            return idx - 2;
        }

        public bool CheckIfContractsEqual(Contract contract1, Contract contract2)
        {
            if (contract1 == null || contract2 == null) return false;

            bool isEqual = contract1 == contract2;

            return contract1.ConId == contract2.ConId ||
                (contract1.Symbol == contract2.Symbol
                && contract1.SecType == contract2.SecType
                && (contract1.Exchange == "SMART" || contract1.Exchange == contract2.Exchange)
                && contract1.LastTradeDateOrContractMonth == contract2.LastTradeDateOrContractMonth
                && contract1.Strike == contract2.Strike);
        }

        public TradeInstrumentDetails GetCurrTradeInstrumentByIdx(int idx)
        {
            switch (idx)
            {
                case -3:
                    return null;
                case -2:
                    return Combo;
                case -1:
                    return Equity;
                default:
                    return OptionLegs[idx];
            }
        }

        public TradeInstrumentDetails GetTradeInsrumentForOrderId(int orderId)
        {
            List<TradeInstrumentDetails> currTradeInstruments = GetCurrTradeInstrumentsAsList();
            return currTradeInstruments.Find(ti =>
            {
                if (ti?.OpenOrders is null) return false;
                return ti.OpenOrders.Exists(o => o.Order.OrderId == orderId);
            });
        }


        public void InitTradeInstrument(int idx, Contract selectedContract)
        {
            if (selectedContract == null) return;
            TradeInstrumentDetails newTradeInstrument = new TradeInstrumentDetails
            {
                Contract = selectedContract
            };
            switch (idx)
            {
                //case -3:
                //    return null;
                case -2:
                    Combo = newTradeInstrument;
                    break;
                case -1:
                    Equity = newTradeInstrument;
                    break;
                default:
                    if (idx == OptionLegs.Count) OptionLegs.Add(newTradeInstrument);
                    else OptionLegs[idx] = newTradeInstrument;
                    break;
            }
            //if (Equity == null) Equity = new TradeInstrumentDetails();
            //Equity.Contract = selectedContract;
            //OnContractSelected?.Invoke();
        }

        public List<OpenOrder> GetStopLossOrdersForPosition(int positionIdx)
        {
            TradeInstrumentDetails details = GetCurrTradeInstrumentByIdx(positionIdx);
            Position position = details.Position;
            List<OpenOrder> openOrders = details.OpenOrders;
            if (position == null) return null;
            return openOrders.Where(order =>
                //order.Contract.Symbol == position.Contract.Symbol &&
                //order.Contract.SecType == position.Contract.SecType &&
                //order.Contract.Exchange == position.Contract.Exchange &&
                (order.Order.OrderType == "STP" ||
                 order.Order.OrderType == "STP LMT" ||
                 order.Order.OrderType == "TRAIL" ||
                 order.Order.OrderType == "TRAIL LIMIT") &&
                 ((order.Order.Action == "BUY" && position.PositionAmount < 0) ||
                 (order.Order.Action == "SELL" && position.PositionAmount > 0)))
                .ToList();
        }

        public void UpdateLastTickDetails(int reqId, string fieldName, double price)
        {
            TradeInstrumentDetails currTradeInstrument = GetCurrTradeInstrumentByIdx(reqId);
            if (currTradeInstrument == null) return;
            if (currTradeInstrument.LastTickDetails == null)
            {
                currTradeInstrument.LastTickDetails = new LastTickDetails();
            }

            switch (fieldName)
            {
                case "Bid":
                    currTradeInstrument.LastTickDetails.Bid = price;
                    break;
                case "Ask":
                    currTradeInstrument.LastTickDetails.Ask = price;
                    break;
                case "Last":
                    currTradeInstrument.LastTickDetails.Last = price;
                    break;
                default:
                    break;
            }
        }

        public void UpdatePosition(string account, Contract contract, decimal position, double avgCost)
        {
            int posIdx = GetCurrTradeInstrumentIdxForContract(contract);
            TradeInstrumentDetails currTradeInstrument = GetCurrTradeInstrumentByIdx(posIdx);

            if (currTradeInstrument == null) return;
            if (currTradeInstrument.Position != null)
            {
                currTradeInstrument.Position.PositionAmount = position;
                currTradeInstrument.Position.AverageCost = avgCost;
            }
            else
            {
                currTradeInstrument.Position = new Position
                {
                    Account = account,
                    PositionAmount = position,
                    AverageCost = avgCost
                };
            }
        }

        public void UpdateOrder(Contract contract, Order order)
        {
            int idx = GetCurrTradeInstrumentIdxForContract(contract);
            TradeInstrumentDetails currTradeInstrument1 = GetCurrTradeInstrumentByIdx(idx);
            TradeInstrumentDetails currTradeInstrument2 = GetTradeInsrumentForOrderId(order.OrderId);
            if (currTradeInstrument1 == null) return;
            if (currTradeInstrument2 != null && currTradeInstrument1 != currTradeInstrument2)
            {
                throw new Exception();
            }
            var existingOrder = currTradeInstrument1.OpenOrders.Find(o => o.Order.OrderId == order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Order = order;
            }
            else
            {
                currTradeInstrument1.OpenOrders
                    .Add(new OpenOrder
                    {
                        Order = order,
                    });
            }
        }

        public void UpdateOrder(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            TradeInstrumentDetails currTradeInstrument = GetTradeInsrumentForOrderId(orderId);
            if (currTradeInstrument == null) return;

            if (status == "Filled" || status == "Cancelled" || status == "ApiCancelled" || status == "Inactive")
            {
                RemoveOrder(orderId);
                return;
            }

            var existingOrder = currTradeInstrument.OpenOrders.Find(o => o.Order.OrderId == orderId);
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
            TradeInstrumentDetails currTradeInstrument = GetTradeInsrumentForOrderId(orderId);
            if (currTradeInstrument == null) return;
            currTradeInstrument.OpenOrders.RemoveAll(o => o.Order.OrderId == orderId);
        }
    }

    public class TradeInstrumentDetails
    {
        public Contract Contract { get; set; }
        public LastTickDetails LastTickDetails { get; set; }
        public Position Position { get; set; }
        public List<OpenOrder> OpenOrders { get; set; } = new List<OpenOrder>();

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


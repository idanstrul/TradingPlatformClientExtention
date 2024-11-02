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
        public List<Position> OpenPositions { get; private set; } = new List<Position>();
        public List<OpenOrder> OpenOrders { get; private set; } = new List<OpenOrder>();
        private EWrapperImpl wrapper;
        private EReader reader;
        private Thread apiThread;
        public int orderId = 0;


        // Events to notify the form when data changes
        public event Action<string, string> OnTickPriceUpdated;
        public event Action<string, string> OnPositionChanged;
        public event Action OnConnected;
        public event Action OnDisconnected;

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

        public void GetData(myContract _contract)
        {
            wrapper.ClientSocket.cancelMktData(1); // cancel market data

            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();
            // Create a new TagValueList object (for API version 9.71 and later) 
            List<TagValue> mktDataOptions = new List<TagValue>();

            // Set the underlying stock symbol fromthe cbSymbol combobox            
            contract.Symbol = _contract.Symbol;
            // Set the Security type to STK for a Stock
            contract.SecType = _contract.SecType;
            // Use "SMART" as the general exchange
            contract.Exchange = _contract.Exchange;
            // Set the primary exchange (sometimes called Listing exchange)
            // Use either NYSE or ISLAND
            contract.PrimaryExch = _contract.PrimaryExch;
            // Set the currency to USD
            contract.Currency = _contract.Currency;

            // If using delayed market data subscription un-comment 
            // the line below to request delayed data
            wrapper.ClientSocket.reqMarketDataType(1);  // delayed data = 3 live = 1

            // Kick off the subscription for real-time data (add the mktDataOptions list for API v9.71)

            // For API v9.72 and higher, add one more parameter for regulatory snapshot
            wrapper.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);

        }

        public Position GetPositionForContract(myContract currContract)
        {
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

        public void PlaceOrder(myContract c, myOrder o)
        {
            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();

            // Set the underlying stock symbol from the cbSymbol combobox
            contract.Symbol = c.Symbol;
            // Set the Security type to STK for a Stock
            contract.SecType = c.SecType;
            // Use "SMART" as the general exchange
            contract.Exchange = c.Exchange;
            // Set the primary exchange (sometimes called Listing exchange)
            // Use either NYSE or ISLAND
            contract.PrimaryExch = c.PrimaryExch;
            // Set the currency to USD
            contract.Currency = c.Currency;


            // Create a new order
            Order order = new Order();
            // gets the next order id from the text box
            if(!string.IsNullOrEmpty(o.OcaGroupName)) order.OcaGroup = o.OcaGroupName;
            order.OcaType = 2;
            order.OrderId = o.OrderId;
            // gets the side of the order (BUY, or SELL)
            order.Action = o.Action;
            // gets order type from combobox market or limit order(MKT, or LMT)
            order.OrderType = o.OrderType;
            // number of shares from Quantity
            order.TotalQuantity = o.TotalQuantity;
            // Value from limit price
            order.LmtPrice = o.LmtPrice;
            //order.OutsideRth = cbOutsideRTH.Checked;
            order.OutsideRth = o.OutsideRth;


            if (o.AttachStop)
            {
                order.Transmit = false;
            }

            // Place the order
            wrapper.ClientSocket.placeOrder(order.OrderId, contract, order);

            // increase the order id value
            orderId++;
        }

        public void PlaceStopLossOrder(myContract c, StopLossOrder o)
        {
            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();

            // Set the underlying stock symbol from the cbSymbol combobox
            contract.Symbol = c.Symbol;
            // Set the Security type to STK for a Stock
            contract.SecType = c.SecType;
            // Use "SMART" as the general exchange
            contract.Exchange = c.Exchange;
            // Set the primary exchange (sometimes called Listing exchange)
            // Use either NYSE or ISLAND
            contract.PrimaryExch = c.PrimaryExch;
            // Set the currency to USD
            contract.Currency = c.Currency;

            Order stopLoss = new Order();
            if (o.ParentId > 0) stopLoss.ParentId = o.ParentId;
            if(!string.IsNullOrEmpty(o.OcaGroupName)) stopLoss.OcaGroup = o.OcaGroupName;
            stopLoss.OcaType = 2;
            stopLoss.OrderId = o.OrderId;
            stopLoss.Action = o.Action;
            stopLoss.TriggerMethod = 7;
            stopLoss.OutsideRth = o.OutsideRth;
            stopLoss.TotalQuantity = o.TotalQuantity;
            if (o.StopType == 2)
            {
                stopLoss.OrderType = (o.OutsideRth) ? "TRAIL LIMIT" : "TRAIL";
                stopLoss.TrailStopPrice = o.TrailStopPrice;
                stopLoss.AuxPrice = (double)o.StopPrice;
                if (stopLoss.OrderType == "TRAIL LIMIT") stopLoss.LmtPriceOffset = o.StopLimitPriceOffset;
            }
            else if (o.StopType == 1)
            {
                stopLoss.OrderType = (o.OutsideRth) ? "STP LMT" : "STP"; //or "STP LMT"
                stopLoss.AuxPrice = o.StopPrice;
                if (stopLoss.OrderType == "STP LMT") stopLoss.LmtPrice = o.StopPrice + o.StopLimitPriceOffset;
            }
            //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
            //to activate all its predecessors
            stopLoss.Transmit = true;
            wrapper.ClientSocket.placeOrder(stopLoss.OrderId, contract, stopLoss);

            orderId++;
        }

        public void CancelOrder(int orderId)
        {
            wrapper.ClientSocket.cancelOrder(orderId, new OrderCancel());
        }

        public void CancelAllOrders()
        {
            wrapper.ClientSocket.reqGlobalCancel(new OrderCancel());
        }

        public void CancelAllOrdersForContract(myContract currContract)
        {
            var ordersToCancel = OpenOrders
                .Where(order => order.Contract.Symbol == currContract.Symbol &&
                                order.Contract.SecType == currContract.SecType &&
                                order.Contract.Exchange == currContract.Exchange)
                .ToList();

            foreach (var openOrder in ordersToCancel)
            {
                CancelOrder(openOrder.Order.OrderId);
            }
        }

        public void OnGetTickPrice(string tickPrice)
        {
            //myform.AddTextBoxItemTickPrice(tickPrice);
            string[] tickerPrice = new string[] { tickPrice };
            tickerPrice = tickPrice.Split(',');

            if (Convert.ToInt32(tickerPrice[0]) == 1)
            {
                if (Convert.ToInt32(tickerPrice[1]) == 4)// Delayed Last quote 68, if you want realtime use tickerPrice == 4
                {
                    // Add the text string to the list box

                    OnTickPriceUpdated?.Invoke("Last", tickerPrice[2]);
                    //this.tbLast.Text = tickerPrice[2];

                }
                else if (Convert.ToInt32(tickerPrice[1]) == 2)  // Delayed Ask quote 67, if you want realtime use tickerPrice == 2
                {
                    // Add the text string to the list box

                    OnTickPriceUpdated?.Invoke("Ask", tickerPrice[2]);
                    //this.tbAsk.Text = tickerPrice[2];

                }
                else if (Convert.ToInt32(tickerPrice[1]) == 1)  // Delayed Bid quote 66, if you want realtime use tickerPrice == 1
                {
                    // Add the text string to the list box

                    OnTickPriceUpdated?.Invoke("Bid", tickerPrice[2]);
                    //this.tbBid.Text = tickerPrice[2];

                }
            }
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

            OnPositionChanged?.Invoke(contract.Symbol, contract.SecType);
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


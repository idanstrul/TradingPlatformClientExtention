using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using IBApi;

namespace IB_TradingPlatformExtention1
{
    public partial class Form1 : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a ListBox control.
        delegate void SetTextCallback(string text);
        delegate void SetTextCallbackTickPrice(string _tickPrice);

        int order_id = 0;

        public List<Position> OpenPositions { get; private set; } = new List<Position>();
        public List<Order> OpenOrders { get; private set; } = new List<Order>();

        // Create the ibClient object to represent the connection
        EWrapperImpl ibClient;
        public Form1()
        {
            InitializeComponent();

            // instantiate the ibClient
            ibClient = new EWrapperImpl();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Parameters to connect to TWS are:
            // host       - IP address or host name of the host running TWS
            // port       - listening port 7496 or 7497
            // clientId   - client application identifier can be any number
            ibClient.ClientSocket.eConnect("127.0.0.1", 7496, 0);

            var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
            reader.Start();
            new Thread(() =>
            {
                while (ibClient.ClientSocket.IsConnected())
                {
                    ibClient.Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();
            // Wait until the connection is completed
            while (ibClient.NextOrderId <= 0) { }

            // Set up the form object in the EWrapper
            ibClient.myform = (Form1)Application.OpenForms[0];

            // updates the order_id value
            order_id = ibClient.NextOrderId;

            ibClient.ClientSocket.reqPositions();

            getData();
        }

        public void AddTextBoxItemTickPrice(string _tickPrice)
        {
            if (this.tbLast.InvokeRequired)
            {
                SetTextCallbackTickPrice d = new SetTextCallbackTickPrice(AddTextBoxItemTickPrice);
                try
                {
                    this.Invoke(d, new object[] { _tickPrice });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from tickPrice", e);
                }
            }
            else
            {

                string[] tickerPrice = new string[] { _tickPrice };
                tickerPrice = _tickPrice.Split(',');

                if (Convert.ToInt32(tickerPrice[0]) == 1)
                {
                    if (Convert.ToInt32(tickerPrice[1]) == 4)// Delayed Last quote 68, if you want realtime use tickerPrice == 4
                    {
                        // Add the text string to the list box

                        this.tbLast.Text = tickerPrice[2];

                    }
                    else if (Convert.ToInt32(tickerPrice[1]) == 2)  // Delayed Ask quote 67, if you want realtime use tickerPrice == 2
                    {
                        // Add the text string to the list box

                        this.tbAsk.Text = tickerPrice[2];

                    }
                    else if (Convert.ToInt32(tickerPrice[1]) == 1)  // Delayed Bid quote 66, if you want realtime use tickerPrice == 1
                    {
                        // Add the text string to the list box

                        this.tbBid.Text = tickerPrice[2];

                    }
                }
            }
        }

        private void getData()
        {
            ibClient.ClientSocket.cancelMktData(1); // cancel market data

            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();
            // Create a new TagValueList object (for API version 9.71 and later) 
            List<TagValue> mktDataOptions = new List<TagValue>();

            // Set the underlying stock symbol fromthe cbSymbol combobox            
            contract.Symbol = cbSymbol.Text;
            // Set the Security type to STK for a Stock
            contract.SecType = "STK";
            // Use "SMART" as the general exchange
            contract.Exchange = "SMART";
            // Set the primary exchange (sometimes called Listing exchange)
            // Use either NYSE or ISLAND
            contract.PrimaryExch = "ISLAND";
            // Set the currency to USD
            contract.Currency = "USD";

            // If using delayed market data subscription un-comment 
            // the line below to request delayed data
            ibClient.ClientSocket.reqMarketDataType(1);  // delayed data = 3 live = 1

            // Kick off the subscription for real-time data (add the mktDataOptions list for API v9.71)

            // For API v9.72 and higher, add one more parameter for regulatory snapshot
            ibClient.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);

        }

        private void cbSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            getData();
        }

        private void cbSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLower(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void cbSymbol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbSymbol.SelectionStart = 0;
                cbSymbol.SelectionLength = cbSymbol.Text.Length;

                e.SuppressKeyPress = true;

                string name = cbSymbol.Text;


                // adds the security symbol to the dropdown list in the symbol combobox
                if (!cbSymbol.Items.Contains(name))
                {
                    cbSymbol.Items.Add(name);
                }
                cbSymbol.SelectAll();

                getData();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            ibClient.ClientSocket.eDisconnect();
        }

        private void btnBuy1_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            send_order(side, modifierKeys, posSize);
        }

        private void btnBuy1_2_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            send_order(side, modifierKeys, posSize);
        }

        private void btnBuy1_4_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            send_order(side, modifierKeys, posSize);
        }

        private void btnBuy1_8_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            send_order(side, modifierKeys, posSize);
        }

        private void btnSell1_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            send_order(side, modifierKeys, posSize);
        }

        private void btnSell1_2_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            send_order(side, modifierKeys, posSize);
        }

        private void btnSell1_4_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            send_order(side, modifierKeys, posSize);
        }

        private void btnSell1_8_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            send_order(side, modifierKeys, posSize);
        }

        public void send_order(string side, Keys modifierKeys, decimal posSize)
        {
            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();

            // Set the underlying stock symbol from the cbSymbol combobox
            contract.Symbol = cbSymbol.Text;
            // Set the Security type to STK for a Stock
            contract.SecType = "STK";
            // Use "SMART" as the general exchange
            contract.Exchange = cbMarket.Text;
            // Set the primary exchange (sometimes called Listing exchange)
            // Use either NYSE or ISLAND
            contract.PrimaryExch = "ISLAND";
            // Set the currency to USD
            contract.Currency = "USD";


            // Create a new order
            Order order = new Order();
            // gets the next order id from the text box
            order.OrderId = order_id;
            // gets the side of the order (BUY, or SELL)
            order.Action = side;
            // gets order type from combobox market or limit order(MKT, or LMT)
            order.OrderType = (modifierKeys == Keys.Control) ? "MKT" : "LMT";
            // number of shares from Quantity
            order.TotalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value));
            double lmtPriceOffset = (double)((side == "buy") ? this.numTradeOffset.Value : -this.numTradeOffset.Value);
            // Value from limit price
            order.LmtPrice = ((side == "buy" && modifierKeys != Keys.Alt) || (side == "sell" && modifierKeys == Keys.Alt) ?
                Convert.ToDouble(tbAsk.Text) : Convert.ToDouble(tbBid.Text)) + lmtPriceOffset;
            //order.OutsideRth = cbOutsideRTH.Checked;
            order.OutsideRth = chkOutside.Checked;


            if (this.cbTrailStop.Checked || this.cbStopLoss.Checked)
            {
                order.Transmit = false;
            }

            // Place the order
            ibClient.ClientSocket.placeOrder(order.OrderId, contract, order);


            if (this.cbTrailStop.Checked || this.cbStopLoss.Checked)
            {
                Order stopLoss = new Order();
                stopLoss.ParentId = order_id;
                stopLoss.OrderId = ++order_id;
                stopLoss.Action = side == "buy" ? "SELL" : "BUY";
                stopLoss.TriggerMethod = 7;
                stopLoss.OutsideRth = chkOutside.Checked;
                if (this.cbTrailStop.Checked)
                {
                    stopLoss.OrderType = (chkOutside.Checked) ? "TRAIL LIMIT" : "TRAIL";
                    stopLoss.TrailStopPrice = 0.00;
                    stopLoss.AuxPrice = (double)numTrailStop.Value;
                    if (stopLoss.OrderType == "TRAIL LIMIT") stopLoss.LmtPriceOffset = - 4 * (double)numTradeOffset.Value;
                }
                else if (this.cbStopLoss.Checked)
                {
                    stopLoss.OrderType = (chkOutside.Checked)? "STP LMT" : "STP"; //or "STP LMT"
                    stopLoss.AuxPrice = (double)numStopLoss.Value;
                    if (stopLoss.OrderType == "STP LMT") stopLoss.LmtPrice = (double)numStopLoss.Value - 4 * (double)numTradeOffset.Value;
                }               
                stopLoss.TotalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value));
                //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
                //to activate all its predecessors
                stopLoss.Transmit = true;
                ibClient.ClientSocket.placeOrder(stopLoss.OrderId, contract, stopLoss);
            }

            // increase the order id value
            order_id++;
        }

        private void btnCancelLast_Click(object sender, EventArgs e)
        {
            // cancels last order
            ibClient.ClientSocket.cancelOrder(order_id - 1, new OrderCancel());
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            // cancels all the orders
            ibClient.ClientSocket.reqGlobalCancel(new OrderCancel());
        }

        private void cbTrailStop_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTrailStop.Checked)
            {
                this.cbStopLoss.Checked = false;
            }
        }

        private void cbStopLoss_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStopLoss.Checked)
            {
                this.cbTrailStop.Checked = false;
            }
        }

        private void btnClosePos_Click(object sender, EventArgs e)
        {
            ibClient.ClientSocket.reqAllOpenOrders();
        }
    }
}

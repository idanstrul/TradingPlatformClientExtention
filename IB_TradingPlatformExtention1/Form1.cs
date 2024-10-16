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
        int timer1_counter = 5;

        public void AddListBoxItem(string text)
        {
            // See if a new invocation is required form a different thread            
            if (this.lbData.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddListBoxItem);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // Add the text string to the list box
                this.lbData.Items.Add(text);
            }
        }

        // Create the ibClient object to represent the connection
        IB_TradingPlatformExtention1.EWrapperImpl ibClient;
        public Form1()
        {
            InitializeComponent();

            // instantiate the ibClient
            ibClient = new IB_TradingPlatformExtention1.EWrapperImpl();
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
            new Thread(() => {
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

            // gets the next order id and puts it in the textbox 
            tbValid_id.Text = ibClient.NextOrderId.ToString();
            // updates the order_id value
            order_id = ibClient.NextOrderId;

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

            // used to clear the contents of the listView
            listViewTns.Items.Clear();

            // Create a new contract to specify the security we are searching for
            IBApi.Contract contract = new IBApi.Contract();
            // Create a new TagValueList object (for API version 9.71 and later) 
            List<IBApi.TagValue> mktDataOptions = new List<IBApi.TagValue>();

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
            ibClient.ClientSocket.reqMktData(1, contract, "233", false, false, mktDataOptions);

            timer1.Start();
        }

        delegate void SetTextCallbackTickString(string _tickString);

        public void AddListViewItemTickString(string _tickString)
        {
            if (this.listViewTns.InvokeRequired)
            {
                try
                {
                    SetTextCallbackTickString d = new SetTextCallbackTickString(AddListViewItemTickString);
                    this.Invoke(d, new object[] { _tickString });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    // get the bid price from the textbox Bid
                    double theBid = Convert.ToDouble(tbBid.Text);
                    // gets the ask price from the textbox Ask
                    double theAsk = Convert.ToDouble(tbAsk.Text);

                    // Contains Last Price, Trade Size, Trade Time, Total Volume, VWAP, 
                    // single trade flag true, or false.
                    // 6 items all together
                    // example 701.28;1;1348075471534;67854;701.46918464;true
                    // extract each value from string and store it in a string list
                    string[] listTimeSales = _tickString.Split(';');

                    // get the first value form the list convert it to a double this value is the last price
                    double last_price = Convert.ToDouble(listTimeSales[0]);

                    int trade_size = Convert.ToInt32(listTimeSales[1]);

                    double trade_time = Convert.ToDouble(listTimeSales[2]);

                    // adds 2 zeros to the trade size
                    int share_size = trade_size * 100;

                    // formats a string to commas
                    string strShareSize = share_size.ToString("###,####,##0");

                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    epoch = epoch.AddMilliseconds(trade_time);
                    // *************************************************
                    epoch = epoch.AddHours(-5);   //Daylight saving time use -4 Summer otherwise use -5 Winter

                    string strSaleTime = epoch.ToString("h:mm:ss:ff");

                    double myMeanPrice = ((theAsk - theBid) / 2);
                    double myMean = (theBid + myMeanPrice);

                    ListViewItem lx = new ListViewItem();

                    // string dt = String.Format("{0:hh:mm:ss}", dnt);

                    // if the last price is the same as the ask change the color to green
                    if (last_price == theAsk)
                    {
                        lx.ForeColor = Color.Green; // listview foreground color
                        lx.Text = (listTimeSales[0]); // last price
                        lx.SubItems.Add(strShareSize); // share size
                        lx.SubItems.Add(strSaleTime); // time
                        listViewTns.Items.Insert(0, lx); // use Insert instead of Add listView.Items.Add(li); 
                    }
                    // if the last price is the same as the bid change the color to red
                    else if (last_price == theBid)
                    {
                        lx.ForeColor = Color.Red;
                        lx.Text = (listTimeSales[0]);
                        lx.SubItems.Add(strShareSize);
                        lx.SubItems.Add(strSaleTime);
                        listViewTns.Items.Insert(0, lx);

                        lbData.Items.Insert(0, strSaleTime);
                    }
                    // if the last price in greater than the mean price and
                    // less than the ask price change the color to lime green
                    else if (last_price > myMean && last_price < theAsk)
                    {
                        lx.ForeColor = Color.Lime;
                        lx.Text = (listTimeSales[0]);
                        lx.SubItems.Add(strShareSize);
                        lx.SubItems.Add(strSaleTime);
                        listViewTns.Items.Insert(0, lx);

                        lbData.Items.Add(epoch);
                    }
                    else
                    {
                        lx.ForeColor = Color.DarkRed;
                        lx.Text = (listTimeSales[0]);
                        lx.SubItems.Add(strShareSize);
                        lx.SubItems.Add(strSaleTime);
                        listViewTns.Items.Insert(0, lx);
                    }
                }
                catch
                {

                }
            }
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

        private void btnSell_Click(object sender, EventArgs e)
        {
            string side = "sell";

            if (Form.ModifierKeys == Keys.Control)
            {
                send_bracket_order(side);
            }
            else
            {
                send_order(side);
            }
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            string side = "buy";

            if (Form.ModifierKeys == Keys.Control)
            {
                send_bracket_order(side);
            }
            else
            {
                send_order(side);
            }
        }

        public void send_bracket_order(string side)
        {
            // create a new contract
            IBApi.Contract contract = new IBApi.Contract();
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

            // order_id, action (Buy or Sell), Quantity, entryPrice, targetPrice, stopLoss, order_type

            string order_type = cbOrderType.Text; // order type LMT or STP from the combobox
            string action = side; // side (BUY or SELL) passed on from the button click event
            decimal quantity = Convert.ToDecimal(numQuantity.Value); // number of shares
            double lmtPrice = Convert.ToDouble(numPrice.Text);  // limit price from numeric up down box on the form
            double takeProfit = Convert.ToDouble(tbPtofitTarget.Text);  // take profit amount from text box on the form
            double stopLoss = Convert.ToDouble(tbStopLoss.Text);  // stop loss from the text box on the form

            // side is the either buy or sell
            // calls a BracketOrder function and stores the results in a list variable called bracket
            List<Order> bracket = BracketOrder(order_id++, action, quantity, lmtPrice, takeProfit, stopLoss, order_type);
            foreach (Order o in bracket) // loops through each order in the list
                ibClient.ClientSocket.placeOrder(o.OrderId, contract, o);

            // increase the order id number by 3 so you don't use the same order id number twice,
            // and get an error
            order_id += 3;

        }

        public static List<Order> BracketOrder(int parentOrderId, string action, decimal quantity, double limitPrice,
            double takeProfitLimitPrice, double stopLossPrice, string order_type)
        {
            //This will be our main or "parent" order
            Order parent = new Order();
            parent.OrderId = parentOrderId;
            parent.Action = action;  // "BUY" or "SELL"
            parent.OrderType = order_type;  // "LMT", "STP", or "STP LMT"
            parent.TotalQuantity = quantity;
            parent.LmtPrice = limitPrice;
            //The parent and children orders will need this attribute set to false to prevent accidental executions.
            //The LAST CHILD will have it set to true
            parent.Transmit = false;

            // Profit Target order
            Order takeProfit = new Order();
            takeProfit.OrderId = parent.OrderId + 1;
            takeProfit.Action = action.Equals("BUY") ? "SELL" : "BUY"; // if statement
            takeProfit.OrderType = "LMT";
            takeProfit.TotalQuantity = quantity;
            takeProfit.LmtPrice = takeProfitLimitPrice;
            takeProfit.ParentId = parentOrderId;
            takeProfit.Transmit = false;

            // Stop loss order
            Order stopLoss = new Order();
            stopLoss.OrderId = parent.OrderId + 2;
            stopLoss.Action = action.Equals("BUY") ? "SELL" : "BUY";
            stopLoss.OrderType = "STP"; //or "STP LMT"
            //Stop trigger price
            // add stopLoss.LmtPrice here if you are going to use a stop limit order
            stopLoss.AuxPrice = stopLossPrice;
            stopLoss.TotalQuantity = quantity;
            stopLoss.ParentId = parentOrderId;
            //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true
            //to activate all its predecessors
            stopLoss.Transmit = true;
            List<Order> bracketOrder = new List<Order>();
            bracketOrder.Add(parent);
            bracketOrder.Add(takeProfit);
            bracketOrder.Add(stopLoss);
            return bracketOrder;
        }

        public void send_order(string side)
        {
            // Create a new contract to specify the security we are searching for
            IBApi.Contract contract = new IBApi.Contract();

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

            IBApi.Order order = new IBApi.Order();
            // gets the next order id from the text box
            order.OrderId = order_id;
            // gets the side of the order (BUY, or SELL)
            order.Action = side;
            // gets order type from combobox market or limit order(MKT, or LMT)
            order.OrderType = cbOrderType.Text;
            // number of shares from Quantity
            order.TotalQuantity = Convert.ToDecimal(numQuantity.Value);
            // Value from limit price
            order.LmtPrice = Convert.ToDouble(numPrice.Value);
            // checks for a stop order
            if (cbOrderType.Text == "STP")
            {
                // Stop order value from the limit textbox
                order.AuxPrice = Convert.ToDouble(numPrice.Value);
            }
            //Visible shares to the market
            order.DisplaySize = Convert.ToInt32(tbVisible.Text);
            //order.OutsideRth = cbOutsideRTH.Checked;
            order.OutsideRth = chkOutside.Checked;

            // Place the order
            ibClient.ClientSocket.placeOrder(order_id, contract, order);

            // increase the order id value
            order_id++;
            tbValid_id.Text = Convert.ToString(order_id);
        }

        private void tbBid_Click(object sender, EventArgs e)
        {
            numPrice.Value = Convert.ToDecimal(tbBid.Text);
        }

        private void tbAsk_Click(object sender, EventArgs e)
        {
            numPrice.Value = Convert.ToDecimal(tbAsk.Text);
        }

        private void tbLast_Click(object sender, EventArgs e)
        {
            numPrice.Value = Convert.ToDecimal(tbLast.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1_counter == 0)
            {
                timer1.Stop(); // stop the timer
                //add the bid price to the limit box
                numPrice.Value = Convert.ToDecimal(tbBid.Text);
                timer1_counter = 5;// resets timer counter back to 5
            }
            timer1_counter--; // subtract 1 every time their is a tick
        }
    }
}

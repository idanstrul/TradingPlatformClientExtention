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
using System.Diagnostics.Contracts;
using System.Security.Policy;
using IBApi;

namespace IB_TradingPlatformExtention1
{
    public partial class Form1 : Form
    {

        private IBApiClient client;

        // This delegate enables asynchronous calls for setting
        // the text property on a ListBox control.
        delegate void SetTextCallbackTickPrice(string field, string price);

        // Create the ibClient object to represent the connection
        public Form1()
        {
            InitializeComponent();

            client = new IBApiClient();

            client.OnTickPriceUpdated += Client_OnTickPriceUpdated;
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
        }

        private void Client_OnDisconnected()
        {
            throw new NotImplementedException();
        }

        private void Client_OnConnected()
        {
            if (cbSymbol.Text.Trim() == "") return;
            myContract contract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };
            client.GetData(contract);

        }

        private void Client_OnTickPriceUpdated(string field, string price)
        {
            if (this.tbLast.InvokeRequired)
            {
                SetTextCallbackTickPrice d = new SetTextCallbackTickPrice(Client_OnTickPriceUpdated);
                try
                {
                    this.Invoke(d, new object[] { field, price });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from Client_OnTickPriceUpdated", e);
                }
            }
            else
            {

                switch (field)
                {
                    case "Last": 
                        this.tbLast.Text = price; 
                        break;
                    case "Ask": 
                        this.tbAsk.Text = price;
                        break;
                    case "Bid": 
                        this.tbBid.Text = price;
                        break;
                    default:
                        break;
                }

            }
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
            client.Connect("127.0.0.1", 7496, 0);
        }

        private void cbSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSymbol.Text.Trim() == "") return;
            myContract contract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };
            client.GetData(contract);
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
                cbSymbol.SelectionLength = cbSymbol.Text.Trim().Length;

                e.SuppressKeyPress = true;

                string name = cbSymbol.Text.Trim();

                if (name == "") return;

                // adds the security symbol to the dropdown list in the symbol combobox
                if (!cbSymbol.Items.Contains(name))
                {
                    cbSymbol.Items.Add(name);
                }
                cbSymbol.SelectAll();

                myContract contract = new myContract
                {
                    Symbol = name,
                    SecType = "STK",
                    Exchange = "SMART",
                    PrimaryExch = "ISLAND",
                    Currency = "USD"
                };
                client.GetData(contract);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }

        private void btnBuy1_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_2_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_4_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_8_Click(object sender, EventArgs e)
        {
            string side = "buy";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_2_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_4_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_8_Click(object sender, EventArgs e)
        {
            string side = "sell";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            placeOrder(side, modifierKeys, posSize);
        }

        public void placeOrder(string side, Keys modifierKeys, decimal posSize)
        {
            // Create a new contract to specify the security we are searching for
            myContract contract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = cbMarket.Text,
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            double lmtPriceOffset = (double)((side == "buy") ? this.numTradeOffset.Value : -this.numTradeOffset.Value);
            double lmtPrice = ((side == "buy" && modifierKeys != Keys.Alt) || (side == "sell" && modifierKeys == Keys.Alt) ?
                Convert.ToDouble(tbAsk.Text) : Convert.ToDouble(tbBid.Text)) + lmtPriceOffset;

            int stopType = 0;
            
            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            myOrder order = new myOrder
            {
                Action = side,
                OrderType = (modifierKeys == Keys.Control) ? "MKT" : "LMT",
                TotalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value)),
                LmtPrice = lmtPrice,
                OutsideRth = chkOutside.Checked,
                StopType = stopType,
                StopPrice = (double)numTrailStop.Value,
                StopLimitPriceOffset = -4 * (double)numTradeOffset.Value
            };

            client.PlaceOrder(contract, order);

        }

        private void btnCancelLast_Click(object sender, EventArgs e)
        {
            client.CancelPreviousOrder();
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            client.CancelAllOrders();
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
            myContract currContract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            // Find the position for the given contract
            var positionToClose = client.OpenPositions.FirstOrDefault(p => p.Contract.Symbol == currContract.Symbol
                                                                    && p.Contract.SecType == currContract.SecType
                                                                    && p.Contract.Exchange == currContract.Exchange);

            // If a position exists, proceed to close it
            if (positionToClose != null && positionToClose.PositionAmount != 0)
            {
                string side = positionToClose.PositionAmount > 0 ? "SELL" : "BUY";

                double lmtPriceOffset = (double)((side == "BUY") ? 4 * this.numTradeOffset.Value : -4 * this.numTradeOffset.Value);
                double lmtPrice = (side == "BUY" ? Convert.ToDouble(tbAsk.Text) : Convert.ToDouble(tbBid.Text)) + lmtPriceOffset;

                // Define a new order to close the position by taking the opposite action
                myOrder closeOrder = new myOrder
                {
                    Action = side, 
                    OrderType = chkOutside.Checked? "LMT" : "MKT",
                    TotalQuantity = Math.Abs(positionToClose.PositionAmount), 
                    LmtPrice = lmtPrice,
                    OutsideRth = chkOutside.Checked, 
                    StopType = 0
                };

                client.PlaceOrder(currContract, closeOrder);
            }
            else
            {
                Console.WriteLine("No open position found for the specified contract.");
            }
        }
    }

    public class myContract
    {
        public string Symbol { get; set; }
        public string SecType { get; set; }
        public string Exchange { get; set; }
        public string PrimaryExch { get; set; }
        public string Currency { get; set; }
    }

    public class myOrder
    {
        public string Action { get; set; }
        public string OrderType { get; set; }
        public decimal TotalQuantity { get; set; }
        public double LmtPrice { get; set; }
        public bool OutsideRth { get; set; }
        public int StopType { get; set; } // 0 - No stop, 1 - STP, 2 - TRAIL
        public double StopPrice { get; set; }
        public double StopLimitPriceOffset { get; set; }
    }
}

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
using System.Diagnostics;

namespace IB_TradingPlatformExtention1
{
    public partial class Form1 : Form
    {

        private IBApiClient client;

        // This delegate enables asynchronous calls for setting
        // the text property on a ListBox control.
        delegate void SetTextCallbackTickPrice(int reqId, string field, string price);
        delegate void SetCallbackContractSamplesRecived(object[] contractIdentifiers);

        // Create the ibClient object to represent the connection
        public Form1()
        {
            InitializeComponent();

            client = new IBApiClient();

            client.OnTickPriceUpdated += Client_OnTickPriceUpdated;
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnPositionChanged += Client_OnPositionChanged;
            client.OnContractSamplesReceived += Client_OnContractSamplesReceived;
        }

        private void Client_OnContractSamplesReceived(object[] contractIdentifiers)
        {
            if (this.cbSymbol.InvokeRequired)
            {
                SetCallbackContractSamplesRecived d = new SetCallbackContractSamplesRecived(Client_OnContractSamplesReceived);
                try
                {
                    this.Invoke(d, new object[] { contractIdentifiers });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from Client_OnContractSamplesRecived", e);
                }
            }
            else
            {
                cbSymbol.Items.Clear();
                cbSymbol.Items.AddRange(contractIdentifiers);
                cbSymbol.DroppedDown = true;
            }
        }

        private void Client_OnPositionChanged(int posIdx)
        {
            if (posIdx != -1) return;
            AdjustStopLoss();
        }

        private void Client_OnDisconnected()
        {
            throw new NotImplementedException();
        }

        private void Client_OnConnected()
        {
            //throw new NotImplementedException();

        }

        private void Client_OnTickPriceUpdated(int reqId, string field, string price)
        {
            if (reqId != -1) return;

            if (this.tbLast.InvokeRequired)
            {
                SetTextCallbackTickPrice d = new SetTextCallbackTickPrice(Client_OnTickPriceUpdated);
                try
                {
                    this.Invoke(d, new object[] { reqId, field, price });
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

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();
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
                client.SearchStockContracts(cbSymbol.Text.Trim());
            }
        }

        private void btnBuy1_Click(object sender, EventArgs e)
        {
            string side = "BUY";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_2_Click(object sender, EventArgs e)
        {
            string side = "BUY";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_4_Click(object sender, EventArgs e)
        {
            string side = "BUY";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnBuy1_8_Click(object sender, EventArgs e)
        {
            string side = "BUY";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_Click(object sender, EventArgs e)
        {
            string side = "SELL";
            Keys modifierKeys = Form.ModifierKeys;
            int posSize = 1;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_2_Click(object sender, EventArgs e)
        {
            string side = "SELL";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.5m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_4_Click(object sender, EventArgs e)
        {
            string side = "SELL";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.25m;

            placeOrder(side, modifierKeys, posSize);
        }

        private void btnSell1_8_Click(object sender, EventArgs e)
        {
            string side = "SELL";
            Keys modifierKeys = Form.ModifierKeys;
            decimal posSize = 0.125m;

            placeOrder(side, modifierKeys, posSize);
        }

        public void placeOrder(string side, Keys modifierKeys, decimal posSize)
        {
            double lmtPriceOffset = (double)((side == "BUY") ? this.numTradeOffset.Value : -this.numTradeOffset.Value);

            int stopType = 0;
            bool isOutsideRth = chkOutside.Checked;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            decimal totalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value));

            double stopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value;


            client.PlaceOrder(-1, side, modifierKeys, totalQuantity, lmtPriceOffset, stopType, isOutsideRth, stopPrice);
        }

        private void btnStopLossAdj_Click(object sender, EventArgs e)
        {
            AdjustStopLoss();
        }

        private void AdjustStopLoss()
        {
            bool isOutsideRth = chkOutside.Checked;
            int stopType = 0;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            double stopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value;

            client.AdjustStopLoss(-1, isOutsideRth, stopType, stopPrice, (double)numTradeOffset.Value);

        }

        private void btnClosePos_Click(object sender, EventArgs e)
        {
            client.ClosePositionForContract(-1, (double)this.numTradeOffset.Value, chkOutside.Checked);
        }

        private void btnCancelLast_Click(object sender, EventArgs e)
        {
            client.CancelLastOrderForContract(-1);
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            Keys modifierKeys = Form.ModifierKeys;
            client.CancelAllOrdersForContract(-1, modifierKeys == Keys.Control);
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
                numStopLoss.Value = Math.Round((decimal.Parse(tbAsk.Text) + decimal.Parse(tbBid.Text)) / 2, 2);
            }
        }

        private void btnOptionsAnalysis_Click(object sender, EventArgs e)
        {
            decimal stockLastPrice = decimal.Parse(this.tbLast.Text);
            OptionsAnalysisForm OAform = new OptionsAnalysisForm(client, stockLastPrice);
            OAform.Show();
        }

        private void cbSymbol_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var selectedItem = cbSymbol.SelectedItem as dynamic;
            client.SetEquityContract(selectedItem.ConId);
        }
    }
}

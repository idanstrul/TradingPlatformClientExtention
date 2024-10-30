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
            // Create a new contract to specify the security we are searching for
            myContract contract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = cbMarket.Text,
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            double lmtPriceOffset = (double)((side == "BUY") ? this.numTradeOffset.Value : -this.numTradeOffset.Value);
            double lmtPrice = ((side == "BUY" && modifierKeys != Keys.Alt) || (side == "SELL" && modifierKeys == Keys.Alt) ?
                Convert.ToDouble(tbAsk.Text) : Convert.ToDouble(tbBid.Text)) + lmtPriceOffset;

            int stopType = 0;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            Position pos = client.GetPositionForContract(contract);
            OpenOrder currStopLossOrder = client.GetStopLossOrderForPosition(pos);

            myOrder order = new myOrder
            {
                AttachStop = stopType > 0,
                OrderId = client.orderId,
                Action = side,
                OrderType = (modifierKeys == Keys.Control) ? "MKT" : "LMT",
                TotalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value)),
                LmtPrice = lmtPrice,
                OutsideRth = chkOutside.Checked,
            };
            StopLossOrder stopLossOrder = null;

            if (stopType > 0)
            {
                stopLossOrder = new StopLossOrder
                {
                    ParentId = client.orderId,
                    OrderId = client.orderId + 1,
                    Action = side == "BUY" ? "SELL" : "BUY",
                    TotalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value)),
                    OutsideRth = chkOutside.Checked,
                    StopType = stopType,
                    StopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value,
                    StopLimitPriceOffset = -4 * (double)numTradeOffset.Value
                };
            }

            if (pos == null)
            {
                client.PlaceOrder(contract, order);
                if (stopType > 0) client.PlaceStopLossOrder(contract, stopLossOrder);
            }
            else
            {
                // Is this order is for increasing or decreasing position size
                bool isIncreasePos = (pos.PositionAmount > 0 && side == "BUY") || (pos.PositionAmount < 0 && side == "SELL");
                if (currStopLossOrder == null)
                {
                    if (isIncreasePos)  
                    {
                        // Same as with no pos
                        client.PlaceOrder(contract, order);
                        if (stopType > 0) client.PlaceStopLossOrder(contract, stopLossOrder);
                    } else
                    {
                        // Do not place stop loss order
                        order.AttachStop = false;
                        client.PlaceOrder(contract, order);
                    }
                } else
                {
                    if (isIncreasePos)
                    {
                        // Adjust stop loss for entire position
                        client.PlaceOrder(contract, order);
                        if (stopType > 0)
                        {
                            stopLossOrder.ParentId = currStopLossOrder.ParentId; // or should it be unchanged?
                            stopLossOrder.OrderId = currStopLossOrder.Order.OrderId;
                            stopLossOrder.TotalQuantity += Math.Abs(pos.PositionAmount);
                            client.PlaceStopLossOrder(contract, stopLossOrder);
                        }
                    }
                    else
                    {
                        // do not place stop loss order and add this order to same oca group as the stop loss that currently exist
                        order.AttachStop = false;
                        client.PlaceOrder(contract, order);
                    }
                }
            }


        }

        private void btnCancelLast_Click(object sender, EventArgs e)
        {
            myContract currContract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            OpenOrder prevOrder = client.OpenOrders
           .Where(order => order.Contract.Symbol == currContract.Symbol
                           && order.Contract.SecType == currContract.SecType
                           && order.Contract.Exchange == currContract.Exchange)
           .OrderByDescending(order => order.Order.OrderId)
           .FirstOrDefault();

            if (prevOrder != null) client.CancelOrder(prevOrder.Order.OrderId);
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            Keys modifierKeys = Form.ModifierKeys;
            myContract currContract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            if (modifierKeys == Keys.Control) client.CancelAllOrders();
            else client.CancelAllOrdersForContract(currContract);
            
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

            client.CancelAllOrdersForContract(currContract);

            // Find the position for the given contract
            var positionToClose = client.GetPositionForContract(currContract);

            // If a position exists, proceed to close it
            if (positionToClose != null && positionToClose.PositionAmount != 0)
            {
                string side = positionToClose.PositionAmount > 0 ? "SELL" : "BUY";

                double lmtPriceOffset = (double)((side == "BUY") ? 4 * this.numTradeOffset.Value : -4 * this.numTradeOffset.Value);
                double lmtPrice = (side == "BUY" ? Convert.ToDouble(tbAsk.Text) : Convert.ToDouble(tbBid.Text)) + lmtPriceOffset;

                // Define a new order to close the position by taking the opposite action
                myOrder closeOrder = new myOrder
                {
                    OrderId = client.orderId,
                    Action = side,
                    OrderType = chkOutside.Checked ? "LMT" : "MKT",
                    TotalQuantity = Math.Abs(positionToClose.PositionAmount),
                    LmtPrice = lmtPrice,
                    OutsideRth = chkOutside.Checked,
                };

                client.PlaceOrder(currContract, closeOrder);
            }
            else
            {
                Console.WriteLine("No open position found for the specified contract.");
            }
        }

        private void btnStopLossAdj_Click(object sender, EventArgs e)
        {
            myContract currContract = new myContract
            {
                Symbol = cbSymbol.Text.Trim(),
                SecType = "STK",
                Exchange = "SMART",
                PrimaryExch = "ISLAND",
                Currency = "USD"
            };

            var position = client.GetPositionForContract(currContract);

            if (position == null || position.PositionAmount == 0) return;

            var stopLossOrder = client.GetStopLossOrderForPosition(position);

            int stopType = 0;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            if (stopLossOrder != null)
            {
                if (stopType == 0)
                {
                    client.CancelOrder(stopLossOrder.Order.OrderId);
                    return;
                }

                StopLossOrder _stopLossOrder = new StopLossOrder
                {
                    ParentId = stopLossOrder.ParentId,
                    OrderId = stopLossOrder.Order.OrderId,
                    Action = stopLossOrder.Order.Action,
                    TotalQuantity = Math.Abs(position.PositionAmount),
                    OutsideRth = chkOutside.Checked,
                    StopType = stopType,
                    StopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value,
                    StopLimitPriceOffset = -4 * (double)numTradeOffset.Value
                };

                client.PlaceStopLossOrder(currContract, _stopLossOrder);
            }
            else
            {
                if (stopType == 0) return;

                StopLossOrder _stopLossOrder = new StopLossOrder
                {
                    OrderId = client.orderId,
                    Action = position.PositionAmount > 0 ? "SELL" : "BUY",
                    TotalQuantity = Math.Abs(position.PositionAmount),
                    OutsideRth = chkOutside.Checked,
                    StopType = stopType,
                    StopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value,
                    StopLimitPriceOffset = -4 * (double)numTradeOffset.Value
                };

                client.PlaceStopLossOrder(currContract, _stopLossOrder);
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
        //public bool isTakeProfit { get; set; } = false;
        public bool AttachStop { get; set; } = false;
        public int OrderId { get; set; }
        public string Action { get; set; }
        public string OrderType { get; set; }
        public decimal TotalQuantity { get; set; }
        public double LmtPrice { get; set; }
        public bool OutsideRth { get; set; }
    }

    public class StopLossOrder
    {
        public int ParentId { get; set; } = 0;
        public int OrderId { get; set; }
        public string Action { get; set; }
        public decimal TotalQuantity { get; set; }
        public bool OutsideRth { get; set; }
        public int StopType { get; set; } // 0 - No stop, 1 - STP, 2 - TRAIL
        public double StopPrice { get; set; }
        public double StopLimitPriceOffset { get; set; }
    }
}

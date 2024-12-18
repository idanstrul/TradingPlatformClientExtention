﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace IB_TradingPlatformExtention1
{
    public partial class OptionsAnalysisForm : Form
    {
        delegate void InitOptionLegCallback(int rowIdx);
        delegate void OnOptionChainDetailsReceivedEndCallback(int multiplier, HashSet<string> expirations, HashSet<double> strikes);
        delegate void SetOptionLegTickPriceCallback(int rowIdx, string fieldName, string value);
        delegate void SetOptionLegTickOptionComputationCallback(int idx, double impliedVolatility, double delta);

        private VerticalLineAnnotation verticalLine;
        private HorizontalLineAnnotation zeroLine;
        private EllipseAnnotation intersectionDot;
        private TextAnnotation priceAnnotation;
        private TextAnnotation pnlAnnotation;
        private TextAnnotation probabilityAnnotation;


        public List<OptionLeg> optionLegs = new List<OptionLeg>();
        public OptionChainDetails optionChainDetails = new OptionChainDetails();
        public double strategyInitialCost = 0;

        private IBApiClient client;

        public OptionsAnalysisForm(IBApiClient _client, decimal stockLastPrice)
        {
            InitializeComponent();

            client = _client;

            client.OnOptionChainDetailsReceived += Client_OnOptionChainDetailsReceived;
            client.OnTickPriceUpdated += Client_OnTickPriceUpdated;
            client.OnTickOptionComputationUpdated += Client_OnTickOptionComputationUpdated;
            client.OnPositionChanged += Client_OnPositionChanged;

            verticalLine = new VerticalLineAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                ClipToChartArea = chartRiskProfile.ChartAreas[0].Name,
                LineColor = Color.Black,
                LineWidth = 1,
                Visible = false, // Start hidden until hover
                IsInfinitive = true,
            };
            chartRiskProfile.Annotations.Add(verticalLine);

            HorizontalLineAnnotation zeroLine = new HorizontalLineAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                IsInfinitive = true,
                Y = 0,
                LineColor = Color.Black,
                LineWidth = 1,
                LineDashStyle = ChartDashStyle.Solid,
                ClipToChartArea = chartRiskProfile.ChartAreas[0].Name,

            };
            chartRiskProfile.Annotations.Add(zeroLine);

            intersectionDot = new EllipseAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Width = 0.5, // Adjust size as needed
                Height = 1,
                BackColor = Color.Black, // Color for the dot
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(intersectionDot);


            // Initialize and add the underlying price text annotation
            priceAnnotation = new TextAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Alignment = ContentAlignment.TopCenter,
                ForeColor = Color.Black,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(priceAnnotation);

            // Initialize and add the P&L text annotation
            pnlAnnotation = new TextAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Alignment = ContentAlignment.MiddleRight,
                ForeColor = Color.Black,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(pnlAnnotation);

            probabilityAnnotation = new TextAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Alignment = ContentAlignment.BottomCenter,
                ForeColor = Color.Black,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(probabilityAnnotation);

            numSpotPrice.Value = stockLastPrice;
        }

        private void OptionsAnalysisForm_Load(object sender, EventArgs e)
        {
            dtpCurrTimePicker.Value = DateTime.Now;

            dgOptTradeLegs.Columns["colVolatility"].ValueType = typeof(double);
            dgOptTradeLegs.Columns["colVolatility"].DefaultCellStyle.Format = "N2";

            dgOptTradeLegs.Columns["colQuantity"].ValueType = typeof(int);
            dgOptTradeLegs.Columns["colQuantity"].DefaultCellStyle.Format = "N0";

            dgOptTradeLegs.Columns["colStrike"].ValueType = typeof(double);
            dgOptTradeLegs.Columns["colStrike"].DefaultCellStyle.Format = "N2";

            dgOptTradeLegs.Columns["colExpiration"].ValueType = typeof(DateTime);
            dgOptTradeLegs.Columns["colExpiration"].DefaultCellStyle.Format = "dd-MM-yyyy";

            client.GetOptionChain();

        }

        private void Client_OnOptionChainDetailsReceived(int multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            if (this.dgOptTradeLegs.InvokeRequired)
            {
                OnOptionChainDetailsReceivedEndCallback d = new OnOptionChainDetailsReceivedEndCallback(Client_OnOptionChainDetailsReceived);
                try
                {
                    this.Invoke(d, new object[] { multiplier, expirations, strikes });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from Client_OnOptionChainDetailsReceived", e);
                }
            }
            else
            {
                optionChainDetails = new OptionChainDetails
                {
                    Multiplier = multiplier,
                    Expirations = expirations,
                    Strikes = strikes
                };

                for (int i = dgOptTradeLegs.Rows.Count - 1; i >= 0; i--)
                {
                    dgOptTradeLegs.Rows.RemoveAt(i);
                }

                dgOptTradeLegs.Rows.Add();
            }
        }

        private void dgOptTradeLegs_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Loop only through the newly added rows
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                InitOptionLeg(i);
            }

        }

        private void dgOptTradeLegs_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                client.RemoveOptionLeg(i);
            }
        }

        public void InitOptionLeg(int rowIdx)
        {

            if (this.dgOptTradeLegs.InvokeRequired)
            {
                InitOptionLegCallback d = new InitOptionLegCallback(InitOptionLeg);
                try
                {
                    this.Invoke(d, new object[] { rowIdx });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from InitOptionLeg", e);
                }
            }
            else
            {
                if (optionChainDetails.Expirations == null || optionChainDetails.Strikes == null) return;

                // Parse and sort expiration dates
                DateTime now = DateTime.Now;
                List<DateTime> sortedExpirations = optionChainDetails.Expirations
                    .Select(exp => DateTime.ParseExact(exp, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
                    .OrderBy(exp => exp)
                    .ToList();

                // Parse and sort strikes
                List<double> sortedStrikes = optionChainDetails.Strikes.OrderBy(strike => strike).ToList();

                // Determine the closest expiration and strike
                DateTime closestExpiration = sortedExpirations
                    .OrderBy(exp => Math.Abs((exp - now).Ticks))
                    .FirstOrDefault();

                double currentSpotPrice = (double)numSpotPrice.Value;
                double closestStrike = sortedStrikes
                    .OrderBy(strike => Math.Abs(strike - currentSpotPrice))
                    .FirstOrDefault();

                DataGridViewRow row = dgOptTradeLegs.Rows[rowIdx];

                // Ensure the expiration and strike ComboBox items are available for the new rows
                DataGridViewCheckBoxCell selectedCell = row.Cells[0] as DataGridViewCheckBoxCell;
                DataGridViewComboBoxCell rightCell = row.Cells[1] as DataGridViewComboBoxCell;
                DataGridViewComboBoxCell expirationCell = row.Cells[2] as DataGridViewComboBoxCell;
                DataGridViewComboBoxCell strikeCell = row.Cells[3] as DataGridViewComboBoxCell;
                DataGridViewTextBoxCell volatilityCell = row.Cells[4] as DataGridViewTextBoxCell;
                DataGridViewTextBoxCell quantityCell = row.Cells[5] as DataGridViewTextBoxCell;

                if (selectedCell != null || rightCell != null || expirationCell != null && strikeCell != null || volatilityCell != null || quantityCell != null)
                {
                    selectedCell.Value = true;
                    rightCell.Value = "CALL";
                    quantityCell.Value = 1;

                    expirationCell.Items.Clear();
                    strikeCell.Items.Clear();

                    foreach (DateTime expiration in sortedExpirations)
                    {
                        expirationCell.Items.Add(expiration);
                    }

                    foreach (double strike in sortedStrikes)
                    {
                        strikeCell.Items.Add(strike);
                    }

                    // Set the default values for the new row
                    expirationCell.Value = closestExpiration;
                    strikeCell.Value = closestStrike;
                }

                if (rowIdx != optionLegs.Count) throw new Exception();
                optionLegs.Add(new OptionLeg
                {
                    Idx = rowIdx, // Add the row index
                    IsSelected = true,
                    Expiration = closestExpiration,
                    Strike = closestStrike,
                    //Volatility = volatility,
                    Right = "CALL",
                    Quantity = 1
                });

                UpdateOptionLeg(rowIdx);

                // Refresh the DataGridView to apply changes
                dgOptTradeLegs.Refresh();
            }
        }

        private void dgOptTradeLegs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the event is triggered for valid rows and columns
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= optionLegs.Count)
                return;

            // Get the affected row
            DataGridViewRow row = dgOptTradeLegs.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            string colName = cell.OwningColumn.Name;

            switch (colName)
            {
                case "colSelected":
                    optionLegs[e.RowIndex].IsSelected = (bool)(cell as DataGridViewCheckBoxCell).Value;
                    UpdateComboContract();
                    break;
                case "colRight":
                    optionLegs[e.RowIndex].Right = (string)(cell as DataGridViewComboBoxCell).Value;
                    UpdateOptionLeg(e.RowIndex);
                    break;
                case "colExpiration":
                    optionLegs[e.RowIndex].Expiration = (DateTime)(cell as DataGridViewComboBoxCell).Value;
                    UpdateOptionLeg(e.RowIndex);
                    break;
                case "colStrike":
                    optionLegs[e.RowIndex].Strike = (double)(cell as DataGridViewComboBoxCell).Value;
                    UpdateOptionLeg(e.RowIndex);
                    break;
                case "colVolatility":
                    optionLegs[e.RowIndex].Volatility = (double)(cell as DataGridViewTextBoxCell).Value;
                    break;
                case "colQuantity":
                    optionLegs[e.RowIndex].Quantity = (int)(cell as DataGridViewTextBoxCell).Value;
                    UpdateComboContract();
                    break;
                default:
                    break;
            }
        }

        public void UpdateOptionLeg(int legIdx)
        {
            OptionLeg currOptionLeg = optionLegs[legIdx];
            if (currOptionLeg != null)
            {
                client.SetOptionLegContract(
                    legIdx,
                    currOptionLeg.Right,
                    currOptionLeg.Expiration.ToString("yyyyMMdd"),
                    currOptionLeg.Strike
                );
            }

            ResetRowValues(legIdx);
            if (currOptionLeg.IsSelected) { UpdateComboContract(); }
        }

        public void UpdateComboContract()
        {
            List<int> comboContractIdices = new List<int>();
            List<int> comboContractQuantities = new List<int>();
            optionLegs.ForEach(ol =>
            {
                if (ol.IsSelected)
                {
                    comboContractIdices.Add(ol.Idx);
                    comboContractQuantities.Add(ol.Quantity);
                }
            });

            client.SetComboContract(comboContractIdices, comboContractQuantities);
            PlotRiskProfile();
        }

        private void ResetRowValues(int rowIdx, bool resetOnlyMarketData = true)
        {
            if (rowIdx < 0 || rowIdx >= dgOptTradeLegs.Rows.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIdx), "Invalid row index.");
            }

            DataGridViewRow row = dgOptTradeLegs.Rows[rowIdx];

            List<string> contractDefinitionCols = new List<string>
                {
                    "colSelected",
                    "colRight",
                    "colExpiration",
                    "colStrike",
                    "colOpenedIterest",
                    "colQuantity"
                };

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.OwningColumn is DataGridViewButtonColumn) { continue; }

                string colName = cell.OwningColumn.Name;

                if (resetOnlyMarketData && contractDefinitionCols.Contains(colName)) { continue; }
                // If we want to reset all cells in the row including contractDefinitionCols, we need to also 
                // modify the optionLegs list to include the correct contract. Maybe we only need this function to 
                // reset market data cells and not more then this. In that case can change the function (including
                // it's name) to reset only market data cells. This to be desided in the future. 

                // Reset value based on column type
                switch (colName)
                {
                    case "colSelect": // Checkbox column
                        cell.Value = false; // Default to unchecked
                        break;

                    case "colRight": // ComboBox column
                    case "colExpiration":
                    case "colStrike":
                        cell.Value = null; // Default to no selection
                        break;

                    case "colVolatility": // TextBox column
                        cell.Value = (double)numAtmIv.Value;
                        break;
                    case "colQuantity":
                        cell.Value = 1; // Default to empty string
                        break;

                    case "colBid":
                    case "colAsk":
                    case "colLast":
                    case "colOpenedInterest":
                    case "colDelta":
                        cell.Value = 0.00;
                        break;

                    default:
                        cell.Value = null; // Default for other columns
                        break;
                }
            }

            // Refresh the DataGridView to apply changes
            dgOptTradeLegs.Refresh();
        }

        private int GetContractIdx()
        {
            List<OptionLeg> selectedOptions = optionLegs.Where(opl => opl.IsSelected).ToList();

            if (selectedOptions.Count == 0) { return -3; }
            return selectedOptions.Count > 1 ? -2 : optionLegs[0].Idx;
        }


        private void btnAddLeg_Click(object sender, EventArgs e)
        {
            dgOptTradeLegs.Rows.Add();
        }

        private void dgOptTradeLegs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != dgOptTradeLegs.Rows.Count - 1) return; // can't remove option legs that are not the last ones becuase then to start changing indices for all legs afterwards and because of market data registrations that creates a mass.
            // Ensure the click is valid and on the button column
            if (e.RowIndex >= 0 && e.ColumnIndex == dgOptTradeLegs.Columns["colRemoveLeg"].Index)
            {
                dgOptTradeLegs.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void Client_OnTickPriceUpdated(int rowIdx, string fieldName, string Value)
        {
            if (this.dgOptTradeLegs.InvokeRequired)
            {
                SetOptionLegTickPriceCallback d = new SetOptionLegTickPriceCallback(Client_OnTickPriceUpdated);
                try
                {
                    this.Invoke(d, new object[] { rowIdx, fieldName, Value });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from Client_OnTickPriceUpdated", e);
                }
            }
            else
            {
                switch (rowIdx)
                {
                    case -2:
                        switch (fieldName)
                        {
                            case "Bid":
                                tbBid.Text = Value;
                                break;
                            case "Ask":
                                tbAsk.Text = Value;
                                break;
                            case "Last":
                                tbLast.Text = Value;
                                break;
                            default:
                                break;
                        }
                        break;
                    case -1:
                        if (fieldName == "Last")
                            this.numSpotPrice.Value = decimal.Parse(Value);
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(fieldName)) { return; }
                        DataGridViewRow row = dgOptTradeLegs.Rows[rowIdx];
                        string colName = "col" + fieldName;
                        row.Cells[colName].Value = Value;
                        break;
                }

            }
        }

        private void Client_OnTickOptionComputationUpdated(int reqId, double impliedVolatility, double delta)
        {
            if (this.dgOptTradeLegs.InvokeRequired)
            {
                SetOptionLegTickOptionComputationCallback d = new SetOptionLegTickOptionComputationCallback(Client_OnTickOptionComputationUpdated);
                try
                {
                    this.Invoke(d, new object[] { reqId, impliedVolatility, delta });
                }
                catch (Exception e)
                {
                    Console.WriteLine("This is from Client_OnTickPriceUpdated", e);
                }
            }
            else
            {
                if (reqId == -2)
                {
                    throw new NotImplementedException();
                }

                if (reqId < 0 || reqId >= dgOptTradeLegs.Rows.Count) return;

                DataGridViewRow row = dgOptTradeLegs.Rows[reqId];
                DataGridViewCell ivCell = row.Cells[4];
                DataGridViewCell deltaCell = row.Cells[10];

                ivCell.Value = impliedVolatility;
                deltaCell.Value = delta;
            }
        }

        private void Client_OnPositionChanged(int posIdx)
        {
            if (posIdx == -1) return;
            AdjustStopLoss();
        }

        private void btnPlotRiskProfile_Click(object sender, EventArgs e)
        {
            PlotRiskProfile();
        }

        private void PlotRiskProfile()
        {
            strategyInitialCost = 0;

            double s = (double)numSpotPrice.Value;
            double r = (double)numRiskFreeRate.Value;
            double range = (double)numPriceRange.Value;
            DateTime currTime = dtpCurrTimePicker.Value;
            List<double> pnlBeforeExp = new List<double>();
            List<double> pnlAtExp = new List<double>();
            List<double> pricePoints = new List<double>();

            if (s == 0) return;

            double lowerBound = s * (1 - range / 100);
            double upperBound = s * (1 + range / 100);
            double step = s * 0.001;

            List<OptionLeg> selectedOptions = optionLegs.Where(opl => opl.IsSelected).ToList();

            // Calculate the initial cost of the strategy (premium)
            strategyInitialCost = BlackScholes.CalculateStrategyPremium(selectedOptions, s, currTime, r);

            // Calculate PnL for each price point
            for (double underlyingPrice = lowerBound; underlyingPrice <= upperBound; underlyingPrice += step)
            {
                double totalPnlBeforeExp = BlackScholes.CalculateStrategyPremium(selectedOptions, underlyingPrice, currTime, r); ;
                double totalPnlAtExp = BlackScholes.CalculateStrategyPremium(selectedOptions, underlyingPrice, DateTime.MaxValue, r);

                pnlAtExp.Add(totalPnlAtExp - strategyInitialCost);
                pnlBeforeExp.Add(totalPnlBeforeExp - strategyInitialCost);
                pricePoints.Add(underlyingPrice);
            }

            // Plot results on chart
            chartRiskProfile.Series.Clear();

            // PnL Before Expiration Series
            Series seriesPnlBeforeExp = new Series("PnL (Before Expiration)");
            seriesPnlBeforeExp.ChartType = SeriesChartType.Line;
            seriesPnlBeforeExp.XValueType = ChartValueType.Double;
            seriesPnlBeforeExp.YValueType = ChartValueType.Double;

            for (int i = 0; i < pricePoints.Count; i++)
            {
                DataPoint point = new DataPoint(pricePoints[i], pnlBeforeExp[i]);
                point.Color = ColorPnlPoint(pnlBeforeExp[i]);
                seriesPnlBeforeExp.Points.Add(point);
            }
            chartRiskProfile.Series.Add(seriesPnlBeforeExp);

            int dashSpacing = 2;
            // PnL at Expiration Series
            Series seriesPnlAtExp = new Series("PnL (At Expiration)");
            seriesPnlAtExp.ChartType = SeriesChartType.Line;
            seriesPnlAtExp.XValueType = ChartValueType.Double;
            seriesPnlAtExp.YValueType = ChartValueType.Double;

            for (int i = 0; i < pricePoints.Count; i++)
            {
                if (i % dashSpacing == 0)
                {
                    DataPoint point = new DataPoint(pricePoints[i], pnlAtExp[i]);
                    point.Color = ColorPnlPoint(pnlAtExp[i]);
                    seriesPnlAtExp.Points.Add(point);
                }
                else
                {
                    DataPoint point = new DataPoint(pricePoints[i], pnlAtExp[i]);
                    point.Color = Color.Transparent;
                    seriesPnlAtExp.Points.Add(point);
                }
            }
            chartRiskProfile.Series.Add(seriesPnlAtExp);

            // Configure chart display
            chartRiskProfile.ChartAreas[0].AxisX.Title = "Underlying Price";
            chartRiskProfile.ChartAreas[0].AxisY.Title = "Profit / Loss";
            chartRiskProfile.ChartAreas[0].RecalculateAxesScale();
        }

        private Color ColorPnlPoint(double yValue)
        {
            int intensity = (int)Math.Min(255, Math.Abs(yValue) * 2); // Scale to keep within 0-255 range
                                                                      // Assign color based on positive or negative yValue
            return yValue > 0
                ? Color.FromArgb(255, 0, intensity / 2, 0) // Green gradient above 0
                : Color.FromArgb(255, intensity, 0, 0);     // Red gradient below 0
        }

        private void chartRiskProfile_MouseMove(object sender, MouseEventArgs e)
        {
            double r = (double)numRiskFreeRate.Value;
            DateTime currTime = dtpCurrTimePicker.Value;
            double atmIv = (double)numAtmIv.Value;

            // Convert mouse X position to chart coordinate (Underlying price)
            double mouseXValue = chartRiskProfile.ChartAreas[0].AxisX.PixelPositionToValue(e.X);

            // Calculate the P&L at this underlying price for the strategy
            double pnlBeforeExp = BlackScholes.CalculateStrategyPremium(optionLegs, mouseXValue, currTime, r) - strategyInitialCost;

            //// Find the nearest price point in the data
            //double step = (double)numSpotPrice.Value * 0.001; // Assuming 0.1% steps of the spot price
            //double mouseXValue = Math.Round(mouseXValue / step) * step;

            // Set the position of the vertical line
            verticalLine.X = mouseXValue;
            verticalLine.Visible = true;

            intersectionDot.AnchorX = mouseXValue;
            intersectionDot.AnchorY = pnlBeforeExp - 4;
            intersectionDot.Visible = true;

            // Display underlying price text at the top of the line
            priceAnnotation.Text = $"Price:       {mouseXValue:F2}";
            priceAnnotation.AnchorX = mouseXValue;
            priceAnnotation.AnchorY = chartRiskProfile.ChartAreas[0].AxisY.Maximum; // At the top of the chart
            priceAnnotation.Visible = true;


            pnlAnnotation.Text = $"PnL:         {pnlBeforeExp:F2}";
            pnlAnnotation.AnchorX = mouseXValue;
            pnlAnnotation.AnchorY = pnlBeforeExp;
            pnlAnnotation.Visible = true;


            // Calculate the probability of being above/below the current price at expiration
            double timeToExp = optionLegs.Count > 0 ? optionLegs.Min(leg => BlackScholes.CalculateTimeToExpirationInMinutes(leg.Expiration, currTime)) / 525600.0 : 0;
            double probAbove = BlackScholes.CalculateProbabilityAboveSpot((double)numSpotPrice.Value, mouseXValue, timeToExp, r, atmIv);
            double probBelow = 1 - probAbove;

            // Update and position the probability annotation
            probabilityAnnotation.Text = $"<--{probBelow * 100:F2}%     {probAbove * 100:F2}%-->";
            probabilityAnnotation.AnchorX = mouseXValue;
            probabilityAnnotation.AnchorY = chartRiskProfile.ChartAreas[0].AxisY.Minimum; // At the bottom of the chart
            probabilityAnnotation.Visible = true;
        }

        private void chartRiskProfile_MouseLeave(object sender, EventArgs e)
        {
            verticalLine.Visible = false;
            intersectionDot.Visible = false;
            priceAnnotation.Visible = false;
            pnlAnnotation.Visible = false;
            probabilityAnnotation.Visible = false;
        }

        public void placeOrder(string side, Keys modifierKeys)
        {
            int contractIdx = GetContractIdx();

            if (contractIdx == -3) { return; }
            int posSize = contractIdx == -2 ? 1 : optionLegs[0].Quantity;

            double lmtPriceOffset = (double)((side == "BUY") ? this.numTradeOffset.Value : -this.numTradeOffset.Value);

            int stopType = 0;
            bool isOutsideRth = cbIsLimitStop.Checked;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            decimal totalQuantity = Math.Floor(posSize * Convert.ToDecimal(numQuantity.Value));

            double stopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value;


            client.PlaceOrder(contractIdx, side, modifierKeys, totalQuantity, lmtPriceOffset, stopType, isOutsideRth, stopPrice);
        }

        private void btnClosePos_Click(object sender, EventArgs e)
        {
            int contractIdx = GetContractIdx();
            if (contractIdx == -3) { return; }

            client.ClosePositionForContract(contractIdx, (double)this.numTradeOffset.Value, cbIsLimitStop.Checked);
        }

        private void btnCancelLast_Click(object sender, EventArgs e)
        {
            int contractIdx = GetContractIdx();
            if (contractIdx == -3) { return; }
            client.CancelLastOrderForContract(contractIdx);
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            int contractIdx = GetContractIdx();
            if (contractIdx == -3) { return; }
            Keys modifierKeys = Form.ModifierKeys;
            client.CancelAllOrdersForContract(contractIdx, modifierKeys == Keys.Control);
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

        private void btnStopLossAdj_Click(object sender, EventArgs e)
        {
            AdjustStopLoss();
        }

        private void AdjustStopLoss()
        {
            int contractIdx = GetContractIdx();

            if (contractIdx == -3) { return; }
            bool isOutsideRth = cbIsLimitStop.Checked;
            int stopType = 0;

            if (this.cbStopLoss.Checked) stopType = 1;
            if (this.cbTrailStop.Checked) stopType = 2;

            double stopPrice = stopType == 1 ? (double)numStopLoss.Value : (double)numTrailStop.Value;

            client.AdjustStopLoss(contractIdx, isOutsideRth, stopType, stopPrice, (double)numTradeOffset.Value);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            string side = "SELL";
            Keys modifierKeys = Form.ModifierKeys;
            placeOrder(side, modifierKeys);
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            string side = "BUY";
            Keys modifierKeys = Form.ModifierKeys;
            placeOrder(side, modifierKeys);
        }

        //private void dgOptTradeLegs_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        //{

        //}

        // Event: When a cell enters edit mode (for immediate updates)
        //private void dgOptTradeLegs_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        //{
        //    string columnName = dgOptTradeLegs.CurrentCell?.OwningColumn?.Name;

        //    // Handle ComboBox columns
        //    if ((columnName == "colRight" || columnName == "colExpiration" || columnName == "colStrike") && e.Control is ComboBox comboBox)
        //    {
        //        // Remove existing handler to prevent multiple subscriptions
        //        comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
        //        comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        //    }

        //    // Handle TextBox columns for Volatility or Quantity
        //    if ((columnName == "colVolatility" || columnName == "colQuantity") && e.Control is TextBox textBox)
        //    {
        //        // Remove existing handler to prevent multiple subscriptions
        //        textBox.TextChanged -= TextBox_TextChanged;
        //        textBox.TextChanged += TextBox_TextChanged;
        //    }

        //    // Handle CheckBox column for Select
        //    if (columnName == "colSelect" && e.Control is CheckBox checkBox)
        //    {
        //        // Remove existing handler to prevent multiple subscriptions
        //        checkBox.CheckedChanged -= CheckBox_CheckedChanged;
        //        checkBox.CheckedChanged += CheckBox_CheckedChanged;
        //    }
        //}

        //// Event: For ComboBox value changes
        //private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var cell = dgOptTradeLegs.CurrentCell;
        //    if (cell != null)
        //    {
        //        string columnName = cell.OwningColumn.Name;
        //        if (columnName == "colRight" || columnName == "colExpiration" || columnName == "colStrike")
        //        {
        //            OnLegContractChanged(cell.RowIndex);
        //        }
        //    }
        //}

        //// Event: For TextBox value changes
        //private void TextBox_TextChanged(object sender, EventArgs e)
        //{
        //    var cell = dgOptTradeLegs.CurrentCell;
        //    if (cell != null)
        //    {
        //        string columnName = cell.OwningColumn.Name;
        //        if (columnName == "colVolatility")
        //        {
        //            OnLegIvAdjusted(cell.RowIndex, ((TextBox)sender).Text);
        //        }
        //        else if (columnName == "colQuantity")
        //        {
        //            OnLegQuantityAdjusted(cell.RowIndex, ((TextBox)sender).Text);
        //        }
        //    }
        //}

        //// Event: For CheckBox state changes
        //private void CheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    var cell = dgOptTradeLegs.CurrentCell;
        //    if (cell != null && cell.OwningColumn.Name == "colSelect")
        //    {
        //        bool isSelected = ((CheckBox)sender).Checked;
        //        OnSelectLeg(cell.RowIndex, isSelected);
        //    }
        //}

        //// Functions to be triggered
        //private void OnSelectLeg(int rowIndex, bool isSelected)
        //{
        //    Console.WriteLine($"Row {rowIndex} selected state: {isSelected}");
        //    // Add your logic here
        //}

        //private void OnLegContractChanged(int rowIndex)
        //{
        //    Console.WriteLine($"Leg contract changed for row {rowIndex}");
        //    // Add your logic here
        //}

        //private void OnLegIvAdjusted(int rowIndex, string newIv)
        //{
        //    Console.WriteLine($"IV adjusted for row {rowIndex}: {newIv}");
        //    // Add your logic here
        //}

        //private void OnLegQuantityAdjusted(int rowIndex, string newQuantity)
        //{
        //    Console.WriteLine($"Quantity adjusted for row {rowIndex}: {newQuantity}");
        //    // Add your logic here
        //}
    }

    public class OptionLeg
    {
        public int Idx { get; set; }
        public bool IsSelected { get; set; }
        public string Right { get; set; }
        public DateTime Expiration { get; set; }
        public double Strike { get; set; }
        public double Volatility { get; set; } // as a decimal (e.g., 0.20 for 20%)
        public int Quantity { get; set; }
    }

    public class OptionChainDetails
    {
        public int Multiplier { get; set; }
        public HashSet<string> Expirations { get; set; }
        public HashSet<double> Strikes { get; set; }
    }

    public class BlackScholes
    {
        private static double Erf(double x)
        {
            // Constants used in approximation of erf function
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = (x < 0) ? -1 : 1;
            x = Math.Abs(x);

            // A&S formula 7.1.26 approximation
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

        // Cumulative distribution function for the standard normal distribution
        private static double Cdf(double x)
        {
            return (1.0 + Erf(x / Math.Sqrt(2.0))) / 2.0;
        }

        // Black-Scholes formula
        public static double CalculateOptionPrice(double S, double K, double T, double r, double sigma, string right)
        {
            double d1 = (Math.Log(S / K) + (r + 0.5 * sigma * sigma) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            if (right == "CALL")
            {
                return S * Cdf(d1) - K * Math.Exp(-r * T) * Cdf(d2);
            }
            else
            {
                return K * Math.Exp(-r * T) * Cdf(-d2) - S * Cdf(-d1);
            }
        }

        public static double CalculateIntrinsicValue(double s, double k, string right)
        {
            if (right == "CALL")
            {
                return Math.Max(s - k, 0); // Call option intrinsic value
            }
            else
            {
                return Math.Max(k - s, 0); // Put option intrinsic value
            }
        }

        public static double CalculateTimeToExpirationInMinutes(DateTime expirationDate, DateTime currentTime)
        {
            // Define the Eastern Time zone
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            // Set the expiration time to 4:00 PM Eastern Time
            DateTime expirationEndOfDayET = new DateTime(
                expirationDate.Year, expirationDate.Month, expirationDate.Day, 16, 0, 0, DateTimeKind.Unspecified);

            // Convert expiration to ET time zone first, then to UTC
            DateTime expirationEndOfDayEastern = TimeZoneInfo.ConvertTimeToUtc(
                TimeZoneInfo.ConvertTimeToUtc(expirationEndOfDayET, easternZone));

            // Convert the provided current time to UTC if it’s not already
            DateTime currentTimeUtc = currentTime.Kind == DateTimeKind.Utc ? currentTime : currentTime.ToUniversalTime();

            // Calculate the difference in minutes
            double timeToExpMinutes = (expirationEndOfDayEastern - currentTimeUtc).TotalMinutes;

            // Ensure time is positive; if negative, the expiration has passed
            return timeToExpMinutes > 0 ? timeToExpMinutes : 0;
        }

        // Modify the CalculateStrategyPremium function
        public static double CalculateStrategyPremium(List<OptionLeg> optionLegs, double underlyingPrice, DateTime currTime, double riskFreeRate)
        {
            double premium = 0.0;

            foreach (var leg in optionLegs)
            {
                // Calculate time to expiration in years
                double timeToExp = CalculateTimeToExpirationInMinutes(leg.Expiration, currTime) / 525600.0;

                // Determine the price of the option based on whether it's before expiration or at expiration
                double optionPrice = timeToExp > 0
                    ? CalculateOptionPrice(underlyingPrice, leg.Strike, timeToExp, riskFreeRate, leg.Volatility, leg.Right) * 100
                    : CalculateIntrinsicValue(underlyingPrice, leg.Strike, leg.Right) * 100;

                // Adjust premium based on whether the leg is a buy or sell
                premium += leg.Quantity * optionPrice;
            }

            return premium;
        }

        public static double CalculateProbabilityAboveSpot(double currentSpotPrice, double targetSpotPrice, double timeToExpirationInYears, double riskFreeRate, double atTheMonyImpliedVolatility)
        {
            // Calculate d2, which is the standard normal variable for the target spot price at expiration
            double d2 = (Math.Log(targetSpotPrice / currentSpotPrice) + (riskFreeRate - 0.5 * atTheMonyImpliedVolatility * atTheMonyImpliedVolatility) * timeToExpirationInYears) /
                        (atTheMonyImpliedVolatility * Math.Sqrt(timeToExpirationInYears));

            // Use the CDF to get the probability that the underlying price will be above the target spot price
            return 1 - Cdf(d2); // Probability of being above S1 is 1 - CDF(d2)
        }

    }
}

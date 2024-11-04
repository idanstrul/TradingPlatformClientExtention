﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IB_TradingPlatformExtention1
{
    public partial class OptionsAnalysisForm : Form
    {
        private VerticalLineAnnotation verticalLine;
        private TextAnnotation priceAnnotation;
        private TextAnnotation pnlAnnotation;

        public List<OptionLeg> optionLegs = new List<OptionLeg>();
        public double strategyInitialCost = 0;


        public OptionsAnalysisForm()
        {
            InitializeComponent();

            verticalLine = new VerticalLineAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AllowMoving = false,
                ClipToChartArea = chartRiskProfile.ChartAreas[0].Name,
                LineColor = Color.Red,
                LineWidth = 1,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(verticalLine);

            // Initialize and add the underlying price text annotation
            priceAnnotation = new TextAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Alignment = ContentAlignment.TopCenter,
                ForeColor = Color.Blue,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(priceAnnotation);

            // Initialize and add the P&L text annotation
            pnlAnnotation = new TextAnnotation
            {
                AxisX = chartRiskProfile.ChartAreas[0].AxisX,
                AxisY = chartRiskProfile.ChartAreas[0].AxisY,
                Alignment = ContentAlignment.MiddleRight,
                ForeColor = Color.Green,
                Visible = false // Start hidden until hover
            };
            chartRiskProfile.Annotations.Add(pnlAnnotation);
        }

        private void btnPlotRiskProfile_Click(object sender, EventArgs e)
        {
            optionLegs.Clear();
            strategyInitialCost = 0;

            double s = (double)numSpotPrice.Value;
            double r = (double)numRiskFreeRate.Value;
            double range = (double)numPriceRange.Value;
            DateTime currTime = dtpCurrTimePicker.Value;
            List<double> pnlBeforeExp = new List<double>();
            List<double> pnlAtExp = new List<double>();
            List<double> pricePoints = new List<double>();

            double lowerBound = s * (1 - range / 100);
            double upperBound = s * (1 + range / 100);
            double step = s * 0.001;

            foreach (DataGridViewRow row in dgOptTradeLegs.Rows)
            {
                if (row.IsNewRow) continue;

                if (!DateTime.TryParse(row.Cells[0].Value?.ToString(), out DateTime expiration))
                    throw new ArgumentException("Invalid expiration date.");
                if (!double.TryParse(row.Cells[1].Value?.ToString(), out double strike))
                    throw new ArgumentException("Invalid strike price.");
                if (!double.TryParse(row.Cells[2].Value?.ToString(), out double volatility))
                    throw new ArgumentException("Invalid volatility.");

                bool isCall = (bool)(row.Cells[3].Value ?? false);
                bool isBuy = (bool)(row.Cells[4].Value ?? false);

                // Add this option leg to the list
                optionLegs.Add(new OptionLeg
                {
                    Expiration = expiration,
                    Strike = strike,
                    Volatility = volatility,
                    IsCall = isCall,
                    IsBuy = isBuy
                });
            }

            // Calculate the initial cost of the strategy (premium)
            strategyInitialCost = BlackScholes.CalculateStrategyPremium(optionLegs, s, currTime, r);

            // Calculate PnL for each price point
            for (double underlyingPrice = lowerBound; underlyingPrice <= upperBound; underlyingPrice += step)
            {
                double totalPnlBeforeExp = BlackScholes.CalculateStrategyPremium(optionLegs, underlyingPrice, currTime, r); ;
                double totalPnlAtExp = BlackScholes.CalculateStrategyPremium(optionLegs, underlyingPrice, DateTime.MaxValue, r);

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
                seriesPnlBeforeExp.Points.AddXY(pricePoints[i], pnlBeforeExp[i]);
            }
            chartRiskProfile.Series.Add(seriesPnlBeforeExp);

            // PnL at Expiration Series
            Series seriesPnlAtExp = new Series("PnL (At Expiration)");
            seriesPnlAtExp.ChartType = SeriesChartType.Line;
            seriesPnlAtExp.XValueType = ChartValueType.Double;
            seriesPnlAtExp.YValueType = ChartValueType.Double;

            for (int i = 0; i < pricePoints.Count; i++)
            {
                seriesPnlAtExp.Points.AddXY(pricePoints[i], pnlAtExp[i]);
            }
            chartRiskProfile.Series.Add(seriesPnlAtExp);

            // Configure chart display
            chartRiskProfile.ChartAreas[0].AxisX.Title = "Underlying Price";
            chartRiskProfile.ChartAreas[0].AxisY.Title = "Profit / Loss";
            chartRiskProfile.ChartAreas[0].RecalculateAxesScale();
        }

        private void chartRiskProfile_MouseMove(object sender, MouseEventArgs e)
        {
            double r = (double)numRiskFreeRate.Value;
            DateTime currTime = dtpCurrTimePicker.Value;

            // Convert mouse X position to chart coordinate (Underlying price)
            double mouseXValue = chartRiskProfile.ChartAreas[0].AxisX.PixelPositionToValue(e.X);

            //// Find the nearest price point in the data
            //double step = (double)numSpotPrice.Value * 0.001; // Assuming 0.1% steps of the spot price
            //double mouseXValue = Math.Round(mouseXValue / step) * step;

            // Set the position of the vertical line
            verticalLine.X = mouseXValue;
            verticalLine.Visible = true;

            // Display underlying price text at the top of the line
            priceAnnotation.Text = $"Price: {mouseXValue:F2}";
            priceAnnotation.AnchorX = mouseXValue;
            priceAnnotation.AnchorY = chartRiskProfile.ChartAreas[0].AxisY.Maximum; // At the top of the chart
            priceAnnotation.Visible = true;

            // Calculate the P&L at this underlying price for the strategy
            double pnlBeforeExp = BlackScholes.CalculateStrategyPremium(optionLegs, mouseXValue, currTime, r);
            pnlAnnotation.Text = $"PnL: {pnlBeforeExp:F2}";
            pnlAnnotation.AnchorX = mouseXValue;
            pnlAnnotation.AnchorY = pnlBeforeExp;
            pnlAnnotation.Visible = true;
        }
    }

    public class OptionLeg
    {
        public DateTime Expiration { get; set; }
        public double Strike { get; set; }
        public double Volatility { get; set; } // as a decimal (e.g., 0.20 for 20%)
        public bool IsCall { get; set; }
        public bool IsBuy { get; set; }
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
        public static double CalculateOptionPrice(double S, double K, double T, double r, double sigma, bool isCall)
        {
            double d1 = (Math.Log(S / K) + (r + 0.5 * sigma * sigma) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            if (isCall)
            {
                return S * Cdf(d1) - K * Math.Exp(-r * T) * Cdf(d2);
            }
            else
            {
                return K * Math.Exp(-r * T) * Cdf(-d2) - S * Cdf(-d1);
            }
        }

        public static double CalculateIntrinsicValue(double s, double k, bool isCall)
        {
            if (isCall)
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
                    ? CalculateOptionPrice(underlyingPrice, leg.Strike, timeToExp, riskFreeRate, leg.Volatility, leg.IsCall) * 100
                    : CalculateIntrinsicValue(underlyingPrice, leg.Strike, leg.IsCall) * 100;

                // Adjust premium based on whether the leg is a buy or sell
                premium += leg.IsBuy ? optionPrice : -optionPrice;
            }

            return premium;
        }

    }
}

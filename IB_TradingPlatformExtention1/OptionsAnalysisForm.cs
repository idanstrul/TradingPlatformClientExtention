using System;
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
        public OptionsAnalysisForm()
        {
            InitializeComponent();
        }

        private void btnPlotRiskProfile_Click(object sender, EventArgs e)
        {
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

            // Calculate initial cost for the strategy
            double initialCost = 0;

            foreach (DataGridViewRow row in dgOptTradeLegs.Rows)
            {
                if (row.IsNewRow) continue;

                // Parse each option leg
                if (!DateTime.TryParse(row.Cells[0].Value?.ToString(), out DateTime expiration))
                    throw new ArgumentException("Invalid expiration date.");
                if (!double.TryParse(row.Cells[1].Value?.ToString(), out double strike))
                    throw new ArgumentException("Invalid strike price.");
                if (!double.TryParse(row.Cells[2].Value?.ToString(), out double volatility))
                    throw new ArgumentException("Invalid volatility.");

                bool isCall = (bool)(row.Cells[3].Value ?? false);
                bool isBuy = (bool)(row.Cells[4].Value ?? false);

                double minutesToExpiration = BlackScholes.CalculateTimeToExpirationInMinutes(expiration, currTime);
                double timeToExp = minutesToExpiration / 525600.0;

                // Calculate option price for the current leg
                double legPrice = BlackScholes.CalculateOptionPrice(s, strike, timeToExp, r, volatility, isCall) * 100;
                initialCost += isBuy ? legPrice : -legPrice;
            }

            // Calculate PnL for each price point
            for (double underlyingPrice = lowerBound; underlyingPrice <= upperBound; underlyingPrice += step)
            {
                double totalPnlBeforeExp = 0;
                double totalPnlAtExp = 0;

                foreach (DataGridViewRow row in dgOptTradeLegs.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Parse each option leg
                    if (!DateTime.TryParse(row.Cells[0].Value?.ToString(), out DateTime expiration))
                        throw new ArgumentException("Invalid expiration date.");
                    if (!double.TryParse(row.Cells[1].Value?.ToString(), out double strike))
                        throw new ArgumentException("Invalid strike price.");
                    if (!double.TryParse(row.Cells[2].Value?.ToString(), out double volatility))
                        throw new ArgumentException("Invalid volatility.");

                    bool isCall = (bool)(row.Cells[3].Value ?? false);
                    bool isBuy = (bool)(row.Cells[4].Value ?? false);

                    double minutesToExpiration = BlackScholes.CalculateTimeToExpirationInMinutes(expiration, currTime);
                    double timeToExp = minutesToExpiration / 525600.0;

                    // Calculate the PnL for each leg
                    double legPriceAtExp = BlackScholes.CalculateIntrinsicValue(underlyingPrice, strike, isCall) * 100;
                    double legPriceBeforeExp = BlackScholes.CalculateOptionPrice(underlyingPrice, strike, timeToExp, r, volatility, isCall) * 100;

                    // Adjust total PnL based on whether it's a long or short position
                    totalPnlAtExp += isBuy ? legPriceAtExp : -legPriceAtExp;
                    totalPnlBeforeExp += isBuy ? legPriceBeforeExp : -legPriceBeforeExp;
                }

                pnlAtExp.Add(totalPnlAtExp - initialCost);
                pnlBeforeExp.Add(totalPnlBeforeExp - initialCost);
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

    }
}

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

        private void btnCalculateOptPrice_Click(object sender, EventArgs e)
        {
            double s = (double)numSpotPrice.Value;
            double k = (double)numStrikePrice.Value;
            double t = (double)numTimeToExpiration.Value;
            double r = (double)numRiskFreeRate.Value;
            double v = (double)numVolatility.Value;
            bool isCall = cbIsCall.Checked;

            double currOptPrice = BlackScholes.CalculateOptionPrice(s, k, t, r, v, isCall);
            numCurrOptPrice.Value = (decimal)currOptPrice;
        }

        private void btnPlotRiskProfile_Click(object sender, EventArgs e)
        {
            double s = (double)numSpotPrice.Value;
            double k = (double)numStrikePrice.Value;
            double t = (double)numTimeToExpiration.Value;
            double r = (double)numRiskFreeRate.Value;
            double v = (double)numVolatility.Value;
            bool isCall = cbIsCall.Checked;

            double range = (double)numPriceRange.Value;
            List<double> optPrices = new List<double>();

            // Define the lower and upper bounds for the underlying price
            double lowerBound = s * (1 - range / 100);
            double upperBound = s * (1 + range / 100);
            double step = s * 0.001; // Step of 0.1% of the spot price

            List<double> pricePoints = new List<double>();

            // Iterate over prices in the defined range and calculate the option price
            for (double underlyingPrice = lowerBound; underlyingPrice <= upperBound; underlyingPrice += step)
            {
                double optionPrice = BlackScholes.CalculateOptionPrice(underlyingPrice, k, t, r, v, isCall);
                optPrices.Add(optionPrice);
                pricePoints.Add(underlyingPrice);
            }

            // Clear previous series and data points
            chartRiskProfile.Series.Clear();

            // Create a new series for the risk profile
            Series series = new Series("Option Price Profile");
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Double;
            series.YValueType = ChartValueType.Double;

            // Add data points to the series
            for (int i = 0; i < pricePoints.Count; i++)
            {
                series.Points.AddXY(pricePoints[i], optPrices[i]);
            }

            // Add the series to the chart
            chartRiskProfile.Series.Add(series);

            // Configure chart display
            chartRiskProfile.ChartAreas[0].AxisX.Title = "Underlying Price";
            chartRiskProfile.ChartAreas[0].AxisY.Title = "Option Price";
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
    }
}

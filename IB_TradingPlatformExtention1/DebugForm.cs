using IBApi;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IB_TradingPlatformExtention1
{
    public partial class DebugForm : Form
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IBApiClient client;

        public DebugForm(IBApiClient _client)
        {
            InitializeComponent();
            client = _client;

            // Ensure the logs folder exists
            string logsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logsFolderPath))
            {
                Directory.CreateDirectory(logsFolderPath);
            }

            // Load NLog configuration
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config");
            var config = new XmlLoggingConfiguration(configFilePath);
            LogManager.Configuration = config;

            // Set the log file name with a timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string logFileName = Path.Combine("logs", $"log_{timestamp}.txt");
            LogManager.Configuration.Variables["logFileName"] = logFileName;
            LogManager.ReconfigExistingLoggers();

            //client.OnTickPriceUpdated += Client_OnTickPriceUpdated;
            //client.OnConnected += Client_OnConnected;
            //client.OnDisconnected += Client_OnDisconnected;
            client.OnPositionChanged += UpdateDetails;
            //client.OnContractSelected += Client_OnContractSelected;
            //client.OnDelayedMarketData += Client_OnDelayedMarketData;
            client.OnOrdersUpdated += UpdateDetails;
            client.OnLogError += Client_OnLogError;
            client.OnFunctionRan += Client_OnFunctionRan;

            UpdateDetails();
        }

        private void Client_OnFunctionRan(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(Client_OnFunctionRan), new object[] { message });
                return;
            }
            tbFunctionLog.AppendText(message + Environment.NewLine);
            tbFunctionLog.SelectionStart = tbFunctionLog.Text.Length;
            tbFunctionLog.ScrollToCaret();

            logger.Info(message);

        }

        private void UpdateDetails()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateDetails), new object[] { });
                return;
            }

            tbPosition.Text = client.GetContractName() + ": " + client.GetPositionDetails();
            tbStopLoss.Text = client.GetStopLossDetails();
            tbTakeProfit.Text = client.GetTakeProfitDetails();
            tbOpenOrders.Text = client.GetOpenOrdersDetails();
        }

        private void Client_OnLogError(string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(Client_OnLogError), new object[] { errorMessage });
                return;
            }
            tbErrorList.AppendText(errorMessage + Environment.NewLine);
            tbErrorList.SelectionStart = tbErrorList.Text.Length;
            tbErrorList.ScrollToCaret();

            UpdateDetails();
        }
    }
}

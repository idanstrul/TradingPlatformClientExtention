namespace IB_TradingPlatformExtention1
{
    partial class OptionsAnalysisForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.numSpotPrice = new System.Windows.Forms.NumericUpDown();
            this.numStrikePrice = new System.Windows.Forms.NumericUpDown();
            this.numTimeToExpiration = new System.Windows.Forms.NumericUpDown();
            this.numRiskFreeRate = new System.Windows.Forms.NumericUpDown();
            this.numVolatility = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numCurrOptPrice = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCalculateOptPrice = new System.Windows.Forms.Button();
            this.cbIsCall = new System.Windows.Forms.CheckBox();
            this.btnPlotRiskProfile = new System.Windows.Forms.Button();
            this.numPriceRange = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chartRiskProfile = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStrikePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeToExpiration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVolatility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrOptPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // numSpotPrice
            // 
            this.numSpotPrice.DecimalPlaces = 2;
            this.numSpotPrice.Location = new System.Drawing.Point(131, 17);
            this.numSpotPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSpotPrice.Name = "numSpotPrice";
            this.numSpotPrice.Size = new System.Drawing.Size(120, 22);
            this.numSpotPrice.TabIndex = 0;
            // 
            // numStrikePrice
            // 
            this.numStrikePrice.DecimalPlaces = 2;
            this.numStrikePrice.Location = new System.Drawing.Point(131, 45);
            this.numStrikePrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numStrikePrice.Name = "numStrikePrice";
            this.numStrikePrice.Size = new System.Drawing.Size(120, 22);
            this.numStrikePrice.TabIndex = 1;
            // 
            // numTimeToExpiration
            // 
            this.numTimeToExpiration.DecimalPlaces = 6;
            this.numTimeToExpiration.Location = new System.Drawing.Point(131, 73);
            this.numTimeToExpiration.Name = "numTimeToExpiration";
            this.numTimeToExpiration.Size = new System.Drawing.Size(120, 22);
            this.numTimeToExpiration.TabIndex = 2;
            // 
            // numRiskFreeRate
            // 
            this.numRiskFreeRate.DecimalPlaces = 6;
            this.numRiskFreeRate.Location = new System.Drawing.Point(131, 101);
            this.numRiskFreeRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numRiskFreeRate.Name = "numRiskFreeRate";
            this.numRiskFreeRate.Size = new System.Drawing.Size(120, 22);
            this.numRiskFreeRate.TabIndex = 3;
            // 
            // numVolatility
            // 
            this.numVolatility.DecimalPlaces = 6;
            this.numVolatility.Location = new System.Drawing.Point(131, 129);
            this.numVolatility.Name = "numVolatility";
            this.numVolatility.Size = new System.Drawing.Size(120, 22);
            this.numVolatility.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Spot price";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Strike price";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Time to expiration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Risk free rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Volatility";
            // 
            // numCurrOptPrice
            // 
            this.numCurrOptPrice.DecimalPlaces = 2;
            this.numCurrOptPrice.Location = new System.Drawing.Point(449, 39);
            this.numCurrOptPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numCurrOptPrice.Name = "numCurrOptPrice";
            this.numCurrOptPrice.Size = new System.Drawing.Size(120, 22);
            this.numCurrOptPrice.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(321, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "Current option price";
            // 
            // btnCalculateOptPrice
            // 
            this.btnCalculateOptPrice.Location = new System.Drawing.Point(323, 92);
            this.btnCalculateOptPrice.Name = "btnCalculateOptPrice";
            this.btnCalculateOptPrice.Size = new System.Drawing.Size(75, 23);
            this.btnCalculateOptPrice.TabIndex = 14;
            this.btnCalculateOptPrice.Text = "Calculate";
            this.btnCalculateOptPrice.UseVisualStyleBackColor = true;
            this.btnCalculateOptPrice.Click += new System.EventHandler(this.btnCalculateOptPrice_Click);
            // 
            // cbIsCall
            // 
            this.cbIsCall.AutoSize = true;
            this.cbIsCall.Location = new System.Drawing.Point(131, 157);
            this.cbIsCall.Name = "cbIsCall";
            this.cbIsCall.Size = new System.Drawing.Size(65, 20);
            this.cbIsCall.TabIndex = 15;
            this.cbIsCall.Text = "Is Call";
            this.cbIsCall.UseVisualStyleBackColor = true;
            // 
            // btnPlotRiskProfile
            // 
            this.btnPlotRiskProfile.Location = new System.Drawing.Point(325, 132);
            this.btnPlotRiskProfile.Name = "btnPlotRiskProfile";
            this.btnPlotRiskProfile.Size = new System.Drawing.Size(75, 23);
            this.btnPlotRiskProfile.TabIndex = 0;
            this.btnPlotRiskProfile.Text = "Plot risk profile";
            this.btnPlotRiskProfile.UseVisualStyleBackColor = true;
            this.btnPlotRiskProfile.Click += new System.EventHandler(this.btnPlotRiskProfile_Click);
            // 
            // numPriceRange
            // 
            this.numPriceRange.DecimalPlaces = 4;
            this.numPriceRange.Location = new System.Drawing.Point(494, 133);
            this.numPriceRange.Name = "numPriceRange";
            this.numPriceRange.Size = new System.Drawing.Size(120, 22);
            this.numPriceRange.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(440, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Range";
            // 
            // chartRiskProfile
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRiskProfile.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRiskProfile.Legends.Add(legend1);
            this.chartRiskProfile.Location = new System.Drawing.Point(122, 229);
            this.chartRiskProfile.Name = "chartRiskProfile";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRiskProfile.Series.Add(series1);
            this.chartRiskProfile.Size = new System.Drawing.Size(541, 268);
            this.chartRiskProfile.TabIndex = 18;
            this.chartRiskProfile.Text = "chart1";
            // 
            // OptionsAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 575);
            this.Controls.Add(this.chartRiskProfile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numPriceRange);
            this.Controls.Add(this.btnPlotRiskProfile);
            this.Controls.Add(this.cbIsCall);
            this.Controls.Add(this.btnCalculateOptPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numCurrOptPrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numVolatility);
            this.Controls.Add(this.numRiskFreeRate);
            this.Controls.Add(this.numTimeToExpiration);
            this.Controls.Add(this.numStrikePrice);
            this.Controls.Add(this.numSpotPrice);
            this.Name = "OptionsAnalysisForm";
            this.Text = "OptionsAnalysisForm";
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStrikePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeToExpiration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVolatility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrOptPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numSpotPrice;
        private System.Windows.Forms.NumericUpDown numStrikePrice;
        private System.Windows.Forms.NumericUpDown numTimeToExpiration;
        private System.Windows.Forms.NumericUpDown numRiskFreeRate;
        private System.Windows.Forms.NumericUpDown numVolatility;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numCurrOptPrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCalculateOptPrice;
        private System.Windows.Forms.CheckBox cbIsCall;
        private System.Windows.Forms.Button btnPlotRiskProfile;
        private System.Windows.Forms.NumericUpDown numPriceRange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRiskProfile;
    }
}
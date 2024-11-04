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
            this.numRiskFreeRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPlotRiskProfile = new System.Windows.Forms.Button();
            this.numPriceRange = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chartRiskProfile = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgOptTradeLegs = new System.Windows.Forms.DataGridView();
            this.coExpiration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStrike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsCall = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDirIsBuy = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dtpCurrTimePicker = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptTradeLegs)).BeginInit();
            this.SuspendLayout();
            // 
            // numSpotPrice
            // 
            this.numSpotPrice.DecimalPlaces = 2;
            this.numSpotPrice.Location = new System.Drawing.Point(103, 19);
            this.numSpotPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSpotPrice.Name = "numSpotPrice";
            this.numSpotPrice.Size = new System.Drawing.Size(120, 22);
            this.numSpotPrice.TabIndex = 0;
            // 
            // numRiskFreeRate
            // 
            this.numRiskFreeRate.DecimalPlaces = 6;
            this.numRiskFreeRate.Location = new System.Drawing.Point(103, 47);
            this.numRiskFreeRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numRiskFreeRate.Name = "numRiskFreeRate";
            this.numRiskFreeRate.Size = new System.Drawing.Size(120, 22);
            this.numRiskFreeRate.TabIndex = 3;
            this.numRiskFreeRate.Value = new decimal(new int[] {
            437,
            0,
            0,
            262144});
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Risk free rate";
            // 
            // btnPlotRiskProfile
            // 
            this.btnPlotRiskProfile.Location = new System.Drawing.Point(15, 146);
            this.btnPlotRiskProfile.Name = "btnPlotRiskProfile";
            this.btnPlotRiskProfile.Size = new System.Drawing.Size(149, 23);
            this.btnPlotRiskProfile.TabIndex = 0;
            this.btnPlotRiskProfile.Text = "Plot risk profile";
            this.btnPlotRiskProfile.UseVisualStyleBackColor = true;
            this.btnPlotRiskProfile.Click += new System.EventHandler(this.btnPlotRiskProfile_Click);
            // 
            // numPriceRange
            // 
            this.numPriceRange.DecimalPlaces = 4;
            this.numPriceRange.Location = new System.Drawing.Point(103, 75);
            this.numPriceRange.Name = "numPriceRange";
            this.numPriceRange.Size = new System.Drawing.Size(120, 22);
            this.numPriceRange.TabIndex = 16;
            this.numPriceRange.Value = new decimal(new int[] {
            28,
            0,
            0,
            65536});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Range";
            // 
            // chartRiskProfile
            // 
            chartArea1.AxisX.IntervalOffset = 1D;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Cross;
            chartArea1.Name = "ChartArea1";
            this.chartRiskProfile.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRiskProfile.Legends.Add(legend1);
            this.chartRiskProfile.Location = new System.Drawing.Point(15, 175);
            this.chartRiskProfile.Name = "chartRiskProfile";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRiskProfile.Series.Add(series1);
            this.chartRiskProfile.Size = new System.Drawing.Size(1079, 388);
            this.chartRiskProfile.TabIndex = 18;
            this.chartRiskProfile.Text = "Risk profile";
            this.chartRiskProfile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartRiskProfile_MouseMove);
            // 
            // dgOptTradeLegs
            // 
            this.dgOptTradeLegs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOptTradeLegs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.coExpiration,
            this.colStrike,
            this.colVolatility,
            this.colIsCall,
            this.colDirIsBuy});
            this.dgOptTradeLegs.Location = new System.Drawing.Point(251, 19);
            this.dgOptTradeLegs.Name = "dgOptTradeLegs";
            this.dgOptTradeLegs.RowHeadersWidth = 51;
            this.dgOptTradeLegs.RowTemplate.Height = 24;
            this.dgOptTradeLegs.Size = new System.Drawing.Size(776, 150);
            this.dgOptTradeLegs.TabIndex = 19;
            // 
            // coExpiration
            // 
            this.coExpiration.HeaderText = "Expiration";
            this.coExpiration.MinimumWidth = 6;
            this.coExpiration.Name = "coExpiration";
            this.coExpiration.Width = 125;
            // 
            // colStrike
            // 
            this.colStrike.HeaderText = "Strike";
            this.colStrike.MinimumWidth = 6;
            this.colStrike.Name = "colStrike";
            this.colStrike.Width = 125;
            // 
            // colVolatility
            // 
            this.colVolatility.HeaderText = "Volatility";
            this.colVolatility.MinimumWidth = 6;
            this.colVolatility.Name = "colVolatility";
            this.colVolatility.Width = 125;
            // 
            // colIsCall
            // 
            this.colIsCall.HeaderText = "Is call";
            this.colIsCall.MinimumWidth = 6;
            this.colIsCall.Name = "colIsCall";
            this.colIsCall.Width = 125;
            // 
            // colDirIsBuy
            // 
            this.colDirIsBuy.HeaderText = "Direction is buy";
            this.colDirIsBuy.MinimumWidth = 6;
            this.colDirIsBuy.Name = "colDirIsBuy";
            this.colDirIsBuy.Width = 125;
            // 
            // dtpCurrTimePicker
            // 
            this.dtpCurrTimePicker.CustomFormat = "dd-MM-yyyy HH:mm";
            this.dtpCurrTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCurrTimePicker.Location = new System.Drawing.Point(23, 103);
            this.dtpCurrTimePicker.Name = "dtpCurrTimePicker";
            this.dtpCurrTimePicker.Size = new System.Drawing.Size(200, 22);
            this.dtpCurrTimePicker.TabIndex = 20;
            this.dtpCurrTimePicker.Value = new System.DateTime(2024, 11, 3, 2, 4, 33, 0);
            // 
            // OptionsAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 575);
            this.Controls.Add(this.dtpCurrTimePicker);
            this.Controls.Add(this.dgOptTradeLegs);
            this.Controls.Add(this.chartRiskProfile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numPriceRange);
            this.Controls.Add(this.btnPlotRiskProfile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numRiskFreeRate);
            this.Controls.Add(this.numSpotPrice);
            this.Name = "OptionsAnalysisForm";
            this.Text = "OptionsAnalysisForm";
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptTradeLegs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numSpotPrice;
        private System.Windows.Forms.NumericUpDown numRiskFreeRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPlotRiskProfile;
        private System.Windows.Forms.NumericUpDown numPriceRange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRiskProfile;
        private System.Windows.Forms.DataGridView dgOptTradeLegs;
        private System.Windows.Forms.DataGridViewTextBoxColumn coExpiration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStrike;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolatility;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsCall;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDirIsBuy;
        private System.Windows.Forms.DateTimePicker dtpCurrTimePicker;
    }
}
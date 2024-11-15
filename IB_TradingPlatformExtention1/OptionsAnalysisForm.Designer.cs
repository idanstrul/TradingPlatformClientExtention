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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.numSpotPrice = new System.Windows.Forms.NumericUpDown();
            this.numRiskFreeRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPlotRiskProfile = new System.Windows.Forms.Button();
            this.numPriceRange = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chartRiskProfile = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgOptTradeLegs = new System.Windows.Forms.DataGridView();
            this.dtpCurrTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.numAtmIv = new System.Windows.Forms.NumericUpDown();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRight = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.coExpiration = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStrike = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptTradeLegs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAtmIv)).BeginInit();
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
            this.btnPlotRiskProfile.Location = new System.Drawing.Point(15, 159);
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
            this.numPriceRange.Location = new System.Drawing.Point(103, 103);
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
            this.label6.Location = new System.Drawing.Point(12, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Range";
            // 
            // chartRiskProfile
            // 
            chartArea2.AxisX.IntervalOffset = 1D;
            chartArea2.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Cross;
            chartArea2.Name = "ChartArea1";
            this.chartRiskProfile.ChartAreas.Add(chartArea2);
            this.chartRiskProfile.Location = new System.Drawing.Point(12, 222);
            this.chartRiskProfile.Name = "chartRiskProfile";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chartRiskProfile.Series.Add(series2);
            this.chartRiskProfile.Size = new System.Drawing.Size(1079, 388);
            this.chartRiskProfile.TabIndex = 18;
            this.chartRiskProfile.Text = "Risk profile";
            this.chartRiskProfile.MouseLeave += new System.EventHandler(this.chartRiskProfile_MouseLeave);
            this.chartRiskProfile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartRiskProfile_MouseMove);
            // 
            // dgOptTradeLegs
            // 
            this.dgOptTradeLegs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOptTradeLegs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colRight,
            this.coExpiration,
            this.colStrike,
            this.colVolatility,
            this.colQuantity});
            this.dgOptTradeLegs.Location = new System.Drawing.Point(251, 19);
            this.dgOptTradeLegs.Name = "dgOptTradeLegs";
            this.dgOptTradeLegs.RowHeadersWidth = 51;
            this.dgOptTradeLegs.RowTemplate.Height = 24;
            this.dgOptTradeLegs.Size = new System.Drawing.Size(776, 150);
            this.dgOptTradeLegs.TabIndex = 19;
            // 
            // dtpCurrTimePicker
            // 
            this.dtpCurrTimePicker.CustomFormat = "dd-MM-yyyy HH:mm";
            this.dtpCurrTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCurrTimePicker.Location = new System.Drawing.Point(23, 131);
            this.dtpCurrTimePicker.Name = "dtpCurrTimePicker";
            this.dtpCurrTimePicker.Size = new System.Drawing.Size(200, 22);
            this.dtpCurrTimePicker.TabIndex = 20;
            this.dtpCurrTimePicker.Value = new System.DateTime(2024, 11, 3, 2, 4, 33, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 22;
            this.label2.Text = "ATM IV";
            // 
            // numAtmIv
            // 
            this.numAtmIv.DecimalPlaces = 6;
            this.numAtmIv.Location = new System.Drawing.Point(103, 75);
            this.numAtmIv.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numAtmIv.Name = "numAtmIv";
            this.numAtmIv.Size = new System.Drawing.Size(120, 22);
            this.numAtmIv.TabIndex = 21;
            this.numAtmIv.Value = new decimal(new int[] {
            437,
            0,
            0,
            262144});
            // 
            // colSelected
            // 
            this.colSelected.HeaderText = "Selected";
            this.colSelected.MinimumWidth = 6;
            this.colSelected.Name = "colSelected";
            this.colSelected.Width = 125;
            // 
            // colRight
            // 
            this.colRight.HeaderText = "Right";
            this.colRight.Items.AddRange(new object[] {
            "CALL",
            "PUT"});
            this.colRight.MinimumWidth = 6;
            this.colRight.Name = "colRight";
            this.colRight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRight.Width = 125;
            // 
            // coExpiration
            // 
            this.coExpiration.HeaderText = "Expiration";
            this.coExpiration.MinimumWidth = 6;
            this.coExpiration.Name = "coExpiration";
            this.coExpiration.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.coExpiration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.coExpiration.Width = 125;
            // 
            // colStrike
            // 
            this.colStrike.HeaderText = "Strike";
            this.colStrike.MinimumWidth = 6;
            this.colStrike.Name = "colStrike";
            this.colStrike.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStrike.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colStrike.Width = 125;
            // 
            // colVolatility
            // 
            this.colVolatility.HeaderText = "Volatility";
            this.colVolatility.MinimumWidth = 6;
            this.colVolatility.Name = "colVolatility";
            this.colVolatility.Width = 125;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.MinimumWidth = 6;
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colQuantity.Width = 125;
            // 
            // OptionsAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 622);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numAtmIv);
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
            ((System.ComponentModel.ISupportInitialize)(this.numAtmIv)).EndInit();
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
        private System.Windows.Forms.DateTimePicker dtpCurrTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numAtmIv;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewComboBoxColumn colRight;
        private System.Windows.Forms.DataGridViewComboBoxColumn coExpiration;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStrike;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolatility;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
    }
}
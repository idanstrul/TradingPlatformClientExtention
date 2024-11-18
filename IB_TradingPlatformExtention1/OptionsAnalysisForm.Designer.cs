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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.colExpiration = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStrike = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAsk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLast = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOpenedInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemoveLeg = new System.Windows.Forms.DataGridViewButtonColumn();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.cbTif = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStopLossAdj = new System.Windows.Forms.Button();
            this.btnClosePos = new System.Windows.Forms.Button();
            this.numTrailStop = new System.Windows.Forms.NumericUpDown();
            this.numStopLoss = new System.Windows.Forms.NumericUpDown();
            this.cbTrailStop = new System.Windows.Forms.CheckBox();
            this.cbStopLoss = new System.Windows.Forms.CheckBox();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.btnCancelLast = new System.Windows.Forms.Button();
            this.btnAddLeg = new System.Windows.Forms.Button();
            this.btnIncreasePos = new System.Windows.Forms.Button();
            this.btnSell1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptTradeLegs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAtmIv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).BeginInit();
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
            chartArea5.AxisX.IntervalOffset = 1D;
            chartArea5.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea5.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea5.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea5.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea5.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea5.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea5.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Cross;
            chartArea5.Name = "ChartArea1";
            this.chartRiskProfile.ChartAreas.Add(chartArea5);
            this.chartRiskProfile.Location = new System.Drawing.Point(15, 324);
            this.chartRiskProfile.Name = "chartRiskProfile";
            series5.ChartArea = "ChartArea1";
            series5.Name = "Series1";
            this.chartRiskProfile.Series.Add(series5);
            this.chartRiskProfile.Size = new System.Drawing.Size(1079, 388);
            this.chartRiskProfile.TabIndex = 18;
            this.chartRiskProfile.Text = "Risk profile";
            this.chartRiskProfile.MouseLeave += new System.EventHandler(this.chartRiskProfile_MouseLeave);
            this.chartRiskProfile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartRiskProfile_MouseMove);
            // 
            // dgOptTradeLegs
            // 
            this.dgOptTradeLegs.AllowUserToAddRows = false;
            this.dgOptTradeLegs.AllowUserToDeleteRows = false;
            this.dgOptTradeLegs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgOptTradeLegs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOptTradeLegs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colRight,
            this.colExpiration,
            this.colStrike,
            this.colVolatility,
            this.colQuantity,
            this.colBid,
            this.colAsk,
            this.colLast,
            this.colOpenedInterest,
            this.colDelta,
            this.colRemoveLeg});
            this.dgOptTradeLegs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgOptTradeLegs.Location = new System.Drawing.Point(251, 19);
            this.dgOptTradeLegs.Name = "dgOptTradeLegs";
            this.dgOptTradeLegs.RowHeadersWidth = 51;
            this.dgOptTradeLegs.RowTemplate.Height = 24;
            this.dgOptTradeLegs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgOptTradeLegs.Size = new System.Drawing.Size(840, 150);
            this.dgOptTradeLegs.TabIndex = 19;
            this.dgOptTradeLegs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOptTradeLegs_CellClick);
            this.dgOptTradeLegs.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOptTradeLegs_CellValueChanged);
            this.dgOptTradeLegs.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgOptTradeLegs_RowsAdded);
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
            // 
            // colExpiration
            // 
            this.colExpiration.HeaderText = "Expiration";
            this.colExpiration.MinimumWidth = 6;
            this.colExpiration.Name = "colExpiration";
            this.colExpiration.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colExpiration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colStrike
            // 
            this.colStrike.HeaderText = "Strike";
            this.colStrike.MinimumWidth = 6;
            this.colStrike.Name = "colStrike";
            this.colStrike.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStrike.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colVolatility
            // 
            this.colVolatility.HeaderText = "IV %";
            this.colVolatility.MinimumWidth = 6;
            this.colVolatility.Name = "colVolatility";
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.MinimumWidth = 6;
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBid
            // 
            this.colBid.HeaderText = "Bid";
            this.colBid.MinimumWidth = 6;
            this.colBid.Name = "colBid";
            // 
            // colAsk
            // 
            this.colAsk.HeaderText = "Ask";
            this.colAsk.MinimumWidth = 6;
            this.colAsk.Name = "colAsk";
            // 
            // colLast
            // 
            this.colLast.HeaderText = "Last";
            this.colLast.MinimumWidth = 6;
            this.colLast.Name = "colLast";
            // 
            // colOpenedInterest
            // 
            this.colOpenedInterest.HeaderText = "Opened interest";
            this.colOpenedInterest.MinimumWidth = 6;
            this.colOpenedInterest.Name = "colOpenedInterest";
            // 
            // colDelta
            // 
            this.colDelta.HeaderText = "Delta";
            this.colDelta.MinimumWidth = 6;
            this.colDelta.Name = "colDelta";
            // 
            // colRemoveLeg
            // 
            this.colRemoveLeg.HeaderText = "Remove";
            this.colRemoveLeg.MinimumWidth = 6;
            this.colRemoveLeg.Name = "colRemoveLeg";
            this.colRemoveLeg.Text = "X";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(899, 250);
            this.numQuantity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(54, 22);
            this.numQuantity.TabIndex = 23;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbTif
            // 
            this.cbTif.FormattingEnabled = true;
            this.cbTif.Items.AddRange(new object[] {
            "DAY",
            "GTC"});
            this.cbTif.Location = new System.Drawing.Point(994, 250);
            this.cbTif.Name = "cbTif";
            this.cbTif.Size = new System.Drawing.Size(76, 24);
            this.cbTif.TabIndex = 24;
            this.cbTif.Text = "DAY";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(838, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Quantity";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(962, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "TIF";
            // 
            // btnStopLossAdj
            // 
            this.btnStopLossAdj.Location = new System.Drawing.Point(1024, 205);
            this.btnStopLossAdj.Name = "btnStopLossAdj";
            this.btnStopLossAdj.Size = new System.Drawing.Size(75, 23);
            this.btnStopLossAdj.TabIndex = 60;
            this.btnStopLossAdj.Text = "Adjust";
            this.btnStopLossAdj.UseVisualStyleBackColor = true;
            // 
            // btnClosePos
            // 
            this.btnClosePos.Location = new System.Drawing.Point(471, 235);
            this.btnClosePos.Name = "btnClosePos";
            this.btnClosePos.Size = new System.Drawing.Size(101, 23);
            this.btnClosePos.TabIndex = 59;
            this.btnClosePos.Text = "Close position";
            this.btnClosePos.UseVisualStyleBackColor = true;
            // 
            // numTrailStop
            // 
            this.numTrailStop.DecimalPlaces = 2;
            this.numTrailStop.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numTrailStop.Location = new System.Drawing.Point(932, 191);
            this.numTrailStop.Name = "numTrailStop";
            this.numTrailStop.Size = new System.Drawing.Size(86, 22);
            this.numTrailStop.TabIndex = 58;
            this.numTrailStop.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // numStopLoss
            // 
            this.numStopLoss.DecimalPlaces = 2;
            this.numStopLoss.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numStopLoss.Location = new System.Drawing.Point(932, 219);
            this.numStopLoss.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numStopLoss.Name = "numStopLoss";
            this.numStopLoss.Size = new System.Drawing.Size(86, 22);
            this.numStopLoss.TabIndex = 57;
            // 
            // cbTrailStop
            // 
            this.cbTrailStop.AutoSize = true;
            this.cbTrailStop.Location = new System.Drawing.Point(841, 193);
            this.cbTrailStop.Name = "cbTrailStop";
            this.cbTrailStop.Size = new System.Drawing.Size(85, 20);
            this.cbTrailStop.TabIndex = 56;
            this.cbTrailStop.Text = "Trail stop";
            this.cbTrailStop.UseVisualStyleBackColor = true;
            // 
            // cbStopLoss
            // 
            this.cbStopLoss.AutoSize = true;
            this.cbStopLoss.Location = new System.Drawing.Point(841, 221);
            this.cbStopLoss.Name = "cbStopLoss";
            this.cbStopLoss.Size = new System.Drawing.Size(85, 20);
            this.cbStopLoss.TabIndex = 55;
            this.cbStopLoss.Text = "Stop loss";
            this.cbStopLoss.UseVisualStyleBackColor = true;
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(703, 235);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(86, 23);
            this.btnCancelAll.TabIndex = 54;
            this.btnCancelAll.Text = "Cancel All";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            // 
            // btnCancelLast
            // 
            this.btnCancelLast.Location = new System.Drawing.Point(594, 235);
            this.btnCancelLast.Name = "btnCancelLast";
            this.btnCancelLast.Size = new System.Drawing.Size(86, 23);
            this.btnCancelLast.TabIndex = 53;
            this.btnCancelLast.Text = "Cancel Last";
            this.btnCancelLast.UseVisualStyleBackColor = true;
            // 
            // btnAddLeg
            // 
            this.btnAddLeg.Location = new System.Drawing.Point(251, 185);
            this.btnAddLeg.Name = "btnAddLeg";
            this.btnAddLeg.Size = new System.Drawing.Size(101, 23);
            this.btnAddLeg.TabIndex = 61;
            this.btnAddLeg.Text = "Add leg";
            this.btnAddLeg.UseVisualStyleBackColor = true;
            this.btnAddLeg.Click += new System.EventHandler(this.btnAddLeg_Click);
            // 
            // btnIncreasePos
            // 
            this.btnIncreasePos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnIncreasePos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncreasePos.ForeColor = System.Drawing.Color.White;
            this.btnIncreasePos.Location = new System.Drawing.Point(386, 181);
            this.btnIncreasePos.Name = "btnIncreasePos";
            this.btnIncreasePos.Size = new System.Drawing.Size(186, 37);
            this.btnIncreasePos.TabIndex = 63;
            this.btnIncreasePos.Text = "Increase position";
            this.btnIncreasePos.UseVisualStyleBackColor = false;
            // 
            // btnSell1
            // 
            this.btnSell1.BackColor = System.Drawing.Color.Red;
            this.btnSell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1.ForeColor = System.Drawing.Color.White;
            this.btnSell1.Location = new System.Drawing.Point(594, 181);
            this.btnSell1.Name = "btnSell1";
            this.btnSell1.Size = new System.Drawing.Size(195, 37);
            this.btnSell1.TabIndex = 62;
            this.btnSell1.Text = "Decrease position";
            this.btnSell1.UseVisualStyleBackColor = false;
            // 
            // OptionsAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 724);
            this.Controls.Add(this.btnIncreasePos);
            this.Controls.Add(this.btnSell1);
            this.Controls.Add(this.btnAddLeg);
            this.Controls.Add(this.btnStopLossAdj);
            this.Controls.Add(this.btnClosePos);
            this.Controls.Add(this.numTrailStop);
            this.Controls.Add(this.numStopLoss);
            this.Controls.Add(this.cbTrailStop);
            this.Controls.Add(this.cbStopLoss);
            this.Controls.Add(this.btnCancelAll);
            this.Controls.Add(this.btnCancelLast);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.cbTif);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
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
            this.Load += new System.EventHandler(this.OptionsAnalysisForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRiskProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptTradeLegs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAtmIv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).EndInit();
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
        private System.Windows.Forms.DataGridViewComboBoxColumn colExpiration;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStrike;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolatility;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAsk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLast;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOpenedInterest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDelta;
        private System.Windows.Forms.DataGridViewButtonColumn colRemoveLeg;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.ComboBox cbTif;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnStopLossAdj;
        private System.Windows.Forms.Button btnClosePos;
        private System.Windows.Forms.NumericUpDown numTrailStop;
        private System.Windows.Forms.NumericUpDown numStopLoss;
        private System.Windows.Forms.CheckBox cbTrailStop;
        private System.Windows.Forms.CheckBox cbStopLoss;
        private System.Windows.Forms.Button btnCancelAll;
        private System.Windows.Forms.Button btnCancelLast;
        private System.Windows.Forms.Button btnAddLeg;
        private System.Windows.Forms.Button btnIncreasePos;
        private System.Windows.Forms.Button btnSell1;
    }
}
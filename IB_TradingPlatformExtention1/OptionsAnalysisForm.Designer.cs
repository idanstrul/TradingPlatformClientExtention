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
            ((System.ComponentModel.ISupportInitialize)(this.numSpotPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStrikePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeToExpiration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiskFreeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVolatility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrOptPrice)).BeginInit();
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
            // OptionsAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}
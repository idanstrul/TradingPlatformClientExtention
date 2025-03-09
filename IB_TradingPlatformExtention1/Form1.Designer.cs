namespace IB_TradingPlatformExtention1
{
    partial class Form1
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
                if (client != null)
                {
                    client.OnDisconnected -= Client_OnDisconnected;
                    client.OnConnected -= Client_OnConnected;
                    client.OnTickPriceUpdated -= Client_OnTickPriceUpdated;
                }
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblDelayedDataWarning = new System.Windows.Forms.Label();
            this.tbSelectedContract = new System.Windows.Forms.TextBox();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.tbAsk = new System.Windows.Forms.TextBox();
            this.tbBid = new System.Windows.Forms.TextBox();
            this.btnStopLossAdj = new System.Windows.Forms.Button();
            this.btnClosePos = new System.Windows.Forms.Button();
            this.numTrailStop = new System.Windows.Forms.NumericUpDown();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.numStopLoss = new System.Windows.Forms.NumericUpDown();
            this.cbTrailStop = new System.Windows.Forms.CheckBox();
            this.cbStopLoss = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numTradeOffset = new System.Windows.Forms.NumericUpDown();
            this.btnBuy1_8 = new System.Windows.Forms.Button();
            this.btnBuy1_4 = new System.Windows.Forms.Button();
            this.btnBuy1_2 = new System.Windows.Forms.Button();
            this.btnSell1_8 = new System.Windows.Forms.Button();
            this.btnSell1_2 = new System.Windows.Forms.Button();
            this.btnSell1_4 = new System.Windows.Forms.Button();
            this.btnCancelLast = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.chkOutside = new System.Windows.Forms.CheckBox();
            this.cbTif = new System.Windows.Forms.ComboBox();
            this.btnBuy1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSell1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTradeOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(26, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(92, 36);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(140, 20);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(44, 16);
            this.lblConnectionStatus.TabIndex = 36;
            this.lblConnectionStatus.Text = "label4";
            // 
            // lblDelayedDataWarning
            // 
            this.lblDelayedDataWarning.AutoSize = true;
            this.lblDelayedDataWarning.BackColor = System.Drawing.Color.Black;
            this.lblDelayedDataWarning.ForeColor = System.Drawing.Color.Yellow;
            this.lblDelayedDataWarning.Location = new System.Drawing.Point(229, 20);
            this.lblDelayedDataWarning.Name = "lblDelayedDataWarning";
            this.lblDelayedDataWarning.Size = new System.Drawing.Size(0, 16);
            this.lblDelayedDataWarning.TabIndex = 37;
            // 
            // tbSelectedContract
            // 
            this.tbSelectedContract.Location = new System.Drawing.Point(100, 62);
            this.tbSelectedContract.Name = "tbSelectedContract";
            this.tbSelectedContract.ReadOnly = true;
            this.tbSelectedContract.Size = new System.Drawing.Size(383, 22);
            this.tbSelectedContract.TabIndex = 84;
            // 
            // tbLast
            // 
            this.tbLast.Location = new System.Drawing.Point(321, 93);
            this.tbLast.Name = "tbLast";
            this.tbLast.ReadOnly = true;
            this.tbLast.Size = new System.Drawing.Size(76, 22);
            this.tbLast.TabIndex = 64;
            this.tbLast.Text = "0.00";
            // 
            // tbAsk
            // 
            this.tbAsk.Location = new System.Drawing.Point(190, 92);
            this.tbAsk.Name = "tbAsk";
            this.tbAsk.ReadOnly = true;
            this.tbAsk.Size = new System.Drawing.Size(76, 22);
            this.tbAsk.TabIndex = 63;
            this.tbAsk.Text = "0.00";
            // 
            // tbBid
            // 
            this.tbBid.Location = new System.Drawing.Point(63, 93);
            this.tbBid.Name = "tbBid";
            this.tbBid.ReadOnly = true;
            this.tbBid.Size = new System.Drawing.Size(78, 22);
            this.tbBid.TabIndex = 62;
            this.tbBid.Text = "0.00";
            // 
            // btnStopLossAdj
            // 
            this.btnStopLossAdj.Location = new System.Drawing.Point(413, 287);
            this.btnStopLossAdj.Name = "btnStopLossAdj";
            this.btnStopLossAdj.Size = new System.Drawing.Size(75, 23);
            this.btnStopLossAdj.TabIndex = 83;
            this.btnStopLossAdj.Text = "Adjust";
            this.btnStopLossAdj.UseVisualStyleBackColor = true;
            // 
            // btnClosePos
            // 
            this.btnClosePos.Location = new System.Drawing.Point(40, 312);
            this.btnClosePos.Name = "btnClosePos";
            this.btnClosePos.Size = new System.Drawing.Size(125, 23);
            this.btnClosePos.TabIndex = 82;
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
            this.numTrailStop.Location = new System.Drawing.Point(321, 273);
            this.numTrailStop.Name = "numTrailStop";
            this.numTrailStop.Size = new System.Drawing.Size(86, 22);
            this.numTrailStop.TabIndex = 81;
            this.numTrailStop.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(40, 282);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(125, 23);
            this.btnCancelAll.TabIndex = 69;
            this.btnCancelAll.Text = "Cancel all orders";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            // 
            // numStopLoss
            // 
            this.numStopLoss.DecimalPlaces = 2;
            this.numStopLoss.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numStopLoss.Location = new System.Drawing.Point(321, 301);
            this.numStopLoss.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numStopLoss.Name = "numStopLoss";
            this.numStopLoss.Size = new System.Drawing.Size(86, 22);
            this.numStopLoss.TabIndex = 80;
            // 
            // cbTrailStop
            // 
            this.cbTrailStop.AutoSize = true;
            this.cbTrailStop.Location = new System.Drawing.Point(230, 275);
            this.cbTrailStop.Name = "cbTrailStop";
            this.cbTrailStop.Size = new System.Drawing.Size(85, 20);
            this.cbTrailStop.TabIndex = 79;
            this.cbTrailStop.Text = "Trail stop";
            this.cbTrailStop.UseVisualStyleBackColor = true;
            // 
            // cbStopLoss
            // 
            this.cbStopLoss.AutoSize = true;
            this.cbStopLoss.Location = new System.Drawing.Point(230, 303);
            this.cbStopLoss.Name = "cbStopLoss";
            this.cbStopLoss.Size = new System.Drawing.Size(85, 20);
            this.cbStopLoss.TabIndex = 78;
            this.cbStopLoss.Text = "Stop loss";
            this.cbStopLoss.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(395, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 77;
            this.label3.Text = "Trade price offset";
            // 
            // numTradeOffset
            // 
            this.numTradeOffset.DecimalPlaces = 2;
            this.numTradeOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numTradeOffset.Location = new System.Drawing.Point(398, 208);
            this.numTradeOffset.Name = "numTradeOffset";
            this.numTradeOffset.Size = new System.Drawing.Size(86, 22);
            this.numTradeOffset.TabIndex = 76;
            this.numTradeOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // btnBuy1_8
            // 
            this.btnBuy1_8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1_8.ForeColor = System.Drawing.Color.White;
            this.btnBuy1_8.Location = new System.Drawing.Point(289, 165);
            this.btnBuy1_8.Name = "btnBuy1_8";
            this.btnBuy1_8.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_8.TabIndex = 75;
            this.btnBuy1_8.Text = "B 1/8";
            this.btnBuy1_8.UseVisualStyleBackColor = false;
            // 
            // btnBuy1_4
            // 
            this.btnBuy1_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1_4.ForeColor = System.Drawing.Color.White;
            this.btnBuy1_4.Location = new System.Drawing.Point(204, 165);
            this.btnBuy1_4.Name = "btnBuy1_4";
            this.btnBuy1_4.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_4.TabIndex = 74;
            this.btnBuy1_4.Text = "B 1/4";
            this.btnBuy1_4.UseVisualStyleBackColor = false;
            // 
            // btnBuy1_2
            // 
            this.btnBuy1_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1_2.ForeColor = System.Drawing.Color.White;
            this.btnBuy1_2.Location = new System.Drawing.Point(119, 165);
            this.btnBuy1_2.Name = "btnBuy1_2";
            this.btnBuy1_2.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_2.TabIndex = 73;
            this.btnBuy1_2.Text = "B 1/2";
            this.btnBuy1_2.UseVisualStyleBackColor = false;
            // 
            // btnSell1_8
            // 
            this.btnSell1_8.BackColor = System.Drawing.Color.Red;
            this.btnSell1_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_8.ForeColor = System.Drawing.Color.White;
            this.btnSell1_8.Location = new System.Drawing.Point(289, 208);
            this.btnSell1_8.Name = "btnSell1_8";
            this.btnSell1_8.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_8.TabIndex = 72;
            this.btnSell1_8.Text = "S 1/8";
            this.btnSell1_8.UseVisualStyleBackColor = false;
            // 
            // btnSell1_2
            // 
            this.btnSell1_2.BackColor = System.Drawing.Color.Red;
            this.btnSell1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_2.ForeColor = System.Drawing.Color.White;
            this.btnSell1_2.Location = new System.Drawing.Point(119, 208);
            this.btnSell1_2.Name = "btnSell1_2";
            this.btnSell1_2.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_2.TabIndex = 71;
            this.btnSell1_2.Text = "S 1/2";
            this.btnSell1_2.UseVisualStyleBackColor = false;
            // 
            // btnSell1_4
            // 
            this.btnSell1_4.BackColor = System.Drawing.Color.Red;
            this.btnSell1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_4.ForeColor = System.Drawing.Color.White;
            this.btnSell1_4.Location = new System.Drawing.Point(204, 208);
            this.btnSell1_4.Name = "btnSell1_4";
            this.btnSell1_4.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_4.TabIndex = 70;
            this.btnSell1_4.Text = "S 1/4";
            this.btnSell1_4.UseVisualStyleBackColor = false;
            // 
            // btnCancelLast
            // 
            this.btnCancelLast.Location = new System.Drawing.Point(40, 251);
            this.btnCancelLast.Name = "btnCancelLast";
            this.btnCancelLast.Size = new System.Drawing.Size(126, 23);
            this.btnCancelLast.TabIndex = 68;
            this.btnCancelLast.Text = "Cancel last order ";
            this.btnCancelLast.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "Contract: ";
            // 
            // numQuantity
            // 
            this.numQuantity.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numQuantity.Location = new System.Drawing.Point(92, 122);
            this.numQuantity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(74, 22);
            this.numQuantity.TabIndex = 54;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkOutside
            // 
            this.chkOutside.AutoSize = true;
            this.chkOutside.Location = new System.Drawing.Point(289, 124);
            this.chkOutside.Name = "chkOutside";
            this.chkOutside.Size = new System.Drawing.Size(104, 20);
            this.chkOutside.TabIndex = 67;
            this.chkOutside.Text = "OutsideRTH";
            this.chkOutside.UseVisualStyleBackColor = true;
            // 
            // cbTif
            // 
            this.cbTif.FormattingEnabled = true;
            this.cbTif.Items.AddRange(new object[] {
            "DAY",
            "GTC"});
            this.cbTif.Location = new System.Drawing.Point(207, 122);
            this.cbTif.Name = "cbTif";
            this.cbTif.Size = new System.Drawing.Size(76, 24);
            this.cbTif.TabIndex = 55;
            this.cbTif.Text = "DAY";
            // 
            // btnBuy1
            // 
            this.btnBuy1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1.ForeColor = System.Drawing.Color.White;
            this.btnBuy1.Location = new System.Drawing.Point(34, 165);
            this.btnBuy1.Name = "btnBuy1";
            this.btnBuy1.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1.TabIndex = 66;
            this.btnBuy1.Text = "B 1";
            this.btnBuy1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "Quantity";
            // 
            // btnSell1
            // 
            this.btnSell1.BackColor = System.Drawing.Color.Red;
            this.btnSell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1.ForeColor = System.Drawing.Color.White;
            this.btnSell1.Location = new System.Drawing.Point(34, 208);
            this.btnSell1.Name = "btnSell1";
            this.btnSell1.Size = new System.Drawing.Size(79, 37);
            this.btnSell1.TabIndex = 65;
            this.btnSell1.Text = "S 1";
            this.btnSell1.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(175, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 16);
            this.label8.TabIndex = 58;
            this.label8.Text = "TIF";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 16);
            this.label9.TabIndex = 59;
            this.label9.Text = "Bid";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(283, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 61;
            this.label11.Text = "Last";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(154, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 16);
            this.label10.TabIndex = 60;
            this.label10.Text = "Ask";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 388);
            this.Controls.Add(this.tbSelectedContract);
            this.Controls.Add(this.tbLast);
            this.Controls.Add(this.tbAsk);
            this.Controls.Add(this.tbBid);
            this.Controls.Add(this.btnStopLossAdj);
            this.Controls.Add(this.btnClosePos);
            this.Controls.Add(this.numTrailStop);
            this.Controls.Add(this.btnCancelAll);
            this.Controls.Add(this.numStopLoss);
            this.Controls.Add(this.cbTrailStop);
            this.Controls.Add(this.cbStopLoss);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numTradeOffset);
            this.Controls.Add(this.btnBuy1_8);
            this.Controls.Add(this.btnBuy1_4);
            this.Controls.Add(this.btnBuy1_2);
            this.Controls.Add(this.btnSell1_8);
            this.Controls.Add(this.btnSell1_2);
            this.Controls.Add(this.btnSell1_4);
            this.Controls.Add(this.btnCancelLast);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.chkOutside);
            this.Controls.Add(this.cbTif);
            this.Controls.Add(this.btnBuy1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSell1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblDelayedDataWarning);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTradeOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblDelayedDataWarning;
        private System.Windows.Forms.TextBox tbSelectedContract;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.TextBox tbAsk;
        private System.Windows.Forms.TextBox tbBid;
        private System.Windows.Forms.Button btnStopLossAdj;
        private System.Windows.Forms.Button btnClosePos;
        private System.Windows.Forms.NumericUpDown numTrailStop;
        private System.Windows.Forms.Button btnCancelAll;
        private System.Windows.Forms.NumericUpDown numStopLoss;
        private System.Windows.Forms.CheckBox cbTrailStop;
        private System.Windows.Forms.CheckBox cbStopLoss;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numTradeOffset;
        private System.Windows.Forms.Button btnBuy1_8;
        private System.Windows.Forms.Button btnBuy1_4;
        private System.Windows.Forms.Button btnBuy1_2;
        private System.Windows.Forms.Button btnSell1_8;
        private System.Windows.Forms.Button btnSell1_2;
        private System.Windows.Forms.Button btnSell1_4;
        private System.Windows.Forms.Button btnCancelLast;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.CheckBox chkOutside;
        private System.Windows.Forms.ComboBox cbTif;
        private System.Windows.Forms.Button btnBuy1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSell1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
    }
}


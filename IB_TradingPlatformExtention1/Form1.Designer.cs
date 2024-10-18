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
            this.cbSymbol = new System.Windows.Forms.ComboBox();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.cbMarket = new System.Windows.Forms.ComboBox();
            this.cbTif = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbPrimaryEx = new System.Windows.Forms.TextBox();
            this.tbBid = new System.Windows.Forms.TextBox();
            this.tbAsk = new System.Windows.Forms.TextBox();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnBuy = new System.Windows.Forms.Button();
            this.chkOutside = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPtofitTarget = new System.Windows.Forms.TextBox();
            this.tbStopLoss = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.btnCancelLast = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
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
            // cbSymbol
            // 
            this.cbSymbol.FormattingEnabled = true;
            this.cbSymbol.Items.AddRange(new object[] {
            "MSFT ",
            "TSLA ",
            "IBM"});
            this.cbSymbol.Location = new System.Drawing.Point(9, 31);
            this.cbSymbol.Name = "cbSymbol";
            this.cbSymbol.Size = new System.Drawing.Size(92, 24);
            this.cbSymbol.TabIndex = 1;
            this.cbSymbol.Text = "MSFT ";
            this.cbSymbol.SelectedIndexChanged += new System.EventHandler(this.cbSymbol_SelectedIndexChanged);
            this.cbSymbol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbSymbol_KeyDown);
            this.cbSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbSymbol_KeyPress);
            // 
            // numQuantity
            // 
            this.numQuantity.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numQuantity.Location = new System.Drawing.Point(107, 33);
            this.numQuantity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(120, 22);
            this.numQuantity.TabIndex = 3;
            this.numQuantity.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // cbMarket
            // 
            this.cbMarket.FormattingEnabled = true;
            this.cbMarket.Items.AddRange(new object[] {
            "SMART",
            "ARCA",
            "BATS",
            "ISLAND"});
            this.cbMarket.Location = new System.Drawing.Point(359, 31);
            this.cbMarket.Name = "cbMarket";
            this.cbMarket.Size = new System.Drawing.Size(121, 24);
            this.cbMarket.TabIndex = 5;
            this.cbMarket.Text = "SMART";
            // 
            // cbTif
            // 
            this.cbTif.FormattingEnabled = true;
            this.cbTif.Items.AddRange(new object[] {
            "DAY",
            "GTC"});
            this.cbTif.Location = new System.Drawing.Point(359, 75);
            this.cbTif.Name = "cbTif";
            this.cbTif.Size = new System.Drawing.Size(121, 24);
            this.cbTif.TabIndex = 7;
            this.cbTif.Text = "DAY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Symbol";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(356, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Market";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(239, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Primary Ex";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(356, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "TIF";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(133, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Bid";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(239, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 16);
            this.label10.TabIndex = 17;
            this.label10.Text = "Ask";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(195, 153);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 18;
            this.label11.Text = "Last";
            // 
            // tbPrimaryEx
            // 
            this.tbPrimaryEx.Location = new System.Drawing.Point(242, 77);
            this.tbPrimaryEx.Name = "tbPrimaryEx";
            this.tbPrimaryEx.Size = new System.Drawing.Size(100, 22);
            this.tbPrimaryEx.TabIndex = 20;
            this.tbPrimaryEx.Text = "NASDAQ";
            // 
            // tbBid
            // 
            this.tbBid.Location = new System.Drawing.Point(136, 122);
            this.tbBid.Name = "tbBid";
            this.tbBid.Size = new System.Drawing.Size(100, 22);
            this.tbBid.TabIndex = 21;
            this.tbBid.Text = "0.00";
            this.tbBid.Click += new System.EventHandler(this.tbBid_Click);
            // 
            // tbAsk
            // 
            this.tbAsk.Location = new System.Drawing.Point(242, 122);
            this.tbAsk.Name = "tbAsk";
            this.tbAsk.Size = new System.Drawing.Size(100, 22);
            this.tbAsk.TabIndex = 22;
            this.tbAsk.Text = "0.00";
            this.tbAsk.Click += new System.EventHandler(this.tbAsk_Click);
            // 
            // tbLast
            // 
            this.tbLast.Location = new System.Drawing.Point(242, 150);
            this.tbLast.Name = "tbLast";
            this.tbLast.Size = new System.Drawing.Size(100, 22);
            this.tbLast.TabIndex = 23;
            this.tbLast.Text = "0.00";
            this.tbLast.Click += new System.EventHandler(this.tbLast_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(137, 10);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(92, 36);
            this.btnDisconnect.TabIndex = 26;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.Red;
            this.btnSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell.ForeColor = System.Drawing.Color.White;
            this.btnSell.Location = new System.Drawing.Point(9, 110);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(97, 37);
            this.btnSell.TabIndex = 27;
            this.btnSell.Text = "SELL";
            this.btnSell.UseVisualStyleBackColor = false;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy.ForeColor = System.Drawing.Color.White;
            this.btnBuy.Location = new System.Drawing.Point(359, 107);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(97, 37);
            this.btnBuy.TabIndex = 28;
            this.btnBuy.Text = "BUY";
            this.btnBuy.UseVisualStyleBackColor = false;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // chkOutside
            // 
            this.chkOutside.AutoSize = true;
            this.chkOutside.Location = new System.Drawing.Point(9, 153);
            this.chkOutside.Name = "chkOutside";
            this.chkOutside.Size = new System.Drawing.Size(104, 20);
            this.chkOutside.TabIndex = 29;
            this.chkOutside.Text = "OutsideRTH";
            this.chkOutside.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(294, 219);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 16);
            this.label13.TabIndex = 31;
            this.label13.Text = "Profit Target";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(307, 191);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 16);
            this.label14.TabIndex = 32;
            this.label14.Text = "Stop Loss";
            // 
            // tbPtofitTarget
            // 
            this.tbPtofitTarget.Location = new System.Drawing.Point(380, 216);
            this.tbPtofitTarget.Name = "tbPtofitTarget";
            this.tbPtofitTarget.Size = new System.Drawing.Size(100, 22);
            this.tbPtofitTarget.TabIndex = 33;
            this.tbPtofitTarget.Text = "0.00";
            // 
            // tbStopLoss
            // 
            this.tbStopLoss.Location = new System.Drawing.Point(380, 188);
            this.tbStopLoss.Name = "tbStopLoss";
            this.tbStopLoss.Size = new System.Drawing.Size(100, 22);
            this.tbStopLoss.TabIndex = 34;
            this.tbStopLoss.Text = "0.00";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(26, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 296);
            this.tabControl1.TabIndex = 35;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnCancelAll);
            this.tabPage1.Controls.Add(this.btnCancelLast);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbStopLoss);
            this.tabPage1.Controls.Add(this.cbSymbol);
            this.tabPage1.Controls.Add(this.tbPtofitTarget);
            this.tabPage1.Controls.Add(this.numQuantity);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.numPrice);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.cbMarket);
            this.tabPage1.Controls.Add(this.chkOutside);
            this.tabPage1.Controls.Add(this.cbTif);
            this.tabPage1.Controls.Add(this.btnBuy);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSell);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.tbLast);
            this.tabPage1.Controls.Add(this.tbAsk);
            this.tabPage1.Controls.Add(this.tbBid);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.tbPrimaryEx);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(490, 267);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(27, 212);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(86, 23);
            this.btnCancelAll.TabIndex = 36;
            this.btnCancelAll.Text = "Cancel All";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
            // 
            // btnCancelLast
            // 
            this.btnCancelLast.Location = new System.Drawing.Point(27, 188);
            this.btnCancelLast.Name = "btnCancelLast";
            this.btnCancelLast.Size = new System.Drawing.Size(86, 23);
            this.btnCancelLast.TabIndex = 35;
            this.btnCancelLast.Text = "Cancel Last";
            this.btnCancelLast.UseVisualStyleBackColor = true;
            this.btnCancelLast.Click += new System.EventHandler(this.btnCancelLast_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(490, 267);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPrice.Location = new System.Drawing.Point(233, 33);
            this.numPrice.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(120, 22);
            this.numPrice.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(230, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Limit Price";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 360);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbSymbol;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.ComboBox cbMarket;
        private System.Windows.Forms.ComboBox cbTif;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbPrimaryEx;
        private System.Windows.Forms.TextBox tbBid;
        private System.Windows.Forms.TextBox tbAsk;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.CheckBox chkOutside;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbPtofitTarget;
        private System.Windows.Forms.TextBox tbStopLoss;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnCancelAll;
        private System.Windows.Forms.Button btnCancelLast;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label label3;
    }
}


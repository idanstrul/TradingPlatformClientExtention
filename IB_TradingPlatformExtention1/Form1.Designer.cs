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
            this.cbSymbol = new System.Windows.Forms.ComboBox();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.cbTif = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBid = new System.Windows.Forms.TextBox();
            this.tbAsk = new System.Windows.Forms.TextBox();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSell1 = new System.Windows.Forms.Button();
            this.btnBuy1 = new System.Windows.Forms.Button();
            this.chkOutside = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnOptionsAnalysis = new System.Windows.Forms.Button();
            this.btnStopLossAdj = new System.Windows.Forms.Button();
            this.btnClosePos = new System.Windows.Forms.Button();
            this.numTrailStop = new System.Windows.Forms.NumericUpDown();
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
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.btnCancelLast = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTradeOffset)).BeginInit();
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
            this.cbSymbol.DisplayMember = "Description";
            this.cbSymbol.DropDownWidth = 400;
            this.cbSymbol.FormattingEnabled = true;
            this.cbSymbol.Location = new System.Drawing.Point(65, 9);
            this.cbSymbol.Name = "cbSymbol";
            this.cbSymbol.Size = new System.Drawing.Size(409, 24);
            this.cbSymbol.TabIndex = 1;
            this.cbSymbol.ValueMember = "ConId";
            this.cbSymbol.SelectionChangeCommitted += new System.EventHandler(this.cbSymbol_SelectionChangeCommitted);
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
            this.numQuantity.Location = new System.Drawing.Point(67, 69);
            this.numQuantity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(74, 22);
            this.numQuantity.TabIndex = 3;
            this.numQuantity.Value = new decimal(new int[] {
            30,
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
            this.cbTif.Location = new System.Drawing.Point(182, 69);
            this.cbTif.Name = "cbTif";
            this.cbTif.Size = new System.Drawing.Size(76, 24);
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
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Quantity";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(150, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "TIF";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Bid";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(129, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 16);
            this.label10.TabIndex = 17;
            this.label10.Text = "Ask";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(258, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 18;
            this.label11.Text = "Last";
            // 
            // tbBid
            // 
            this.tbBid.Location = new System.Drawing.Point(38, 40);
            this.tbBid.Name = "tbBid";
            this.tbBid.Size = new System.Drawing.Size(78, 22);
            this.tbBid.TabIndex = 21;
            this.tbBid.Text = "0.00";
            // 
            // tbAsk
            // 
            this.tbAsk.Location = new System.Drawing.Point(165, 39);
            this.tbAsk.Name = "tbAsk";
            this.tbAsk.Size = new System.Drawing.Size(76, 22);
            this.tbAsk.TabIndex = 22;
            this.tbAsk.Text = "0.00";
            // 
            // tbLast
            // 
            this.tbLast.Location = new System.Drawing.Point(296, 40);
            this.tbLast.Name = "tbLast";
            this.tbLast.Size = new System.Drawing.Size(76, 22);
            this.tbLast.TabIndex = 23;
            this.tbLast.Text = "0.00";
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
            // btnSell1
            // 
            this.btnSell1.BackColor = System.Drawing.Color.Red;
            this.btnSell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1.ForeColor = System.Drawing.Color.White;
            this.btnSell1.Location = new System.Drawing.Point(9, 155);
            this.btnSell1.Name = "btnSell1";
            this.btnSell1.Size = new System.Drawing.Size(79, 37);
            this.btnSell1.TabIndex = 27;
            this.btnSell1.Text = "S 1";
            this.btnSell1.UseVisualStyleBackColor = false;
            this.btnSell1.Click += new System.EventHandler(this.btnSell1_Click);
            // 
            // btnBuy1
            // 
            this.btnBuy1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1.ForeColor = System.Drawing.Color.White;
            this.btnBuy1.Location = new System.Drawing.Point(9, 112);
            this.btnBuy1.Name = "btnBuy1";
            this.btnBuy1.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1.TabIndex = 28;
            this.btnBuy1.Text = "B 1";
            this.btnBuy1.UseVisualStyleBackColor = false;
            this.btnBuy1.Click += new System.EventHandler(this.btnBuy1_Click);
            // 
            // chkOutside
            // 
            this.chkOutside.AutoSize = true;
            this.chkOutside.Location = new System.Drawing.Point(264, 71);
            this.chkOutside.Name = "chkOutside";
            this.chkOutside.Size = new System.Drawing.Size(104, 20);
            this.chkOutside.TabIndex = 29;
            this.chkOutside.Text = "OutsideRTH";
            this.chkOutside.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(26, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 324);
            this.tabControl1.TabIndex = 35;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnOptionsAnalysis);
            this.tabPage1.Controls.Add(this.btnStopLossAdj);
            this.tabPage1.Controls.Add(this.btnClosePos);
            this.tabPage1.Controls.Add(this.numTrailStop);
            this.tabPage1.Controls.Add(this.numStopLoss);
            this.tabPage1.Controls.Add(this.cbTrailStop);
            this.tabPage1.Controls.Add(this.cbStopLoss);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.numTradeOffset);
            this.tabPage1.Controls.Add(this.btnBuy1_8);
            this.tabPage1.Controls.Add(this.btnBuy1_4);
            this.tabPage1.Controls.Add(this.btnBuy1_2);
            this.tabPage1.Controls.Add(this.btnSell1_8);
            this.tabPage1.Controls.Add(this.btnSell1_2);
            this.tabPage1.Controls.Add(this.btnSell1_4);
            this.tabPage1.Controls.Add(this.btnCancelAll);
            this.tabPage1.Controls.Add(this.btnCancelLast);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cbSymbol);
            this.tabPage1.Controls.Add(this.numQuantity);
            this.tabPage1.Controls.Add(this.chkOutside);
            this.tabPage1.Controls.Add(this.cbTif);
            this.tabPage1.Controls.Add(this.btnBuy1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSell1);
            this.tabPage1.Controls.Add(this.tbLast);
            this.tabPage1.Controls.Add(this.tbAsk);
            this.tabPage1.Controls.Add(this.tbBid);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(490, 295);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnOptionsAnalysis
            // 
            this.btnOptionsAnalysis.Location = new System.Drawing.Point(122, 252);
            this.btnOptionsAnalysis.Name = "btnOptionsAnalysis";
            this.btnOptionsAnalysis.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsAnalysis.TabIndex = 52;
            this.btnOptionsAnalysis.Text = "Options analysis";
            this.btnOptionsAnalysis.UseVisualStyleBackColor = true;
            this.btnOptionsAnalysis.Click += new System.EventHandler(this.btnOptionsAnalysis_Click);
            // 
            // btnStopLossAdj
            // 
            this.btnStopLossAdj.Location = new System.Drawing.Point(388, 234);
            this.btnStopLossAdj.Name = "btnStopLossAdj";
            this.btnStopLossAdj.Size = new System.Drawing.Size(75, 23);
            this.btnStopLossAdj.TabIndex = 51;
            this.btnStopLossAdj.Text = "Adjust";
            this.btnStopLossAdj.UseVisualStyleBackColor = true;
            this.btnStopLossAdj.Click += new System.EventHandler(this.btnStopLossAdj_Click);
            // 
            // btnClosePos
            // 
            this.btnClosePos.Location = new System.Drawing.Point(15, 252);
            this.btnClosePos.Name = "btnClosePos";
            this.btnClosePos.Size = new System.Drawing.Size(101, 23);
            this.btnClosePos.TabIndex = 50;
            this.btnClosePos.Text = "Close position";
            this.btnClosePos.UseVisualStyleBackColor = true;
            this.btnClosePos.Click += new System.EventHandler(this.btnClosePos_Click);
            // 
            // numTrailStop
            // 
            this.numTrailStop.DecimalPlaces = 2;
            this.numTrailStop.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numTrailStop.Location = new System.Drawing.Point(296, 220);
            this.numTrailStop.Name = "numTrailStop";
            this.numTrailStop.Size = new System.Drawing.Size(86, 22);
            this.numTrailStop.TabIndex = 49;
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
            this.numStopLoss.Location = new System.Drawing.Point(296, 248);
            this.numStopLoss.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numStopLoss.Name = "numStopLoss";
            this.numStopLoss.Size = new System.Drawing.Size(86, 22);
            this.numStopLoss.TabIndex = 48;
            // 
            // cbTrailStop
            // 
            this.cbTrailStop.AutoSize = true;
            this.cbTrailStop.Location = new System.Drawing.Point(205, 222);
            this.cbTrailStop.Name = "cbTrailStop";
            this.cbTrailStop.Size = new System.Drawing.Size(85, 20);
            this.cbTrailStop.TabIndex = 47;
            this.cbTrailStop.Text = "Trail stop";
            this.cbTrailStop.UseVisualStyleBackColor = true;
            this.cbTrailStop.CheckedChanged += new System.EventHandler(this.cbTrailStop_CheckedChanged);
            // 
            // cbStopLoss
            // 
            this.cbStopLoss.AutoSize = true;
            this.cbStopLoss.Location = new System.Drawing.Point(205, 250);
            this.cbStopLoss.Name = "cbStopLoss";
            this.cbStopLoss.Size = new System.Drawing.Size(85, 20);
            this.cbStopLoss.TabIndex = 45;
            this.cbStopLoss.Text = "Stop loss";
            this.cbStopLoss.UseVisualStyleBackColor = true;
            this.cbStopLoss.CheckedChanged += new System.EventHandler(this.cbStopLoss_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 44;
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
            this.numTradeOffset.Location = new System.Drawing.Point(373, 155);
            this.numTradeOffset.Name = "numTradeOffset";
            this.numTradeOffset.Size = new System.Drawing.Size(86, 22);
            this.numTradeOffset.TabIndex = 43;
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
            this.btnBuy1_8.Location = new System.Drawing.Point(264, 112);
            this.btnBuy1_8.Name = "btnBuy1_8";
            this.btnBuy1_8.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_8.TabIndex = 42;
            this.btnBuy1_8.Text = "B 1/8";
            this.btnBuy1_8.UseVisualStyleBackColor = false;
            this.btnBuy1_8.Click += new System.EventHandler(this.btnBuy1_8_Click);
            // 
            // btnBuy1_4
            // 
            this.btnBuy1_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1_4.ForeColor = System.Drawing.Color.White;
            this.btnBuy1_4.Location = new System.Drawing.Point(179, 112);
            this.btnBuy1_4.Name = "btnBuy1_4";
            this.btnBuy1_4.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_4.TabIndex = 41;
            this.btnBuy1_4.Text = "B 1/4";
            this.btnBuy1_4.UseVisualStyleBackColor = false;
            this.btnBuy1_4.Click += new System.EventHandler(this.btnBuy1_4_Click);
            // 
            // btnBuy1_2
            // 
            this.btnBuy1_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBuy1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy1_2.ForeColor = System.Drawing.Color.White;
            this.btnBuy1_2.Location = new System.Drawing.Point(94, 112);
            this.btnBuy1_2.Name = "btnBuy1_2";
            this.btnBuy1_2.Size = new System.Drawing.Size(79, 37);
            this.btnBuy1_2.TabIndex = 40;
            this.btnBuy1_2.Text = "B 1/2";
            this.btnBuy1_2.UseVisualStyleBackColor = false;
            this.btnBuy1_2.Click += new System.EventHandler(this.btnBuy1_2_Click);
            // 
            // btnSell1_8
            // 
            this.btnSell1_8.BackColor = System.Drawing.Color.Red;
            this.btnSell1_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_8.ForeColor = System.Drawing.Color.White;
            this.btnSell1_8.Location = new System.Drawing.Point(264, 155);
            this.btnSell1_8.Name = "btnSell1_8";
            this.btnSell1_8.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_8.TabIndex = 39;
            this.btnSell1_8.Text = "S 1/8";
            this.btnSell1_8.UseVisualStyleBackColor = false;
            this.btnSell1_8.Click += new System.EventHandler(this.btnSell1_8_Click);
            // 
            // btnSell1_2
            // 
            this.btnSell1_2.BackColor = System.Drawing.Color.Red;
            this.btnSell1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_2.ForeColor = System.Drawing.Color.White;
            this.btnSell1_2.Location = new System.Drawing.Point(94, 155);
            this.btnSell1_2.Name = "btnSell1_2";
            this.btnSell1_2.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_2.TabIndex = 38;
            this.btnSell1_2.Text = "S 1/2";
            this.btnSell1_2.UseVisualStyleBackColor = false;
            this.btnSell1_2.Click += new System.EventHandler(this.btnSell1_2_Click);
            // 
            // btnSell1_4
            // 
            this.btnSell1_4.BackColor = System.Drawing.Color.Red;
            this.btnSell1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell1_4.ForeColor = System.Drawing.Color.White;
            this.btnSell1_4.Location = new System.Drawing.Point(179, 155);
            this.btnSell1_4.Name = "btnSell1_4";
            this.btnSell1_4.Size = new System.Drawing.Size(79, 37);
            this.btnSell1_4.TabIndex = 37;
            this.btnSell1_4.Text = "S 1/4";
            this.btnSell1_4.UseVisualStyleBackColor = false;
            this.btnSell1_4.Click += new System.EventHandler(this.btnSell1_4_Click);
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(113, 219);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(86, 23);
            this.btnCancelAll.TabIndex = 36;
            this.btnCancelAll.Text = "Cancel All";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
            // 
            // btnCancelLast
            // 
            this.btnCancelLast.Location = new System.Drawing.Point(15, 219);
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
            this.tabPage2.Size = new System.Drawing.Size(490, 295);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 388);
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
            ((System.ComponentModel.ISupportInitialize)(this.numTrailStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStopLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTradeOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbSymbol;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.ComboBox cbTif;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbBid;
        private System.Windows.Forms.TextBox tbAsk;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSell1;
        private System.Windows.Forms.Button btnBuy1;
        private System.Windows.Forms.CheckBox chkOutside;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnCancelAll;
        private System.Windows.Forms.Button btnCancelLast;
        private System.Windows.Forms.Button btnBuy1_8;
        private System.Windows.Forms.Button btnBuy1_4;
        private System.Windows.Forms.Button btnBuy1_2;
        private System.Windows.Forms.Button btnSell1_8;
        private System.Windows.Forms.Button btnSell1_2;
        private System.Windows.Forms.Button btnSell1_4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numTradeOffset;
        private System.Windows.Forms.CheckBox cbTrailStop;
        private System.Windows.Forms.CheckBox cbStopLoss;
        private System.Windows.Forms.NumericUpDown numTrailStop;
        private System.Windows.Forms.NumericUpDown numStopLoss;
        private System.Windows.Forms.Button btnClosePos;
        private System.Windows.Forms.Button btnStopLossAdj;
        private System.Windows.Forms.Button btnOptionsAnalysis;
    }
}


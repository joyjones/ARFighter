namespace SkyARFighter.Server
{
    partial class ServerForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lsvClients = new System.Windows.Forms.ListView();
            this.txbClientMsgs = new System.Windows.Forms.TextBox();
            this.bnDisconnect = new System.Windows.Forms.Button();
            this.txbServerLogs = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bnStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslClientCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerTick = new System.Windows.Forms.Timer(this.components);
            this.ctxMenuPeerLogs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearPeerLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ctxMenuPeerLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsvClients
            // 
            this.lsvClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvClients.FullRowSelect = true;
            this.lsvClients.HideSelection = false;
            this.lsvClients.Location = new System.Drawing.Point(0, 0);
            this.lsvClients.Name = "lsvClients";
            this.lsvClients.Size = new System.Drawing.Size(164, 204);
            this.lsvClients.TabIndex = 0;
            this.lsvClients.UseCompatibleStateImageBehavior = false;
            this.lsvClients.View = System.Windows.Forms.View.List;
            this.lsvClients.SelectedIndexChanged += new System.EventHandler(this.lsvClients_SelectedIndexChanged);
            // 
            // txbClientMsgs
            // 
            this.txbClientMsgs.ContextMenuStrip = this.ctxMenuPeerLogs;
            this.txbClientMsgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbClientMsgs.Location = new System.Drawing.Point(0, 0);
            this.txbClientMsgs.Multiline = true;
            this.txbClientMsgs.Name = "txbClientMsgs";
            this.txbClientMsgs.ReadOnly = true;
            this.txbClientMsgs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbClientMsgs.Size = new System.Drawing.Size(463, 236);
            this.txbClientMsgs.TabIndex = 1;
            this.txbClientMsgs.WordWrap = false;
            // 
            // bnDisconnect
            // 
            this.bnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bnDisconnect.Location = new System.Drawing.Point(3, 210);
            this.bnDisconnect.Name = "bnDisconnect";
            this.bnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.bnDisconnect.TabIndex = 3;
            this.bnDisconnect.Text = "断开连接";
            this.bnDisconnect.UseVisualStyleBackColor = true;
            // 
            // txbServerLogs
            // 
            this.txbServerLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbServerLogs.Location = new System.Drawing.Point(78, 17);
            this.txbServerLogs.Multiline = true;
            this.txbServerLogs.Name = "txbServerLogs";
            this.txbServerLogs.ReadOnly = true;
            this.txbServerLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbServerLogs.Size = new System.Drawing.Size(552, 83);
            this.txbServerLogs.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbServerLogs);
            this.groupBox1.Controls.Add(this.bnStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(633, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统日志";
            // 
            // bnStart
            // 
            this.bnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.bnStart.Location = new System.Drawing.Point(3, 17);
            this.bnStart.Name = "bnStart";
            this.bnStart.Size = new System.Drawing.Size(75, 83);
            this.bnStart.TabIndex = 3;
            this.bnStart.Text = "启动";
            this.bnStart.UseVisualStyleBackColor = true;
            this.bnStart.Click += new System.EventHandler(this.bnStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(4, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(633, 256);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "客户端列表";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txbClientMsgs);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(167, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(463, 236);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lsvClients);
            this.panel1.Controls.Add(this.bnDisconnect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 236);
            this.panel1.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslClientCount});
            this.statusStrip1.Location = new System.Drawing.Point(4, 363);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(633, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslClientCount
            // 
            this.tsslClientCount.Name = "tsslClientCount";
            this.tsslClientCount.Size = new System.Drawing.Size(70, 17);
            this.tsslClientCount.Text = "客户端数: 0";
            // 
            // timerTick
            // 
            this.timerTick.Interval = 500;
            this.timerTick.Tick += new System.EventHandler(this.timerTick_Tick);
            // 
            // ctxMenuPeerLogs
            // 
            this.ctxMenuPeerLogs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearPeerLogs});
            this.ctxMenuPeerLogs.Name = "ctxMenuPeerLogs";
            this.ctxMenuPeerLogs.Size = new System.Drawing.Size(181, 48);
            // 
            // tsmiClearPeerLogs
            // 
            this.tsmiClearPeerLogs.Name = "tsmiClearPeerLogs";
            this.tsmiClearPeerLogs.Size = new System.Drawing.Size(180, 22);
            this.tsmiClearPeerLogs.Text = "清空日志";
            this.tsmiClearPeerLogs.Click += new System.EventHandler(this.tsmiClearPeerLogs_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 389);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ServerForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "ARFighter PC服务器";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ctxMenuPeerLogs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lsvClients;
        private System.Windows.Forms.TextBox txbClientMsgs;
        private System.Windows.Forms.Button bnDisconnect;
        private System.Windows.Forms.TextBox txbServerLogs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslClientCount;
        private System.Windows.Forms.Button bnStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timerTick;
        private System.Windows.Forms.ContextMenuStrip ctxMenuPeerLogs;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearPeerLogs;
    }
}


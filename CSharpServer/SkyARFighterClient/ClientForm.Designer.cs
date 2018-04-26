namespace SkyARFighterClient
{
    partial class ClientForm
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
            this.bnConnectServer = new System.Windows.Forms.Button();
            this.txbServerIP = new System.Windows.Forms.TextBox();
            this.txbServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbNewMsg = new System.Windows.Forms.TextBox();
            this.bnSendMsg = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txbMessages = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bnDisconnect = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslConnectState = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bnConnectServer
            // 
            this.bnConnectServer.Location = new System.Drawing.Point(12, 39);
            this.bnConnectServer.Name = "bnConnectServer";
            this.bnConnectServer.Size = new System.Drawing.Size(93, 23);
            this.bnConnectServer.TabIndex = 0;
            this.bnConnectServer.Text = "连接服务器";
            this.bnConnectServer.UseVisualStyleBackColor = true;
            this.bnConnectServer.Click += new System.EventHandler(this.bnConnectServer_Click);
            // 
            // txbServerIP
            // 
            this.txbServerIP.Location = new System.Drawing.Point(12, 12);
            this.txbServerIP.Name = "txbServerIP";
            this.txbServerIP.Size = new System.Drawing.Size(142, 21);
            this.txbServerIP.TabIndex = 1;
            // 
            // txbServerPort
            // 
            this.txbServerPort.Location = new System.Drawing.Point(169, 12);
            this.txbServerPort.Name = "txbServerPort";
            this.txbServerPort.Size = new System.Drawing.Size(50, 21);
            this.txbServerPort.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = ":";
            // 
            // txbNewMsg
            // 
            this.txbNewMsg.Location = new System.Drawing.Point(12, 102);
            this.txbNewMsg.Name = "txbNewMsg";
            this.txbNewMsg.Size = new System.Drawing.Size(263, 21);
            this.txbNewMsg.TabIndex = 4;
            // 
            // bnSendMsg
            // 
            this.bnSendMsg.Location = new System.Drawing.Point(281, 100);
            this.bnSendMsg.Name = "bnSendMsg";
            this.bnSendMsg.Size = new System.Drawing.Size(37, 23);
            this.bnSendMsg.TabIndex = 5;
            this.bnSendMsg.Text = "发送";
            this.bnSendMsg.UseVisualStyleBackColor = true;
            this.bnSendMsg.Click += new System.EventHandler(this.bnSendMsg_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "发送消息:";
            // 
            // txbMessages
            // 
            this.txbMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txbMessages.Location = new System.Drawing.Point(12, 157);
            this.txbMessages.Multiline = true;
            this.txbMessages.Name = "txbMessages";
            this.txbMessages.ReadOnly = true;
            this.txbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbMessages.Size = new System.Drawing.Size(306, 132);
            this.txbMessages.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "接受消息:";
            // 
            // bnDisconnect
            // 
            this.bnDisconnect.Location = new System.Drawing.Point(111, 39);
            this.bnDisconnect.Name = "bnDisconnect";
            this.bnDisconnect.Size = new System.Drawing.Size(93, 23);
            this.bnDisconnect.TabIndex = 9;
            this.bnDisconnect.Text = "断开连接";
            this.bnDisconnect.UseVisualStyleBackColor = true;
            this.bnDisconnect.Click += new System.EventHandler(this.bnDisconnect_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnectState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 292);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(326, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslConnectState
            // 
            this.tsslConnectState.Name = "tsslConnectState";
            this.tsslConnectState.Size = new System.Drawing.Size(44, 17);
            this.tsslConnectState.Text = "未连接";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 314);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bnDisconnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbMessages);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bnSendMsg);
            this.Controls.Add(this.txbNewMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbServerPort);
            this.Controls.Add(this.txbServerIP);
            this.Controls.Add(this.bnConnectServer);
            this.Name = "ClientForm";
            this.Text = "SkyARFighter 模拟客户端";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnConnectServer;
        private System.Windows.Forms.TextBox txbServerIP;
        private System.Windows.Forms.TextBox txbServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbNewMsg;
        private System.Windows.Forms.Button bnSendMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbMessages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bnDisconnect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslConnectState;
    }
}


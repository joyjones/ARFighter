namespace SkyARFighter.Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.bnConnectServer = new System.Windows.Forms.Button();
            this.txbServerIP = new System.Windows.Forms.TextBox();
            this.txbServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbMessages = new System.Windows.Forms.TextBox();
            this.bnDisconnect = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslConnectState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsMenuItemCreateBox = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            // txbMessages
            // 
            this.txbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbMessages.Location = new System.Drawing.Point(3, 3);
            this.txbMessages.Multiline = true;
            this.txbMessages.Name = "txbMessages";
            this.txbMessages.ReadOnly = true;
            this.txbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbMessages.Size = new System.Drawing.Size(438, 229);
            this.txbMessages.TabIndex = 7;
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 393);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(476, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslConnectState
            // 
            this.tsslConnectState.Name = "tsslConnectState";
            this.tsslConnectState.Size = new System.Drawing.Size(44, 17);
            this.tsslConnectState.Text = "未连接";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 68);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(452, 322);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "场景";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txbMessages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 235);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(438, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 265);
            this.panel1.TabIndex = 1;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton2.Text = "发送消息";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMenuItemCreateBox});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(85, 22);
            this.toolStripDropDownButton1.Text = "创建物体";
            // 
            // tsMenuItemCreateBox
            // 
            this.tsMenuItemCreateBox.Name = "tsMenuItemCreateBox";
            this.tsMenuItemCreateBox.Size = new System.Drawing.Size(180, 22);
            this.tsMenuItemCreateBox.Text = "Box";
            this.tsMenuItemCreateBox.Click += new System.EventHandler(this.tsMenuItemCreateBox_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 415);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bnDisconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbServerPort);
            this.Controls.Add(this.txbServerIP);
            this.Controls.Add(this.bnConnectServer);
            this.Name = "ClientForm";
            this.Text = "SkyARFighter 模拟客户端";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnConnectServer;
        private System.Windows.Forms.TextBox txbServerIP;
        private System.Windows.Forms.TextBox txbServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbMessages;
        private System.Windows.Forms.Button bnDisconnect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslConnectState;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsMenuItemCreateBox;
    }
}


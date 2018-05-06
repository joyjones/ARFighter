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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnlScene = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbnSetupWorld = new System.Windows.Forms.ToolStripButton();
            this.tsbnCreateModel = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbnCreateModel_Box = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bnResetDeviceList = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.txbMessages.Size = new System.Drawing.Size(438, 290);
            this.txbMessages.TabIndex = 7;
            this.txbMessages.WordWrap = false;
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
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Location = new System.Drawing.Point(12, 68);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(452, 322);
            this.tabMain.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlScene);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "场景";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlScene
            // 
            this.pnlScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScene.Location = new System.Drawing.Point(3, 28);
            this.pnlScene.Name = "pnlScene";
            this.pnlScene.Size = new System.Drawing.Size(438, 265);
            this.pnlScene.TabIndex = 1;
            this.pnlScene.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlScene_Paint);
            this.pnlScene.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlScene_MouseDown);
            this.pnlScene.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlScene_MouseMove);
            this.pnlScene.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlScene_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnSetupWorld,
            this.tsbnCreateModel});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(438, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbnSetupWorld
            // 
            this.tsbnSetupWorld.Image = ((System.Drawing.Image)(resources.GetObject("tsbnSetupWorld.Image")));
            this.tsbnSetupWorld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbnSetupWorld.Name = "tsbnSetupWorld";
            this.tsbnSetupWorld.Size = new System.Drawing.Size(76, 22);
            this.tsbnSetupWorld.Text = "启动场景";
            this.tsbnSetupWorld.Click += new System.EventHandler(this.tsbnSetupWorld_Click);
            // 
            // tsbnCreateModel
            // 
            this.tsbnCreateModel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnCreateModel_Box});
            this.tsbnCreateModel.Enabled = false;
            this.tsbnCreateModel.Image = ((System.Drawing.Image)(resources.GetObject("tsbnCreateModel.Image")));
            this.tsbnCreateModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbnCreateModel.Name = "tsbnCreateModel";
            this.tsbnCreateModel.Size = new System.Drawing.Size(85, 22);
            this.tsbnCreateModel.Text = "创建物体";
            // 
            // tsbnCreateModel_Box
            // 
            this.tsbnCreateModel_Box.Name = "tsbnCreateModel_Box";
            this.tsbnCreateModel_Box.Size = new System.Drawing.Size(98, 22);
            this.tsbnCreateModel_Box.Text = "Box";
            this.tsbnCreateModel_Box.Click += new System.EventHandler(this.tsMenuItemCreateBox_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txbMessages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bnResetDeviceList
            // 
            this.bnResetDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnResetDeviceList.Location = new System.Drawing.Point(371, 10);
            this.bnResetDeviceList.Name = "bnResetDeviceList";
            this.bnResetDeviceList.Size = new System.Drawing.Size(93, 23);
            this.bnResetDeviceList.TabIndex = 12;
            this.bnResetDeviceList.Text = "重置设备列表";
            this.bnResetDeviceList.UseVisualStyleBackColor = true;
            this.bnResetDeviceList.Click += new System.EventHandler(this.bnResetDeviceList_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 415);
            this.Controls.Add(this.bnResetDeviceList);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bnDisconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbServerPort);
            this.Controls.Add(this.txbServerIP);
            this.Controls.Add(this.bnConnectServer);
            this.Name = "ClientForm";
            this.Text = "SkyARFighter 模拟客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pnlScene;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbnSetupWorld;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripDropDownButton tsbnCreateModel;
        private System.Windows.Forms.ToolStripMenuItem tsbnCreateModel_Box;
        private System.Windows.Forms.Button bnResetDeviceList;
    }
}


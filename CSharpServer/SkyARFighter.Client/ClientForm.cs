using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            txbServerIP.Text = "127.0.0.1";
            txbServerPort.Text = "8333";
            bnDisconnect.Enabled = false;
            tabMain.Enabled = false;
            Program.Client.LogAppended += Client_LogAppended;
            Program.Client.Disconnected += Client_Disconnected;
            Program.Client.StateChanged += Client_StateChanged;
        }

        private void Client_StateChanged(SimulateClient.States state)
        {
            BeginInvoke(new Action(() =>
            {
                switch (state)
                {
                    case SimulateClient.States.Offline:
                        txbServerIP.Enabled = true;
                        txbServerPort.Enabled = true;
                        bnConnectServer.Enabled = true;
                        bnDisconnect.Enabled = false;
                        tabMain.SelectedIndex = 0;
                        tabMain.Enabled = false;
                        tsbnSetupWorld.Enabled = true;
                        tsbnCreateModel.Enabled = false;
                        txbMessages.Text = "";
                        tsslConnectState.Text = "未连接";
                        break;
                    case SimulateClient.States.Connected:
                        txbServerIP.Enabled = false;
                        txbServerPort.Enabled = false;
                        bnConnectServer.Enabled = false;
                        bnDisconnect.Enabled = true;
                        tabMain.Enabled = true;
                        tsbnSetupWorld.Enabled = true;
                        tsbnCreateModel.Enabled = false;
                        tsslConnectState.Text = "已连接";
                        break;
                    case SimulateClient.States.InScene:
                    case SimulateClient.States.SceneEdit:
                        tsbnSetupWorld.Enabled = false;
                        tsbnCreateModel.Enabled = true;
                        break;
                }
            }));
        }

        private void Client_Disconnected()
        {
            BeginInvoke(new Action(() =>
            {
            }));
        }

        private void Client_LogAppended(string msg)
        {
            BeginInvoke(new Action(() =>
            {
                txbMessages.Text += msg + "\r\n";
            }));
        }

        private void bnConnectServer_Click(object sender, EventArgs e)
        {
            if (Program.Client.Connect(txbServerIP.Text, int.Parse(txbServerPort.Text)))
            {
            }
        }

        private void bnDisconnect_Click(object sender, EventArgs e)
        {
            Program.Client.Disconnect();
        }
        
        private void tsMenuItemCreateBox_Click(object sender, EventArgs e)
        {
            var pos = new Vector3(1, 2, 3);
            var scale = new Vector3(0.1f, 0.1f, 0.1f);
            var rotate = new Vector4(0, 0, 0, 0);
            Program.Client.Server_CreateObject(SceneModelType.标注_圆点, pos, scale, rotate);
        }

        private void tsbnSetupWorld_Click(object sender, EventArgs e)
        {
            Program.Client.Server_SetupWorld("test01");
        }
    }
}

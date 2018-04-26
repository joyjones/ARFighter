using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyARFighterClient
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
            txbServerPort.Text = "8765";
            Program.Client.ReceivedMessage += Client_ReceivedMessage;
        }

        private void Client_ReceivedMessage(string msg)
        {
            BeginInvoke(new Action(() =>
            {
                txbMessages.Text += msg + "\r\n";
            }));
        }

        private void bnConnectServer_Click(object sender, EventArgs e)
        {
            if (Program.Client.Connect(txbServerIP.Text, int.Parse(txbServerPort.Text)))
                tsslConnectState.Text = "已连接";
        }

        private void bnDisconnect_Click(object sender, EventArgs e)
        {
            Program.Client.Disconnect();
            tsslConnectState.Text = "未连接";
        }

        private void bnSendMsg_Click(object sender, EventArgs e)
        {
            if (txbNewMsg.Text.Length == 0)
                return;
            Program.Client.SendMessage(txbNewMsg.Text);
        }

    }
}

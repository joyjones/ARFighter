using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyARFighterServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            Program.Server.LogAppended += Server_LogAppended;
            Program.Server.ClientConnectionChanged += Server_ClientConnectionChanged;
            Program.Server.ReceivedClientMessage += Server_ReceivedClientMessage;
        }

        public Socket SelectedClient
        {
            get; private set;
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            if (SelectedClient != null)
                txbClientMsgs.Text = clientMessages[SelectedClient];
        }

        private void Server_ClientConnectionChanged(Socket client, bool connected)
        {
            if (!connected)
                clientMessages.Remove(client);
            else if (!clientMessages.ContainsKey(client))
                clientMessages[client] = "";

            BeginInvoke(new Action(() =>
            {
                lsvClients.Items.Clear();
                foreach (var c in Program.Server.Clients)
                {
                    var lvi = new ListViewItem(c.RemoteEndPoint.ToString());
                    lvi.Tag = c;
                    if (c == SelectedClient)
                        lvi.Selected = true;
                    lsvClients.Items.Add(lvi);
                }
                if (lsvClients.Items.Count > 0 && SelectedClient == null)
                {
                    lsvClients.Items[0].Selected = true;
                }
            }));
        }

        private void Server_ReceivedClientMessage(Socket client, string msg)
        {
            clientMessages[client] += msg + "\r\n";
        }

        private void Server_LogAppended(string msg)
        {
            BeginInvoke(new Action(() =>
            {
                txbServerLogs.Text += msg + "\r\n";
            }));
        }

        private void lsvClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvClients.SelectedItems.Count == 0)
            {
                SelectedClient = null;
                txbClientMsgs.Text = "";
            }
            else
            {
                SelectedClient = lsvClients.SelectedItems[0].Tag as Socket;
                txbClientMsgs.Text = clientMessages[SelectedClient];
            }
        }

        private void bnStart_Click(object sender, EventArgs e)
        {
            if (Program.Server.Running)
            {
                Program.Server.Stop();
                timerTick.Enabled = false;
                bnStart.Text = "启动";
            }
            else
            {
                Program.Server.Start();
                timerTick.Enabled = true;
                bnStart.Text = "停止";
            }
        }

        private void bnSendMsg_Click(object sender, EventArgs e)
        {
            if (SelectedClient == null || txbNewMsg.Text.Length == 0)
                return;
            Program.Server.SendMessage(SelectedClient, txbNewMsg.Text);
        }

        private Dictionary<Socket, string> clientMessages = new Dictionary<Socket, string>();

    }
}

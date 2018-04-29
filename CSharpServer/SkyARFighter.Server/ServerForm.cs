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

namespace SkyARFighter.Server
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
            //Program.Server.ReceivedClientMessage += Server_ReceivedClientMessage;
            bnStart_Click(null, null);
        }

        public Player SelectedPlayer
        {
            get; private set;
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            //if (SelectedPlayer != null)
            //    txbClientMsgs.Text = clientMessages[SelectedPlayer];
        }

        private void Server_ClientConnectionChanged(Player player, bool connected)
        {
            if (connected)
            {
                player.SendMessage("你好啊客户端，我是服务器~");
                    //Server_SentClientMessage(player, msg);
            }

            BeginInvoke(new Action(() =>
            {
                lsvClients.Items.Clear();
                foreach (var c in Program.Server.Players)
                {
                    var lvi = new ListViewItem(c.UsingSocket.RemoteEndPoint.ToString());
                    lvi.Tag = c;
                    if (c == SelectedPlayer)
                        lvi.Selected = true;
                    lsvClients.Items.Add(lvi);
                }
                if (lsvClients.Items.Count > 0 && SelectedPlayer == null)
                {
                    lsvClients.Items[0].Selected = true;
                }
                tsslClientCount.Text = "客户端数: " + lsvClients.Items.Count;
            }));
        }

        private void Server_ReceivedClientMessage(Socket client, string msg)
        {
            clientMessages[client] += "=> " + msg + "\r\n";
        }

        private void Server_SentClientMessage(Socket client, string msg)
        {
            clientMessages[client] += "<= " + msg + "\r\n";
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
                SelectedPlayer = null;
                txbClientMsgs.Text = "";
            }
            else
            {
                SelectedPlayer = lsvClients.SelectedItems[0].Tag as Player;
                //txbClientMsgs.Text = clientMessages[SelectedPlayer];
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
            if (SelectedPlayer == null || txbNewMsg.Text.Length == 0)
                return;
            SelectedPlayer.SendMessage(txbNewMsg.Text);
            txbNewMsg.Text = "";
        }

        private Dictionary<Socket, string> clientMessages = new Dictionary<Socket, string>();

    }
}

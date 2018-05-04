using SkyARFighter.Server.Network;
using SkyARFighter.Server.Structures;
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
            Program.Server.ClientConnected += Server_ClientConnected;
            bnStart_Click(null, null);
            Program.Server.Log(null);
        }

        private PlayerPeer selectedPeer = null;
        public PlayerPeer SelectedPeer
        {
            get => selectedPeer;
            set
            {
                if (selectedPeer != value)
                {
                    selectedPeer = value;
                    if (selectedPeer == null)
                        txbClientMsgs.Text = "";
                    else
                        txbClientMsgs.Text = string.Join("\r\n", selectedPeer.Logs);
                    foreach (ListViewItem i in lsvClients.Items)
                    {
                        if (i.Tag == selectedPeer)
                        {
                            i.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            //if (SelectedPlayer != null)
            //    txbClientMsgs.Text = clientMessages[SelectedPlayer];
        }

        private void Server_ClientConnected(PlayerPeer peer)
        {
            peer.Disconnected += Peer_Disconnected;
            peer.LogAppended += Peer_LogAppended;
            BeginInvoke(new Action(() =>
            {
                var lvi = new ListViewItem(peer.ToString()) { Tag = peer };
                lsvClients.Items.Add(lvi);
                if (SelectedPeer == null)
                    SelectedPeer = peer;
                tsslClientCount.Text = "客户端数: " + lsvClients.Items.Count;
            }));
        }

        private void Peer_LogAppended(PlayerPeer peer, string arg2)
        {
            if (peer != SelectedPeer)
                return;
            BeginInvoke(new Action(() =>
            {
                txbClientMsgs.Text = string.Join("\r\n", peer.Logs);
            }));
        }

        private void Peer_Disconnected(PlayerPeer peer)
        {
            BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < lsvClients.Items.Count; ++i)
                {
                    if (lsvClients.Items[i].Tag == peer)
                    {
                        lsvClients.Items.RemoveAt(i);
                        if (SelectedPeer == peer)
                            SelectedPeer = null;
                        break;
                    }
                }
                txbServerLogs.Text += $"客户端{peer.ToString()}断开连接。\r\n";
                tsslClientCount.Text = "客户端数: " + lsvClients.Items.Count;
            }));
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
                SelectedPeer = null;
            else
                SelectedPeer = lsvClients.SelectedItems[0].Tag as PlayerPeer;
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
    }
}

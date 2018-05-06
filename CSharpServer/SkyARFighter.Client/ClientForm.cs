using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
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
            Text = "SkyARFighter 模拟客户端 - " + Program.Client.UniqueDeviceId;
            Program.Client.LogAppended += Client_LogAppended;
            Program.Client.Disconnected += Client_Disconnected;
            Program.Client.StateChanged += Client_StateChanged;
            Program.Client.SceneContentChanged += Client_SceneContentChanged;
        }

        private void Client_SceneContentChanged()
        {
            BeginInvoke(new Action(() =>
            {
                pnlScene.Refresh();
            }));
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
                        bnResetDeviceList.Enabled = true;
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
                        bnResetDeviceList.Enabled = false;
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
            var info = new SceneModelInfo();
            info.ModelId = 1;
            info.PosX = (float)CommonMethods.Rander.NextDouble() * (CommonMethods.Rander.Next(2) == 0 ? 1 : -1);
            info.PosY = (float)CommonMethods.Rander.NextDouble() * (CommonMethods.Rander.Next(2) == 0 ? 1 : -1);
            info.PosZ = (float)CommonMethods.Rander.NextDouble() * (CommonMethods.Rander.Next(2) == 0 ? 1 : -1);
            info.ScaleX = info.ScaleY = info.ScaleZ = 0.1f;
            info.RotationX = info.RotationY = info.RotationZ = 0;
            info.CreatePlayerId = Program.Client.Info.Id;
            Program.Client.CreateSceneModel(info);
        }

        private void tsbnSetupWorld_Click(object sender, EventArgs e)
        {
            Program.Client.Server_SetupWorld("test02");
        }

        private const float MetersPerPixel = 0.01f;
        private Dictionary<SceneModelInfo, RectangleF> modelRects = new Dictionary<SceneModelInfo, RectangleF>();
        private SceneModelInfo selectedModel = null;
        private Point? dragOffset = null;
        private Font tipFont = new Font(new FontFamily("微软雅黑"), 8);
        private void pnlScene_Paint(object sender, PaintEventArgs e)
        {
            Point ptOrigin = new Point(pnlScene.Width / 2, pnlScene.Height / 2);
            e.Graphics.DrawLine(Pens.LightGray, new Point(0, ptOrigin.Y), new Point(pnlScene.Width, ptOrigin.Y));
            e.Graphics.DrawLine(Pens.LightGray, new Point(ptOrigin.X, 0), new Point(ptOrigin.X, pnlScene.Height));

            var scene = Program.Client.Scene;
            if (scene == null)
                return;
            modelRects.Clear();
            foreach (var model in scene.Models)
            {
                var pos = new PointF(
                    ptOrigin.X + model.PosX / MetersPerPixel,
                    ptOrigin.Y + model.PosZ / MetersPerPixel
                );
                var size = 10.0f;
                var rc = new RectangleF(pos.X - size * 0.5f, pos.Y - size * 0.5f, size, size);
                e.Graphics.FillRectangle(Brushes.OrangeRed, rc);
                string tip = $"[{model.PosX},{model.PosZ}]";
                var tipSize = e.Graphics.MeasureString(tip, tipFont);
                var tipPos = new Point((int)(pos.X - tipSize.Width / 2), (int)rc.Bottom);
                e.Graphics.DrawString(tip, tipFont, Brushes.Black, tipPos);
                modelRects[model] = rc;
            }
        }

        private void pnlScene_MouseDown(object sender, MouseEventArgs e)
        {
            selectedModel = null;
            dragOffset = null;
            foreach (var mr in modelRects)
            {
                var rc = mr.Value;
                if (rc.Contains(e.X, e.Y))
                {
                    selectedModel = mr.Key;
                    var center = new PointF(rc.X + rc.Width * 0.5f, rc.Y + rc.Height * 0.5f);
                    dragOffset = new Point(e.X - (int)center.X, e.Y - (int)center.Y);
                    break;
                }
            }
        }

        private void pnlScene_MouseUp(object sender, MouseEventArgs e)
        {
            dragOffset = null;
        }

        private void pnlScene_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptOrigin = new Point(pnlScene.Width / 2, pnlScene.Height / 2);
            if (e.Button == MouseButtons.Left)
            {
                if (selectedModel != null)
                {
                    var centerNew = new Point(e.X - dragOffset.Value.X, e.Y - dragOffset.Value.Y);
                    selectedModel.PosX = (centerNew.X - ptOrigin.X) * MetersPerPixel;
                    selectedModel.PosZ = (centerNew.Y - ptOrigin.Y) * MetersPerPixel;

                    Program.Client.Server_MoveSceneModel(selectedModel.Id, new Vector3(selectedModel.PosX, selectedModel.PosY, selectedModel.PosZ));
                    pnlScene.Refresh();
                }
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Client.ReleaseDeviceId();
            Program.Client.Disconnect();
        }

        private void bnResetDeviceList_Click(object sender, EventArgs e)
        {
            Program.Client.GenerateDeviceList();
            Program.Client.RequireDeviceId();
            Text = "SkyARFighter 模拟客户端 - " + Program.Client.UniqueDeviceId;
        }
    }
}

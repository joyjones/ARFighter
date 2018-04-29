using Newtonsoft.Json;
using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        public SimulateClient()
        {
            cameraTransform = new Matrix();
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = false;
            timer.Elapsed += Tick;
        }

        public bool Connected
        {
            get => socket != null && socket.Connected;
        }

        public bool Connect(string ipAddress, int port)
        {
            if (Connected)
                return true;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipAddress, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接失败");
                return false;
            }
            timer.Enabled = true;
            BeginReceive();
            return true;
        }

        public void Disconnect()
        {
            if (Connected)
            {
                timer.Enabled = false;
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
            }
        }

        public bool SendMessage(string msg)
        {
            if (!Connected)
                return false;

            var bytes = Encoding.Unicode.GetBytes(msg);
            var header = BitConverter.GetBytes(bytes.Length);
            var data = new byte[header.Length + bytes.Length];
            Array.Copy(header, data, header.Length);
            Array.Copy(bytes, 0, data, 4, bytes.Length);
            return socket.Send(data) > 0;
        }

        private void BeginReceive()
        {
            var buffer = new byte[1024];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback((ar) =>
            {
                int length = 0;
                try
                {
                    length = socket.EndReceive(ar);
                }
                catch { return; }

                int len = BitConverter.ToInt32(buffer, 0);
                var message = Encoding.Unicode.GetString(buffer, 4, len);
                //ReceivedMessage?.Invoke(message);

                BeginReceive();
            }), null);
        }

        private void InvokeRemoteMethod(RemotingMethodId type, object argObj)
        {
            var data = BitConverter.GetBytes((int)type);
            var json = JsonConvert.SerializeObject(argObj);
            var context = Encoding.UTF8.GetBytes(json);
            Array.Resize(ref data, data.Length + context.Length);
            Array.Copy(context, 0, data, 4, context.Length);
            socket.Send(data);
        }
        
        public void Tick(object sender, ElapsedEventArgs e)
        {
            var ts = new TimeSpan(e.SignalTime.Ticks - lastElapsedTick);
            if (ts.TotalMilliseconds >= 100)
            {
                timer.Enabled = false;
                Server_SyncCamera();
                lastElapsedTick = e.SignalTime.Ticks;
            }
        }

        private Socket socket = null;
        private long lastElapsedTick;
        private Matrix cameraTransform;
        private System.Timers.Timer timer;
    }
}

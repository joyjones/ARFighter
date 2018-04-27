using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public class SimulateClient
    {
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
            BeginReceive();
            return true;
        }

        public void Disconnect()
        {
            if (Connected)
            {
                socket.Disconnect(false);
                socket.Close();
                socket = null;
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
                ReceivedMessage?.Invoke(message);

                BeginReceive();
            }), null);
        }

        private Socket socket = null;
        public event Action<string> ReceivedMessage;
    }
}

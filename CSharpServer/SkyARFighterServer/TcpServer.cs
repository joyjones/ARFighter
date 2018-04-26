using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyARFighterServer
{
    public class TcpServer
    {
        public bool Running
        {
            get => startedUp.WaitOne(0);
        }

        public IEnumerable<Socket> Clients
        {
            get => clients;
        }

        public void Start()
        {
            if (Running)
                return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                Thread.CurrentThread.Name = "连接监听线程";
                using (serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    var point = new IPEndPoint(IPAddress.Any, 8765);
                    serverSocket.Bind(point);
                    serverSocket.Listen(10);
                    startedUp.Set();
                    LogAppended?.Invoke("服务器启动成功。");

                    Listen();

                    while (startedUp.WaitOne(50));

                    if (serverSocket.Connected)
                        serverSocket.Shutdown(SocketShutdown.Both);
                }
            }), null);
        }

        public void Stop()
        {
            if (!Running)
                return;
            startedUp.Reset();
            clients.Clear();
            LogAppended?.Invoke("服务器已停止。");
        }

        public void SendMessage(Socket client, string msg)
        {
            if (Running && client.Connected)
                client.Send(Encoding.Unicode.GetBytes(msg));
        }

        private void Listen()
        {
            serverSocket.BeginAccept(new AsyncCallback(ar =>
            {
                Socket client = null;
                try
                {
                    client = serverSocket.EndAccept(ar);
                    clients.Add(client);
                }
                catch
                {
                    //clients.Remove(client);
                    //ClientConnectionChanged?.Invoke(client, false);
                    return;
                }
                connectedNewClient.Set();
                SendMessage(client, "你好啊客户端，我是服务器~");

                ClientConnectionChanged?.Invoke(client, true);
                LogAppended?.Invoke($"客户端[{client.RemoteEndPoint}] 连接成功。");

                ReceiveMessage(client);
                Listen();
            }), null);
        }

        private void ReceiveMessage(Socket client)
        {
            var buffer = new byte[1024];
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback((ar) =>
            {
                int length = 0;
                try
                {
                    length = client.EndReceive(ar);
                }
                catch { return; }
                var message = Encoding.Unicode.GetString(buffer, 0, length);
                if (!string.IsNullOrEmpty(message))
                    ReceivedClientMessage?.Invoke(client, message);
                ReceiveMessage(client);
            }), null);
        }

        private ManualResetEvent startedUp = new ManualResetEvent(false);
        private AutoResetEvent connectedNewClient = new AutoResetEvent(false);
        private Socket serverSocket;
        private List<Socket> clients = new List<Socket>();

        public event Action<string> LogAppended;
        public event Action<Socket, bool> ClientConnectionChanged;
        public event Action<Socket, string> ReceivedClientMessage;
    }
}

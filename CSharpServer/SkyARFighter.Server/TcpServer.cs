using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyARFighter.Server
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
                    var point = new IPEndPoint(IPAddress.Any, 8333);
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

        public bool SendMessage(Socket client, string msg)
        {
            if (Running && client.Connected)
            {
                var bytes = Encoding.Unicode.GetBytes(msg);
                var header = BitConverter.GetBytes(bytes.Length);
                var data = new byte[header.Length + bytes.Length];
                Array.Copy(header, data, header.Length);
                Array.Copy(bytes, 0, data, 4, bytes.Length);
                client.Send(data);
                return true;
            }
            return false;
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

                if (length > 0)
                {
                    int readLen = 0, offset = 0;
                    if (receivingPartialDataLength > 0)
                    {
                        readLen = receivingPartialDataLength;
                        receivingPartialDataLength = -1;
                    }
                    else
                    {
                        readLen = BitConverter.ToInt32(buffer, 0);
                        if (length == 4)
                        {
                            receivingPartialDataLength = readLen;
                            readLen = 0;
                        }
                        else
                        {
                            offset = 4;
                        }
                    }
                    if (readLen > 0)
                    {
                        var str = Encoding.UTF8.GetString(buffer, offset, readLen);
                        var msg = JsonConvert.DeserializeObject(str).ToString();
                        if (!string.IsNullOrEmpty(msg))
                            ReceivedClientMessage?.Invoke(client, msg);
                    }
                }
                ReceiveMessage(client);
            }), null);
        }

        private ManualResetEvent startedUp = new ManualResetEvent(false);
        private AutoResetEvent connectedNewClient = new AutoResetEvent(false);
        private Socket serverSocket;
        private List<Socket> clients = new List<Socket>();
        private int receivingPartialDataLength = -1;

        public event Action<string> LogAppended;
        public event Action<Socket, bool> ClientConnectionChanged;
        public event Action<Socket, string> ReceivedClientMessage;
    }
}

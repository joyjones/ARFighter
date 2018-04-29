using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyARFighter.Server
{
    public class TcpServer
    {
        static TcpServer()
        {
            foreach (var mi in typeof(Player).GetMethods())
            {
                if (mi.GetCustomAttribute(typeof(RemotingMethodAttribute)) is RemotingMethodAttribute attr)
                {
                    MsgHandlers[attr.MethodId] = mi;
                }
            }
        }

        public TcpServer()
        {
        }

        public bool Running
        {
            get => startedUp.WaitOne(0);
        }

        public IEnumerable<Player> Players
        {
            get => players;
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
            players.Clear();
            LogAppended?.Invoke("服务器已停止。");
        }

        private void Listen()
        {
            serverSocket.BeginAccept(new AsyncCallback(ar =>
            {
                Player player = null;
                try
                {
                    var sock = serverSocket.EndAccept(ar);
                    player = new Player(sock);
                    players.Add(player);
                }
                catch
                {
                    return;
                }
                connectedNewClient.Set();

                ClientConnectionChanged?.Invoke(player, true);
                LogAppended?.Invoke($"客户端[{player.UsingSocket.RemoteEndPoint}] 连接成功。");

                ReceiveMessage(player);
                Listen();
            }), null);
        }

        private void ReceiveMessage(Player player)
        {
            var buffer = new byte[1024];
            player.UsingSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback((ar) =>
            {
                int length = 0;
                try
                {
                    length = player.UsingSocket.EndReceive(ar);
                }
                catch { return; }

                if (length > 0)
                {
                    int msgType = BitConverter.ToInt32(buffer, 0);
                    if (MsgHandlers.TryGetValue(msgType, out MethodInfo mi))
                    {
                        var str = Encoding.UTF8.GetString(buffer, 4, length - 4);
                        var args = JsonHelper.ParseMethodParameters(mi, str);
                        mi.Invoke(player, args);
                    }
                    ReceiveMessage(player);
                }
                else
                {
                    players.Remove(player);
                    LogAppended?.Invoke($"客户端[{player.UsingSocket.RemoteEndPoint}] 断开连接。");
                    ClientConnectionChanged?.Invoke(player, false);
                }
            }), null);
        }
        
        private ManualResetEvent startedUp = new ManualResetEvent(false);
        private AutoResetEvent connectedNewClient = new AutoResetEvent(false);
        private Socket serverSocket;
        private List<Player> players = new List<Player>();

        public event Action<string> LogAppended;
        public event Action<Player, bool> ClientConnectionChanged;
        public event Action<Player, string> ReceivedClientMessage;

        public static Dictionary<int, MethodInfo> MsgHandlers = new Dictionary<int, MethodInfo>();
    }
}

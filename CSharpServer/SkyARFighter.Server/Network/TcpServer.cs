using SkyARFighter.Common;
using SkyARFighter.Server.Network;
using SkyARFighter.Server.Structures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace SkyARFighter.Server.Network
{
    public class TcpServer
    {
        public TcpServer()
        {
        }

        public bool Running
        {
            get => startedUp.WaitOne(0);
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

                    BeginListen();

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
            LogAppended?.Invoke("服务器已停止。");
        }

        public void Log(string msg)
        {
            if (LogAppended == null)
                stackedLogs.Enqueue(msg);
            else
            {
                while (stackedLogs.Count > 0)
                    LogAppended?.Invoke(stackedLogs.Dequeue());
            }
            if (!string.IsNullOrEmpty(msg))
                LogAppended?.Invoke(msg);
        }

        private void BeginListen()
        {
            serverSocket.BeginAccept(new AsyncCallback(ar =>
            {
                try
                {
                    var sock = serverSocket.EndAccept(ar);
                    var peer = CreatePeerInstance(sock);

                    connectedNewClient.Set();

                    ClientConnected?.Invoke(peer);
                    LogAppended?.Invoke($"客户端[{sock.RemoteEndPoint}] 连接成功。");
                }
                catch
                {
                    return;
                }

                BeginListen();
            }), null);
        }

        private PlayerPeer CreatePeerInstance(Socket socket)
        {
            return new PlayerPeer(Program.Game, socket);
        }

        private ManualResetEvent startedUp = new ManualResetEvent(false);
        private AutoResetEvent connectedNewClient = new AutoResetEvent(false);
        private Socket serverSocket;
        private Queue<string> stackedLogs = new Queue<string>();

        public event Action<string> LogAppended;
        public event Action<PlayerPeer> ClientConnected;

    }
}

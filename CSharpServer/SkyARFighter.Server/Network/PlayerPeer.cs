using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Server.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Network
{
    public class PlayerPeer
    {
        static PlayerPeer()
        {
            foreach (var mi in typeof(Player).GetMethods())
            {
                if (mi.GetCustomAttribute(typeof(RemotingMethodAttribute)) is RemotingMethodAttribute attr)
                {
                    MsgHandlers[attr.MethodId] = mi;
                }
            }
        }

        public PlayerPeer(Socket sock)
        {
            socket = sock;
            LogAppended += OnLogAppended;
            ReceiveMessage();
        }

        private void OnLogAppended(PlayerPeer self, string msg)
        {
            logs.Add(msg);
        }

        public Player HostPlayer
        {
            get; set;
        }

        public string IPEndPoint
        {
            get => socket.RemoteEndPoint.ToString();
        }

        public string[] Logs => logs.ToArray();

        private Socket socket = null;
        private ManualResetEvent waitingNextMessage = new ManualResetEvent(false);

        private void ReceiveMessage()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                Thread.CurrentThread.Name = "客户端通信处理线程";
                var buffer = new byte[4096];
                while (socket.Connected)
                {
                    waitingNextMessage.Reset();

                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ar =>
                    {
                        try
                        {
                            if (socket.EndReceive(ar) == 0)
                                throw new Exception();
                        }
                        catch
                        {
                            LogAppended?.Invoke(this, "断开连接");
                            Disconnected?.Invoke(this);
                            return;
                        }

                        var msgType = (RemotingMethodId)BitConverter.ToInt32(buffer, 0);
                        int length = BitConverter.ToInt32(buffer, 4);
                        if (length > 0)
                        {
                            if (!MsgHandlers.TryGetValue(msgType, out MethodInfo mi))
                                LogAppended?.Invoke(this, "收到未注册的的远程方法调用请求：" + msgType);
                            else
                            {
                                try
                                {
                                    var context = buffer.Skip(8).Take(length).ToArray();
                                    var str = Encoding.UTF8.GetString(context, 0, context.Length);
                                    var args = JsonHelper.ParseMethodParameters(mi, str);
                                    LogAppended?.Invoke(this, $"调用远程方法：{mi.Name}, 参数：{str}");

                                    mi.Invoke(HostPlayer, args);
                                }
                                catch (Exception ex)
                                {
                                    LogAppended?.Invoke(this, "远程方法调用异常：" + ex.Message);
                                }
                            }
                        }

                        waitingNextMessage.Set();
                    }), null);

                    while (socket.Connected && !waitingNextMessage.WaitOne(50)) ;
                }
            }));
        }

        public bool SendMessage(RemotingMethodId methodId, object args)
        {
            if (!socket.Connected)
                return false;

            using (var sw = new System.IO.MemoryStream())
            {
                var bsType = BitConverter.GetBytes((int)methodId);
                var json = JsonConvert.SerializeObject(args);
                var bsCtx = Encoding.UTF8.GetBytes(json);

                sw.Write(bsType, 0, bsType.Length);
                var bsLen = BitConverter.GetBytes(bsCtx.Length);
                sw.Write(bsLen, 0, bsLen.Length);
                sw.Write(bsCtx, 0, bsCtx.Length);
                var bsSend = sw.GetBuffer();
                socket.Send(bsSend);

                LogAppended?.Invoke(this, $"调用客户端方法：{methodId}, 参数：{json}");
            }
            return true;
        }
        public override string ToString()
        {
            if (socket != null)
            {
                return socket.RemoteEndPoint.ToString();
            }
            return base.ToString();
        }

        public event Action<PlayerPeer, string> LogAppended;
        public event Action<PlayerPeer> Disconnected;
        private List<string> logs = new List<string>();
        public static Dictionary<RemotingMethodId, MethodInfo> MsgHandlers = new Dictionary<RemotingMethodId, MethodInfo>();
    }
}

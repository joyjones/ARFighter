using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Server.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
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
            BeginReceiveMessage();
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
        private void BeginReceiveMessage()
        {
            var buffer = new byte[1024];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback((ar) =>
            {
                int length = 0;
                try
                {
                    length = socket.EndReceive(ar);
                }
                catch
                {
                    LogAppended?.Invoke(this, "断开连接");
                    Disconnected?.Invoke(this);
                    return;
                }

                if (length > 0)
                {
                    try
                    {
                        var msgType = (RemotingMethodId)BitConverter.ToInt32(buffer, 0);
                        if (MsgHandlers.TryGetValue(msgType, out MethodInfo mi))
                        {
                            var str = Encoding.UTF8.GetString(buffer, 4, length - 4);
                            var args = JsonHelper.ParseMethodParameters(mi, str);
                            LogAppended?.Invoke(this, $"调用远程方法：{mi.Name}, 参数：{str}");

                            mi.Invoke(HostPlayer, args);
                        }
                        else
                        {
                            LogAppended?.Invoke(this, "收到未注册的的远程方法调用请求：" + msgType);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogAppended?.Invoke(this, "远程方法调用异常：" + ex.Message);
                    }
                    BeginReceiveMessage();
                }
                else
                {
                    LogAppended?.Invoke(this, "断开连接");
                    Disconnected?.Invoke(this);
                }
            }), null);
        }

        public bool SendMessage(RemotingMethodId methodId, object args)
        {
            if (!socket.Connected)
                return false;
            var data = BitConverter.GetBytes((int)methodId);
            var json = JsonConvert.SerializeObject(args);
            var context = Encoding.UTF8.GetBytes(json);
            Array.Resize(ref data, data.Length + context.Length);
            Array.Copy(context, 0, data, 4, context.Length);
            socket.Send(data);

            LogAppended?.Invoke(this, $"调用客户端方法：{methodId}, 参数：{json}");
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

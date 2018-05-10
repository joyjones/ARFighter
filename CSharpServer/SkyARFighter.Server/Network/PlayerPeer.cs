using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.Network;
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
    public partial class PlayerPeer
    {
        static PlayerPeer()
        {
            MsgHandlers = RemotingMethodAttribute.GetTypeMethodsMapping(typeof(Player), typeof(PlayerPeer));
        }

        public PlayerPeer(Game game, Socket sock)
        {
            HostGame = game;
            socket = sock;
            LogAppended += OnLogAppended;
            MsgProc = new Communication(MsgHandlers);
            MsgProc.StartupThread("接收端通信处理线程",  socket, 
                (msg) => LogAppended?.Invoke(this, msg), () => Disconnected?.Invoke(this));
        }

        private void OnLogAppended(PlayerPeer self, string msg)
        {
            logs.Add(msg);
        }

        public Player HostPlayer
        {
            get; set;
        }

        public Game HostGame
        {
            get; private set;
        }

        public string IPEndPoint
        {
            get => socket.RemoteEndPoint.ToString();
        }

        public string[] Logs => logs.ToArray();
        public Communication MsgProc { get; private set; }

        private Socket socket = null;

        public void SendMessage(RemotingMethodId methodId, object args)
        {
            MsgProc.SendMessage(socket, methodId, args, (msg) => LogAppended?.Invoke(this, msg));
        }

        public void ProcessMessages()
        {
            foreach (var msg in MsgProc.Messages)
            {
                //var content = ZipHelper.GZipDecompressString(msg.args);
                try
                {
                    if (MsgHandlers.TryGetValue((int)msg.code, out MethodInfo method))
                    {
                        var args = JsonHelper.ParseMethodParameters(method, msg.args);
                        if (method.Name != "SyncPlayerState")
                            LogAppended?.Invoke(this, $"调用本地方法：{method.Name}, 参数：{msg.args}");
                        if (method.DeclaringType == typeof(PlayerPeer))
                            method.Invoke(this, args);
                        else if (HostPlayer != null)
                            method.Invoke(HostPlayer, args);
                        else
                            LogAppended?.Invoke(this, ">> 调用失败");
                    }
                }
                catch (Exception ex)
                {
                    LogAppended?.Invoke(this, "本地方法调用异常：" + ex.Message);
                }
            }
        }
        public void Log(string msg)
        {
            LogAppended?.Invoke(this, msg);
        }
        public void ClearLogs()
        {
            logs.Clear();
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
        public static Dictionary<int, MethodInfo> MsgHandlers = new Dictionary<int, MethodInfo>();
    }
}

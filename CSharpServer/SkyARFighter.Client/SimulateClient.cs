using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        static SimulateClient()
        {
            foreach (var mi in typeof(SimulateClient).GetMethods())
            {
                if (mi.GetCustomAttribute(typeof(RemotingMethodAttribute)) is RemotingMethodAttribute attr)
                {
                    MsgHandlers[attr.MethodId] = mi;
                }
            }
        }

        public SimulateClient()
        {
            cameraTransform = new Matrix();
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = false;
            timer.Elapsed += Tick;
            State = States.Offline;
        }

        public enum States
        {
            Offline,
            Connected,
            InScene,
            SceneEdit
        }

        private States curState = States.Offline;
        public States State
        {
            get => curState;
            set
            {
                if (curState != value)
                {
                    curState = value;
                    StateChanged?.Invoke(curState);
                }
            }
        }

        public GameScene Scene => scene;

        public bool Connected => socket != null && socket.Connected;

        public PlayerInfo Info => playerInfo;

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
            State = States.Connected;
            
            ReceiveMessage();
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
                State = States.Offline;
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
                            LogAppended?.Invoke("断开连接");
                            Disconnected?.Invoke();
                            return;
                        }

                        var msgType = (RemotingMethodId)BitConverter.ToInt32(buffer, 0);
                        int length = BitConverter.ToInt32(buffer, 4);
                        if (length > 0)
                        {
                            if (!MsgHandlers.TryGetValue(msgType, out MethodInfo mi))
                                LogAppended?.Invoke("收到未注册的的远程方法调用请求：" + msgType);
                            else
                            {
                                try
                                {
                                    var context = buffer.Skip(8).Take(length).ToArray();
                                    var str = Encoding.UTF8.GetString(context, 0, context.Length);
                                    var args = JsonHelper.ParseMethodParameters(mi, str);
                                    LogAppended?.Invoke($"调用本地方法：{mi.Name}, 参数：{str}");

                                    mi.Invoke(this, args);
                                }
                                catch (Exception ex)
                                {
                                    LogAppended?.Invoke("远程方法调用异常：" + ex.Message);
                                }
                            }
                        }

                        waitingNextMessage.Set();
                    }), null);

                    while (socket.Connected && !waitingNextMessage.WaitOne(50)) ;
                }
            }));
        }

        public void InvokeRemoteMethod(RemotingMethodId methodId, object args)
        {
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

                LogAppended?.Invoke($"调用远程方法：{methodId}, 参数：{json}");
            }
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
        private GameScene scene = null;
        private PlayerInfo playerInfo = null;
        private ManualResetEvent waitingNextMessage = new ManualResetEvent(false);
        public event Action<string> LogAppended;
        public event Action<States> StateChanged;
        public event Action Disconnected;
        public event Action SceneContentChanged;
        public static Dictionary<RemotingMethodId, MethodInfo> MsgHandlers = new Dictionary<RemotingMethodId, MethodInfo>();
    }
}

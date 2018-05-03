using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
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
            
            BeginReceiveMessage();
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
                    LogAppended?.Invoke("断开连接");
                    Disconnected?.Invoke();
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
                            LogAppended?.Invoke($"调用本地方法：{mi.Name}, 参数：{str}");
                            mi.Invoke(this, args);
                        }
                        else
                        {
                            LogAppended?.Invoke("收到未注册的的本地方法调用请求：" + msgType);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogAppended?.Invoke("本地方法调用异常：" + ex.Message);
                    }
                    BeginReceiveMessage();
                }
                else
                {
                    LogAppended?.Invoke("断开连接");
                    Disconnected?.Invoke();
                }
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

            LogAppended?.Invoke($"调用远程方法：{type}, 参数：{json}");
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
        public event Action<string> LogAppended;
        public event Action<States> StateChanged;
        public event Action Disconnected;
        public static Dictionary<RemotingMethodId, MethodInfo> MsgHandlers = new Dictionary<RemotingMethodId, MethodInfo>();
    }
}

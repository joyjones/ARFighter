using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.IO;
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
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = false;
            timer.Elapsed += Tick;
            State = States.Offline;

            RequireDeviceId();
            System.Diagnostics.Debug.Assert(UniqueDeviceId != null);
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
                    timer.Enabled = curState != States.Offline;
                    StateChanged?.Invoke(curState);
                }
            }
        }

        public GameScene Scene => scene;

        public bool Connected => socket != null && socket.Connected;

        public PlayerInfo Info => playerInfo;

        public string UniqueDeviceId { get; private set; }

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
            
            ReceiveMessage_v3();

            if (UniqueDeviceId != null)
                Server_Login(UniqueDeviceId);
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

                Disconnected?.Invoke();
            }
        }

        private void ReceiveMessage_v3()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                Thread.CurrentThread.Name = "客户端通信处理线程";

                var buffer = new byte[1024];
                while (socket.Connected)
                {
                    socket.ReceiveTimeout = 5000;
                    try
                    {
                        if (socket.Receive(buffer, 0, 8, SocketFlags.None) == 0)
                            continue;
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.TimedOut)
                            continue;
                    }
                    catch (Exception ex)
                    {
                        LogAppended?.Invoke("断开连接");
                        Disconnected?.Invoke();
                        return;
                    }

                    var msgType = BitConverter.ToInt32(buffer, 0);
                    int contentLen = BitConverter.ToInt32(buffer, 4);
                    if (contentLen > 0)
                    {
                        if (!MsgHandlers.TryGetValue((RemotingMethodId)msgType, out MethodInfo mi))
                            LogAppended?.Invoke("收到未注册的的远程方法调用请求：" + (RemotingMethodId)msgType);
                        else
                        {
                            try
                            {
                                List<byte> bytes = new List<byte>();
                                int dataLen = 0, restLen = contentLen;
                                while (restLen > 0)
                                {
                                    int len = socket.Receive(buffer, 0, restLen > buffer.Length ? buffer.Length : restLen, SocketFlags.None);
                                    bytes.AddRange(buffer.Take(len));
                                    dataLen += len;
                                    restLen -= len;
                                }
                                var str = Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
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
            if (ts.TotalMilliseconds >= 10 * 1000)
            {
                if (Info != null)
                {
                    Server_SyncPlayerState();
                }
                lastElapsedTick = e.SignalTime.Ticks;
            }
        }

        public static readonly int MaxDeviceCount = 10;
        public static readonly string DeviceListFile = $"{Environment.CurrentDirectory}\\devices.dat";
        public void GenerateDeviceList()
        {
            using (var sw = new StreamWriter(DeviceListFile))
            {
                for (int i = 0; i < MaxDeviceCount; ++i)
                {
                    var dev = $"SIMCLINET{i + 1}:0";
                    sw.WriteLine(dev);
                }
            }
        }

        public string RequireDeviceId()
        {
            if (!File.Exists(DeviceListFile))
                GenerateDeviceList();
            string deviceId = null;
            var lst = new List<(string dev, bool used)>();
            using (var sr = new StreamReader(DeviceListFile))
            {
                while (!sr.EndOfStream)
                {
                    var ln = sr.ReadLine();
                    if (string.IsNullOrEmpty(ln))
                        continue;
                    string[] ss = ln.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    bool used = int.Parse(ss[1]) != 0;
                    if (deviceId == null && !used)
                    {
                        deviceId = ss[0];
                        used = true;
                    }
                    lst.Add((ss[0], used));
                }
            }
            if (deviceId != null)
            {
                using (var sw = new StreamWriter(DeviceListFile))
                {
                    foreach (var (dev, used) in lst)
                        sw.WriteLine($"{dev}:{(used ? 1 : 0)}");
                }
            }
            UniqueDeviceId = deviceId;
            return deviceId;
        }

        public bool ReleaseDeviceId()
        {
            if (!File.Exists(DeviceListFile) || string.IsNullOrEmpty(UniqueDeviceId))
                return false;
            bool released = false;
            var lst = new List<(string dev, bool used)>();
            using (var sr = new StreamReader(DeviceListFile))
            {
                while (!sr.EndOfStream)
                {
                    var ln = sr.ReadLine();
                    if (string.IsNullOrEmpty(ln))
                        continue;
                    string[] ss = ln.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    bool used = int.Parse(ss[1]) != 0;
                    if (ss[0] == UniqueDeviceId)
                    {
                        used = false;
                        released = true;
                    }
                    lst.Add((ss[0], used));
                }
            }
            if (released)
            {
                UniqueDeviceId = null;
                using (var sw = new StreamWriter(DeviceListFile))
                {
                    foreach (var (dev, used) in lst)
                        sw.WriteLine($"{dev}:{(used ? 1 : 0)}");
                }
            }
            return released;
        }

        private Socket socket = null;
        private long lastElapsedTick;
        private Vector3 cameraPos = new Vector3();
        private Vector3 cameraRotation = new Vector3();
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

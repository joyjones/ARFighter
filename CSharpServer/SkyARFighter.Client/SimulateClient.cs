using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using SkyARFighter.Common.Network;
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
            MsgHandlers = RemotingMethodAttribute.GetTypeMethodsMapping(typeof(SimulateClient));
        }

        public SimulateClient()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = false;
            timer.Elapsed += Tick;
            State = States.Offline;
            MsgProc = new Communication(MsgHandlers);

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

        public Communication MsgProc { get; private set; }

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

            MsgProc.StartupThread("客户端消息处理线程", socket, LogAppended, Disconnected);

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

        public void InvokeRemoteMethod(RemotingMethodId methodId, object args)
        {
            MsgProc.SendMessage(socket, methodId, args, LogAppended);
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
            ProcessMessages();
        }

        private void ProcessMessages()
        {
            foreach (var msg in MsgProc.Messages)
            {
                //var content = ZipHelper.GZipDecompressString(msg.args);
                if (MsgHandlers.TryGetValue((int)msg.code, out MethodInfo mi))
                {
                    try
                    {
                        var args = JsonHelper.ParseMethodParameters(mi, msg.args);
                        mi.Invoke(this, args);
                    }
                    catch (Exception ex)
                    {
                        LogAppended?.Invoke("远程方法调用异常：" + ex.Message);
                    }
                }
                else
                {
                    LogAppended?.Invoke("收到未注册的的远程方法调用请求：" + msg.code);
                }
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
        private ManualResetEvent receivedNewMessage = new ManualResetEvent(false);
        public event Action<string> LogAppended;
        public event Action<States> StateChanged;
        public event Action Disconnected;
        public event Action SceneContentChanged;
        public static Dictionary<int, MethodInfo> MsgHandlers = new Dictionary<int, MethodInfo>();
    }
}

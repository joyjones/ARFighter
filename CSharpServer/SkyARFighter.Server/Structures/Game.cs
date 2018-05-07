using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using SkyARFighter.Server.Network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Game
    {
        public Game()
        {
            Initialize();
        }

        public bool IsRunning
        {
            get { return isRunning.WaitOne(0); }
        }

        public int OnlinePlayersCount
        {
            get { return players.Count; }
        }

        public MySqlConnectionFactory DB
        {
            get; private set;
        }

        private static readonly Type[] NeedLoadingGameObjectTypes = new Type[]
        {
            typeof(Marker), typeof(Model), typeof(Player),
            typeof(Scene), typeof(SceneModel), typeof(SceneMarker)
        };

        private void Initialize()
        {
            try
            {
                var file = Program.ConfigsFile;
                if (File.Exists(file + ".bk"))
                    File.Copy(file + ".bk", file, true);
                var connName = ConfigurationManager.AppSettings["connectionName"];
                var connString = ConfigurationManager.ConnectionStrings[connName].ToString();
                DB = new MySqlConnectionFactory(connString);

                Program.Server.Log("正在加载数据库数据...");
                using (var ctx = DB.CreateContext())
                {
                    foreach (var t in NeedLoadingGameObjectTypes)
                    {
                        var method = t.BaseType.GetMethod("LoadDB");
                        method.Invoke(null, new object[] { ctx });
                    }
                }

                foreach (var info in Marker.Records)
                {
                    var obj = new Marker(info);
                    markers[info.Id] = obj;
                    obj.Load();
                }
                foreach (var info in Model.Records)
                {
                    var obj = new Model(info);
                    models[info.Id] = obj;
                    obj.Load();
                }
                foreach (var info in Scene.Records)
                {
                    var scene = new Scene(this, info);
                    scenes[info.Id] = scene;
                    scene.Load();
                }
                StartupLogic();
            }
            catch (Exception ex)
            {
                Program.Server.Log("Database Initialize Failed: " + ex.Message);
            }
        }

        private void StartupLogic()
        {
            isRunning.Set();
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                Thread.CurrentThread.Name = "主逻辑线程";

                long lastTick = 0;
                while (isRunning.WaitOne(0))
                {
                    long currTick = DateTime.Now.Ticks;
                    long elapsedTick = (lastTick == 0 ? 0 : (currTick - lastTick));
                    var ts = new TimeSpan(elapsedTick);

                    lock (remotingMessages)
                    {
                        while (remotingMessages.Count > 0)
                        {
                            var (peer, method, json) = remotingMessages.Dequeue();
                            try
                            {
                                var args = JsonHelper.ParseMethodParameters(method, json);
                                if (method.Name != "SyncPlayerState")
                                    peer.Log($"调用远程方法：{method.Name}, 参数：{json}");
                                if (method.DeclaringType == typeof(PlayerPeer))
                                    method.Invoke(peer, args);
                                else if (peer.HostPlayer != null)
                                    method.Invoke(peer.HostPlayer, args);
                                else
                                    peer.Log(">> 调用失败");
                            }
                            catch (Exception ex)
                            {
                                peer.Log("远程方法调用异常：" + ex.Message);
                            }
                        }
                    }

                    lock (scenes)
                    {
                        foreach (var s in scenes.Values)
                        {
                            s.Tick(ts);
                        }
                    }

                    lock (players)
                    {
                        foreach (var p in players.Values)
                        {
                            p.Tick(ts);
                        }
                    }
                    lastTick = currTick;
                    Thread.Sleep(50);
                }
            }));
        }

        public Player EnterPlayer(PlayerPeer peer, LoginWay way, string token, string password = null)
        {
            Player player = null;
            var info = GetRegisteredPlayerInfo(way, token, password);
            if (info != null)
                player = new Player(peer, info);
            else if (way == LoginWay.DeviceId && !string.IsNullOrEmpty(token))
                player = new Player(peer, token);
            else
                return null;
            peer.Disconnected += Peer_Disconnected;
            player.Client_Welcome(player.Info);
            players[player.Id] = player;
            player.Info.Save(DB);
            PlayerLoggedIn?.Invoke(player);
            return player;
        }

        private void Peer_Disconnected(PlayerPeer peer)
        {
            var player = peer.HostPlayer;
            if (player == null)
                return;
            if (player.CurScene != null)
                player.CurScene.RemovePlayer(player);
            players.Remove(player.Id);
        }

        public void PushRemotingMessage(PlayerPeer peer, MethodInfo method, string json)
        {
            lock (remotingMessages)
                remotingMessages.Enqueue((peer, method, json));
        }

        public Scene RequirePlayerScene(Player player, string identityName)
        {
            var scene = scenes.Values.Where(s => s.StartupMarker.ParentMarker.Info.Name == identityName).FirstOrDefault();
            if (scene != null)
            {
                scene.AddPlayer(player);
                players.Remove(player.Id);
            }
            return scene;
        }

        public PlayerInfo GetRegisteredPlayerInfo(LoginWay way, string token, string password)
        {
            PlayerInfo pi = null;
            switch (way)
            {
                case LoginWay.DeviceId:
                    pi = Player.Records.Where(i => i.UniqueDeviceId == token).FirstOrDefault(); break;
                case LoginWay.AccessToken:
                    {
                        pi = Player.Records.Where(i => i.AccessToken == token).FirstOrDefault();
                        bool expired = false;
                        if (pi != null && expired)
                            pi = null;
                    } break;
                case LoginWay.Account:
                    {
                        pi = Player.Records.Where(i => i.Account == token).FirstOrDefault();
                        if (pi != null)
                        {
                            if (string.IsNullOrEmpty(password))
                                pi = null;
                            else
                            {
                                string md5 = CommonMethods.ComputeMD5(password + pi.PasswordSalt);
                                if (!md5.Equals(pi.Password))
                                    pi = null;
                            }
                        }
                    } break;
            }
            return pi;
        }

        public Marker GetMarker(long id)
        {
            markers.TryGetValue(id, out Marker m);
            return m;
        }

        public Model GetModel(long id)
        {
            models.TryGetValue(id, out Model m);
            return m;
        }

        private ManualResetEvent isRunning = new ManualResetEvent(false);
        private Dictionary<long, Marker> markers = new Dictionary<long, Marker>();
        private Dictionary<long, Model> models = new Dictionary<long, Model>();
        private Dictionary<long, Scene> scenes = new Dictionary<long, Scene>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
        private Queue<(PlayerPeer peer, MethodInfo method, string json)> remotingMessages = new Queue<(PlayerPeer, MethodInfo, string)>();
        public event Action<Player> PlayerLoggedIn;
    }
}

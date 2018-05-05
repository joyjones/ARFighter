using SkyARFighter.Common;
using SkyARFighter.Server.Network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

        public Player EnterPlayer(PlayerPeer peer)
        {
            var player = new Player(peer);
            player.Client_Welcome(player.Id);
            return player;
        }

        public Scene RequirePlayerScene(Player player, string identityName)
        {
            return scenes.Values.Where(s => s.StartupName == identityName).FirstOrDefault();
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
    }
}

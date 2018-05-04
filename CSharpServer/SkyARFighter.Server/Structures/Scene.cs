using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Scene : GameObject<SceneInfo>
    {
        public Scene(Game game, SceneInfo info)
            : base(info)
        {
            ParentGame = game;
        }
        public Game ParentGame
        {
            get; private set;
        }
        public string StartupName
        {
            get; private set;
        }
        public IEnumerable<SceneModel> Models => models.Values;

        public override void Load()
        {
            foreach (var mi in SceneMarker.Records)
            {
                var sm = new SceneMarker(this, mi);
                markers[sm.ParentMarker.Info.Name] = sm;
                if (StartupName == null)
                    StartupName = sm.ParentMarker.Info.Name;
            }
            foreach (var mi in SceneModel.Records)
            {
                var sm = new SceneModel(this, mi);
                models[sm.Id] = sm;
            }
        }
        public void AddPlayer(Player player)
        {
            players[player.Id] = player;
        }
        public void AddModel(long playerId, long typeId, Vector3 pos, Vector3 scale, Vector4 rotate)
        {
            var modelInfo = Model.GetDataInfo(typeId);
            var smi = new SceneModelInfo()
            {
                SceneId = Id,
                ModelId = typeId,
                PosX = pos.x,
                PosY = pos.y,
                PosZ = pos.z,
                ScaleX = scale.x,
                ScaleY = scale.y,
                ScaleZ = scale.z,
                RotationX = rotate.x,
                RotationY = rotate.y,
                RotationZ = rotate.z,
                CreatePlayerId = playerId
            };
            var model = new SceneModel(this, smi);
            models[model.Id] = model;

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == playerId)
                    continue;
                plr.Client_CreateSceneModel(playerId, typeId, pos, scale, rotate);
            }
        }

        private Dictionary<string, SceneMarker> markers = new Dictionary<string, SceneMarker>();
        private Dictionary<long, SceneModel> models = new Dictionary<long, SceneModel>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

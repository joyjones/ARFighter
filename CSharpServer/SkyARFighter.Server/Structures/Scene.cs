using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Scene : GameObject
    {
        public Scene(string name)
        {
            IdentityName = name;
        }
        public string IdentityName
        {
            get; private set;
        }
        public IEnumerable<SceneModel> Models => models.Values;

        public void AddPlayer(Player player)
        {
            players[player.Id] = player;
        }
        public void AddModel(long playerId, SceneModelType type, Vector3 pos, Vector3 scale, Vector4 rotate)
        {
            var model = new SceneModel();
            model.Info.Pos = pos;
            model.Info.Scale = scale;
            model.Info.Rotation = rotate;
            models[model.Id] = model;

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == playerId)
                    continue;
                plr.Client_CreateSceneModel(playerId, type, pos, scale, rotate);
            }
        }

        private Dictionary<long, SceneModel> models = new Dictionary<long, SceneModel>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

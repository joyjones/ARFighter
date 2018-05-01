using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Scene
    {
        public Scene(string name)
        {
            IdentityName = name;
        }
        public string IdentityName
        {
            get; private set;
        }
        public void AddPlayer(Player player)
        {
            players[player.Id] = player;
        }
        public void AddModel(long playerId, SceneModelType type, Vector3 size, Matrix transform)
        {
            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == playerId)
                    continue;
                plr.Client_CreateSceneModel(playerId, type, size, transform);
            }
        }

        private Dictionary<long, Model> models = new Dictionary<long, Model>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

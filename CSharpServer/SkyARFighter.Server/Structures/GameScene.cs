using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server
{
    public class GameScene
    {
        public GameScene()
        {

        }
        public string IdentityName
        {
            get; set;
        }
        public void AddPlayer(Player player)
        {
            players[player.Id] = player;
        }
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

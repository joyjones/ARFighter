using SkyARFighter.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Game
    {
        public Game()
        {
        }

        public Player EnterPlayer(PlayerPeer peer)
        {
            var player = new Player(peer);
            return player;
        }

        public Scene RequirePlayerScene(Player player, string identityName)
        {
            if (scenes.TryGetValue(identityName, out Scene scene))
                return scene;
            scene = new Scene(identityName);
            scene.AddPlayer(player);
            scenes[scene.IdentityName] = scene;
            return scene;
        }

        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

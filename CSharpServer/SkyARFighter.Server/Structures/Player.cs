using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using SkyARFighter.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public partial class Player : GameObject<PlayerInfo>
    {
        public Player(PlayerInfo info)
            : base(info)
        {
        }
        public Player(PlayerPeer peer)
        {
            Peer = peer;
            Peer.HostPlayer = this;
            Id = CommonMethods.Rander.Next();
        }

        public long Id
        {
            get; private set;
        }

        public PlayerPeer Peer
        {
            get; private set;
        }

        private Scene curScene = null;
        private Matrix cameraTransform = new Matrix();
    }
}

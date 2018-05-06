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
        public Player(PlayerPeer peer, PlayerInfo info)
            : base(info)
        {
            Peer = peer;
            Peer.HostPlayer = this;
            GenerateAccessToken();
        }
        public Player(PlayerPeer peer, string deviceId)
        {
            Peer = peer;
            Peer.HostPlayer = this;
            Info = new PlayerInfo();
            Id = Info.Id;
            Info.UniqueDeviceId = deviceId;
            Info.Nickname = "玩家" + Info.Id.ToString().Substring(Info.Id.ToString().Length - 5);
            GenerateAccessToken();
            AddModelInfo(Info);
        }

        public PlayerPeer Peer
        {
            get; private set;
        }

        public Scene CurScene
        {
            get; set;
        }

        public void GenerateAccessToken()
        {
            Info.AccessToken = CommonMethods.MakeRandomString(32);
        }

        public override string ToString()
        {
            var key = Info.Account;
            if (string.IsNullOrEmpty(key))
                key = Info.UniqueDeviceId;
            if (!string.IsNullOrEmpty(key))
                key = $"({key})";
            return Info.Nickname + key;
        }

        private Matrix cameraTransform = new Matrix();
    }
}

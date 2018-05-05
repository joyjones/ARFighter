using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public partial class Player
    {
        public void Client_Welcome(long playerId)
        {
            Peer.SendMessage(RemotingMethodId.Welcome, new object[] { playerId });
        }

        public void Client_SetupWorld(string identityName, SceneModelInfo[] models)
        {
            Peer.SendMessage(RemotingMethodId.SetupWorld, new object[] { identityName, models });
        }

        public void Client_SyncCamera(Matrix mat)
        {
            Peer.SendMessage(RemotingMethodId.SyncCamera, new object[] { mat });
        }

        public void Client_CreateSceneModel(long playerId, long typeId, Vector3 pos, Vector3 scale, Vector4 rotate)
        {
            Peer.SendMessage(RemotingMethodId.CreateSceneModel, new object[] { playerId, typeId, pos, scale, rotate });
        }
    }
}

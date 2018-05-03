using SkyARFighter.Common;
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

        public void Client_SetupWorld(string identityName)
        {
            Peer.SendMessage(RemotingMethodId.SetupWorld, new object[] { identityName });
        }

        public void Client_SyncCamera(Matrix mat)
        {
            Peer.SendMessage(RemotingMethodId.SyncCamera, new object[] { mat });
        }

        public void Client_CreateSceneModel(long playerId, SceneModelType type, Vector3 size, Matrix transform)
        {
            Peer.SendMessage(RemotingMethodId.CreateSceneModel, new object[] { playerId, type, size, transform });
        }
    }
}

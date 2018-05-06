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
        public void Client_Welcome(PlayerInfo info)
        {
            Peer.SendMessage(RemotingMethodId.Welcome, new object[] { info.AccessToken, info });
        }

        public void Client_SetupWorld(string identityName, SceneInfo sceneInfo, SceneModelInfo[] models)
        {
            Peer.SendMessage(RemotingMethodId.SetupWorld, new object[] { identityName, sceneInfo, models });
        }

        public void Client_SendMessage(MessageType type, string message)
        {
            Peer.SendMessage(RemotingMethodId.SendMessage, new object[] { type, message });
        }

        public void Client_CreateSceneModel(SceneModelInfo info)
        {
            Peer.SendMessage(RemotingMethodId.CreateSceneModel, new object[] { info });
        }

        public void Client_MoveSceneModel(long playerId, long modelId, Vector3 pos)
        {
            Peer.SendMessage(RemotingMethodId.MoveSceneModel, new object[] { playerId, modelId, pos });
        }

        public void Client_RotateSceneModel(long playerId, long modelId, Vector3 rotation)
        {
            Peer.SendMessage(RemotingMethodId.RotateSceneModel, new object[] { playerId, modelId, rotation });
        }

        public void Client_ScaleSceneModel(long playerId, long modelId, Vector3 scale)
        {
            Peer.SendMessage(RemotingMethodId.ScaleSceneModel, new object[] { playerId, modelId, scale });
        }

        public void Client_DeleteSceneModel(long playerId, long modelId)
        {
            Peer.SendMessage(RemotingMethodId.DeleteSceneModel, new object[] { playerId, modelId });
        }

        public void Client_AddPlayer(PlayerInfo info)
        {
            Peer.SendMessage(RemotingMethodId.AddPlayer, new object[] { info });
        }

        public void Client_RemovePlayer(long playerId)
        {
            Peer.SendMessage(RemotingMethodId.RemovePlayer, new object[] { playerId });
        }

        public void Client_SyncPlayerState(long playerId, Vector3 pos, Vector3 rotation)
        {
            Peer.SendMessage(RemotingMethodId.SyncPlayerState, new object[] { playerId, pos, rotation });
        }
    }
}

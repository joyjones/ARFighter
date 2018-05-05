﻿using SkyARFighter.Common;
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

        public void Client_SetupWorld(string identityName, SceneInfo sceneInfo, SceneModelInfo[] models)
        {
            Peer.SendMessage(RemotingMethodId.SetupWorld, new object[] { identityName, sceneInfo, models });
        }

        public void Client_SyncCamera(Matrix mat)
        {
            Peer.SendMessage(RemotingMethodId.SyncCamera, new object[] { mat });
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
    }
}

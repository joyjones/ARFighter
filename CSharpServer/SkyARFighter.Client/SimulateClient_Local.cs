using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        [RemotingMethod(RemotingMethodId.Welcome)]
        public void InitPlayer(string accessToken, PlayerInfo info)
        {
            playerInfo = info;
            playerInfo.AccessToken = accessToken;
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(long markerId, SceneInfo sceneInfo, SceneMarkerInfo[] markers, SceneModelInfo[] models)
        {
            timer.Enabled = true;
            scene = new GameScene(sceneInfo, markerId, markers, models);
            State = States.InScene;
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.AddPlayer)]
        public void AddPlayer(PlayerInfo info)
        {
            Scene.AddPlayer(info);
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.RemovePlayer)]
        public void RemovePlayer(long playerId)
        {
            Scene.RemovePlayer(playerId);
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.SyncPlayerState)]
        public void SyncPlayerState(long playerId, Vector3 camPos, Vector3 camRotation)
        {
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.SendMessage)]
        public void SendMessage(MessageType type, string message)
        {
        }

        [RemotingMethod(RemotingMethodId.CreateSceneModel)]
        public void CreateSceneModel(SceneModelInfo info)
        {
            scene.AddModel(info);
            SceneContentChanged?.Invoke();
            if (info.CreatePlayerId == playerInfo.Id)
            {
                Server_CreateSceneModel(info);
            }
        }

        [RemotingMethod(RemotingMethodId.MoveSceneModel)]
        public void MoveSceneModel(long playerId, long modelId, Vector3 pos)
        {
            scene.TransformModel(playerId, modelId, pos, null, null);
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.RotateSceneModel)]
        public void RotateSceneModel(long playerId, long modelId, Vector3 rotation)
        {
            scene.TransformModel(playerId, modelId, null, rotation, null);
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.ScaleSceneModel)]
        public void ScaleSceneModel(long playerId, long modelId, Vector3 scale)
        {
            scene.TransformModel(playerId, modelId, null, null, scale);
            SceneContentChanged?.Invoke();
        }

        [RemotingMethod(RemotingMethodId.DeleteSceneModel)]
        public void DeleteSceneModel(long playerId, long modelId)
        {
            scene.DeleteModel(modelId);
            SceneContentChanged?.Invoke();
        }
    }
}

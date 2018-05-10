using Newtonsoft.Json;
using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using SkyARFighter.Common.Network;
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
    public partial class SimulateClient : ILoginServiceCallback, IGameServiceCallback
    {
        public void InitPlayer(string accessToken, PlayerInfo info)
        {
            playerInfo = info;
            playerInfo.AccessToken = accessToken;
            SceneContentChanged?.Invoke();
        }

        public void SetupWorld(long markerId, SceneInfo sceneInfo, SceneMarkerInfo[] markers, SceneModelInfo[] models)
        {
            timer.Enabled = true;
            scene = new GameScene(sceneInfo, markerId, markers, models);
            State = States.InScene;
            SceneContentChanged?.Invoke();
        }

        public void AddPlayer(PlayerInfo info)
        {
            Scene.AddPlayer(info);
            SceneContentChanged?.Invoke();
        }

        public void RemovePlayer(long playerId)
        {
            Scene.RemovePlayer(playerId);
            SceneContentChanged?.Invoke();
        }

        public void SyncPlayerState(long playerId, Vector3 camPos, Vector3 camRotation)
        {
            SceneContentChanged?.Invoke();
        }

        public void SendMessage(MessageType type, string message)
        {
        }

        public void CreateSceneModel(SceneModelInfo info)
        {
            scene.AddModel(info);
            SceneContentChanged?.Invoke();
            if (info.CreatePlayerId == playerInfo.Id)
            {
                Server_CreateSceneModel(info);
            }
        }

        public void MoveSceneModel(long playerId, long modelId, Vector3 pos)
        {
            scene.TransformModel(playerId, modelId, pos, null, null);
            SceneContentChanged?.Invoke();
        }

        public void RotateSceneModel(long playerId, long modelId, Vector3 rotation)
        {
            scene.TransformModel(playerId, modelId, null, rotation, null);
            SceneContentChanged?.Invoke();
        }

        public void ScaleSceneModel(long playerId, long modelId, Vector3 scale)
        {
            scene.TransformModel(playerId, modelId, null, null, scale);
            SceneContentChanged?.Invoke();
        }

        public void DeleteSceneModel(long playerId, long modelId)
        {
            scene.DeleteModel(modelId);
            SceneContentChanged?.Invoke();
        }
    }
}

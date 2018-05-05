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
        public void InitPlayer(long playerId)
        {
            playerInfo = new PlayerInfo();
            playerInfo.Id = playerId;
        }
        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(string identityName, SceneInfo sceneInfo, SceneModelInfo[] models)
        {
            timer.Enabled = true;
            scene = new GameScene(sceneInfo, identityName, models);
            State = States.InScene;
            SceneContentChanged?.Invoke();
        }

        public void AddPlayer(long playerId, Matrix cameraTrans)
        {
        }

        [RemotingMethod(RemotingMethodId.CreateSceneModel)]
        public void CreateSceneModel(SceneModelInfo info)
        {
            scene.AddModel(info);
            SceneContentChanged?.Invoke();
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
    }
}

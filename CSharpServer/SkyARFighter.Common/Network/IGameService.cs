using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.Network
{
    [ServiceContract(Callback = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [RemotingMethod((int)RemotingMethodId.SetupWorld)]
        void SetupWorld(string identityName);
        [RemotingMethod((int)RemotingMethodId.CreateSceneModel)]
        void CreateSceneModel(SceneModelInfo info);
        [RemotingMethod((int)RemotingMethodId.MoveSceneModel)]
        void MoveSceneModel(long modelId, Vector3 pos);
        [RemotingMethod((int)RemotingMethodId.RotateSceneModel)]
        void RotateSceneModel(long modelId, Vector3 rotation);
        [RemotingMethod((int)RemotingMethodId.ScaleSceneModel)]
        void ScaleSceneModel(long modelId, Vector3 scale);
        [RemotingMethod((int)RemotingMethodId.DeleteSceneModel)]
        void DeleteSceneModel(long modelId);
        [RemotingMethod((int)RemotingMethodId.SyncPlayerState)]
        void SyncPlayerState(Vector3 pos, Vector3 rotation);
    }

    public interface IGameServiceCallback
    {
        [RemotingMethod((int)RemotingMethodId.SetupWorld)]
        void SetupWorld(long markerId, SceneInfo sceneInfo, SceneMarkerInfo[] markers, SceneModelInfo[] models);

        [RemotingMethod((int)RemotingMethodId.AddPlayer)]
        void AddPlayer(PlayerInfo info);

        [RemotingMethod((int)RemotingMethodId.RemovePlayer)]
        void RemovePlayer(long playerId);

        [RemotingMethod((int)RemotingMethodId.SyncPlayerState)]
        void SyncPlayerState(long playerId, Vector3 camPos, Vector3 camRotation);

        [RemotingMethod((int)RemotingMethodId.SendMessage)]
        void SendMessage(MessageType type, string message);

        [RemotingMethod((int)RemotingMethodId.CreateSceneModel)]
        void CreateSceneModel(SceneModelInfo info);

        [RemotingMethod((int)RemotingMethodId.MoveSceneModel)]
        void MoveSceneModel(long playerId, long modelId, Vector3 pos);

        [RemotingMethod((int)RemotingMethodId.RotateSceneModel)]
        void RotateSceneModel(long playerId, long modelId, Vector3 rotation);

        [RemotingMethod((int)RemotingMethodId.ScaleSceneModel)]
        void ScaleSceneModel(long playerId, long modelId, Vector3 scale);

        [RemotingMethod((int)RemotingMethodId.DeleteSceneModel)]
        void DeleteSceneModel(long playerId, long modelId);
    }
}

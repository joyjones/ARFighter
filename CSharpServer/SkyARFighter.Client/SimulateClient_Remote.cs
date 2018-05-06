using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        public void Server_Login(string uniqueId)
        {
            InvokeRemoteMethod(RemotingMethodId.Login, new object[] { LoginWay.DeviceId, uniqueId, "" });
        }

        public void Server_SetupWorld(string identityName)
        {
            InvokeRemoteMethod(RemotingMethodId.SetupWorld, new object[] { identityName });
        }

        public void Server_CreateSceneModel(SceneModelInfo info)
        {
            InvokeRemoteMethod(RemotingMethodId.CreateSceneModel, new object[] { info });
        }

        public void Server_MoveSceneModel(long modelId, Vector3 pos)
        {
            InvokeRemoteMethod(RemotingMethodId.MoveSceneModel, new object[] { modelId, pos });
        }

        public void Server_RotateSceneModel(long modelId, Vector3 rotation)
        {
            InvokeRemoteMethod(RemotingMethodId.RotateSceneModel, new object[] { modelId, rotation });
        }

        public void Server_ScaleSceneModel(long modelId, Vector3 scale)
        {
            InvokeRemoteMethod(RemotingMethodId.ScaleSceneModel, new object[] { modelId, scale });
        }

        public void Server_DeleteSceneModel(long modelId)
        {
            InvokeRemoteMethod(RemotingMethodId.ScaleSceneModel, new object[] { modelId });
        }

        public void Server_SyncPlayerState()
        {
            InvokeRemoteMethod(RemotingMethodId.SyncPlayerState, new object[] { cameraPos, cameraRotation });
        }
    }
}

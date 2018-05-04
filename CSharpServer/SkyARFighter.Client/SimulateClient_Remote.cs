using SkyARFighter.Common;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        public void Server_SetupWorld(string identityName)
        {
            InvokeRemoteMethod(RemotingMethodId.SetupWorld, new object[] { identityName });
        }

        public void Server_SyncCamera()
        {
            InvokeRemoteMethod(RemotingMethodId.SyncCamera, new object[] { cameraTransform });
        }

        public void Server_CreateObject(SceneModelType type, Vector3 pos, Vector3 scale, Vector4 rotate)
        {
            InvokeRemoteMethod(RemotingMethodId.CreateSceneModel, new object[] { type, pos, scale, rotate });
        }
    }
}

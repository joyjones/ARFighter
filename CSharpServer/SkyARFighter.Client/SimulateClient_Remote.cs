using Newtonsoft.Json;
using SkyARFighter.Common;
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

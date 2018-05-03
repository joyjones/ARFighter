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
        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(string identityName)
        {
            curScene = Program.Game.RequirePlayerScene(this, identityName);
            Client_SetupWorld(identityName);
        }

        [RemotingMethod(RemotingMethodId.SyncCamera)]
        public void SyncCamera(Matrix mat)
        {
            cameraTransform.CopyFrom(mat);
        }

        [RemotingMethod(RemotingMethodId.CreateSceneModel)]
        public void CreateSceneModel(SceneModelType type, Vector3 size, Matrix transform)
        {
            curScene.AddModel(Id, type, size, transform);
        }
    }
}

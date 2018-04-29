using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server
{
    public partial class Player
    {
        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(string identityName)
        {
            Program.Server.RequirePlayerScene(this, identityName);
        }

        [RemotingMethod(RemotingMethodId.SyncCamera)]
        public void SyncCamera(Matrix mat)
        {
            cameraTransform.Fill(mat.Values);
        }

        [RemotingMethod(RemotingMethodId.CreateObject)]
        public void CreateObject(ObjectType type, Vector3 size, Matrix transform)
        {

        }
    }
}

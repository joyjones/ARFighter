using SkyARFighter.Common;
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
        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(string identityName)
        {
            Program.Game.RequirePlayerScene(this, identityName);
            if (CurScene != null)
            {
                Client_SetupWorld(identityName, CurScene.Info, CurScene.Models.Select(m => m.Info).ToArray());
            }
        }

        [RemotingMethod(RemotingMethodId.SyncCamera)]
        public void SyncCamera(Matrix mat)
        {
            cameraTransform.CopyFrom(mat);
        }

        [RemotingMethod(RemotingMethodId.CreateSceneModel)]
        public void CreateSceneModel(SceneModelInfo info)
        {
            if (CurScene == null)
                return;
            info.CreatePlayerId = Id;
            CurScene.AddModel(info);
        }

        [RemotingMethod(RemotingMethodId.MoveSceneModel)]
        public void MoveSceneModel(long modelId, Vector3 pos)
        {
            if (CurScene == null)
                return;
            CurScene.TransformModel(Id, modelId, pos, null, null);
        }

        [RemotingMethod(RemotingMethodId.RotateSceneModel)]
        public void RotateSceneModel(long modelId, Vector3 rotation)
        {
            if (CurScene == null)
                return;
            CurScene.TransformModel(Id, modelId, null, rotation, null);
        }

        [RemotingMethod(RemotingMethodId.ScaleSceneModel)]
        public void ScaleSceneModel(long modelId, Vector3 scale)
        {
            if (CurScene == null)
                return;
            CurScene.TransformModel(Id, modelId, null, null, scale);
        }
    }
}

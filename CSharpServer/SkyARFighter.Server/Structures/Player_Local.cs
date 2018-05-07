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
            if (CurScene == null)
                Client_SendMessage(MessageType.System_Failure, "未能识别到任何场景。");
            else
            {
                Client_SetupWorld(CurScene.StartupMarker.Id, CurScene.Info, CurScene.Markers.Select(m => m.Info).ToArray(), CurScene.Models.Select(m => m.Info).ToArray());

                foreach (var plr in CurScene.Players.ToArray())
                {
                    if (plr == this)
                        continue;
                    Client_AddPlayer(plr.Info);
                }
            }
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

        [RemotingMethod(RemotingMethodId.DeleteSceneModel)]
        public void DeleteSceneModel(long modelId)
        {
            if (CurScene == null)
                return;
            CurScene.DeleteModel(Id, modelId);
        }

        [RemotingMethod(RemotingMethodId.SyncPlayerState)]
        public void SyncPlayerState(Vector3 pos, Vector3 rotation)
        {
            cameraPos = pos;
            cameraRotation = rotation;

            foreach (var plr in CurScene.Players.ToArray())
            {
                if (plr == this)
                    continue;
                plr.Client_SyncPlayerState(Id, pos, rotation);
            }
        }
    }
}

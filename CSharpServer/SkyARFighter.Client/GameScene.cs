using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Client
{
    public class GameScene : SceneInfo
    {
        public GameScene(SceneInfo info, string identityName, SceneModelInfo[] models)
        {
            Id = info.Id;
            Name = info.Name;
            StartupName = identityName;
            foreach (var mi in models)
            {
                this.models[mi.Id] = mi;
            }
        }

        public string StartupName
        {
            get; private set;
        }

        public SceneModelInfo[] Models => models.Values.ToArray();
        
        public void AddModel(SceneModelInfo info)
        {
            models[info.Id] = info;
        }

        public SceneModelInfo GetModel(long id)
        {
            models.TryGetValue(id, out SceneModelInfo model);
            return model;
        }

        public void TransformModel(long playerId, long modelId, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            var model = GetModel(modelId);
            if (model == null)
                return;
            if (pos != null)
            {
                model.PosX = pos.x;
                model.PosY = pos.y;
                model.PosZ = pos.z;
            }
            if (rotation != null)
            {
                model.RotationX = rotation.x;
                model.RotationY = rotation.y;
                model.RotationZ = rotation.z;
            }
            if (scale != null)
            {
                model.ScaleX = scale.x;
                model.ScaleY = scale.y;
                model.ScaleZ = scale.z;
            }
        }

        private Dictionary<long, SceneModelInfo> models = new Dictionary<long, SceneModelInfo>();
    }
}

using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class SceneModel : GameObject<SceneModelInfo>
    {
        public SceneModel(Scene scene, SceneModelInfo info)
            : base(info)
        {
            ParentScene = scene;
        }

        public Scene ParentScene
        {
            get; private set;
        }
        public Vector3 Pos
        {
            get => new Vector3(Info.PosX, Info.PosY, Info.PosZ);
            set
            {
                Info.PosX = value.x;
                Info.PosY = value.y;
                Info.PosZ = value.z;
            }
        }
        public Vector3 Rotation
        {
            get => new Vector3(Info.RotationX, Info.RotationY, Info.RotationZ);
            set
            {
                Info.RotationX = value.x;
                Info.RotationY = value.y;
                Info.RotationZ = value.z;
            }
        }
        public Vector3 Scale
        {
            get => new Vector3(Info.ScaleX, Info.ScaleY, Info.ScaleZ);
            set
            {
                Info.ScaleX = value.x;
                Info.ScaleY = value.y;
                Info.ScaleZ = value.z;
            }
        }

    }
}

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
            get => curPos;
            set
            {
                curPos = value;
                Info.PosX = curPos.x;
                Info.PosY = curPos.y;
                Info.PosZ = curPos.z;
            }
        }
        public Vector3 Rotation
        {
            get => curRotation;
            set
            {
                curRotation = value;
                Info.RotationX = curRotation.x;
                Info.RotationY = curRotation.y;
                Info.RotationZ = curRotation.z;
            }
        }
        public Vector3 Scale
        {
            get => curScale;
            set
            {
                curScale = value;
                Info.ScaleX = curScale.x;
                Info.ScaleY = curScale.y;
                Info.ScaleZ = curScale.z;
            }
        }

        private Vector3 curPos = new Vector3();
        private Vector3 curRotation = new Vector3();
        private Vector3 curScale = new Vector3();
    }
}

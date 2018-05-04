using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class SceneMarker : GameObject<SceneMarkerInfo>
    {
        public SceneMarker(Scene scene, SceneMarkerInfo info)
            : base(info)
        {
            ParentScene = scene;
        }
        public Scene ParentScene
        {
            get; private set;
        }
        public Marker ParentMarker
        {
            get => ParentScene.ParentGame.GetMarker(Info.MarkderId);
        }
        public override void Load()
        {
            
        }
    }
}

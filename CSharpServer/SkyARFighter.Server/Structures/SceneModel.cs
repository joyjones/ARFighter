using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class SceneModel : GameObject
    {
        public SceneModelInfo Info
        {
            get; private set;
        } = new SceneModelInfo();
    }
}

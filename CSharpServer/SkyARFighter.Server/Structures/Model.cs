using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Model : GameObject<ModelInfo>
    {
        public Model(ModelInfo info)
            : base(info)
        {
        }
    }
}

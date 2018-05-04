using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Marker : GameObject<MarkerInfo>
    {
        public Marker(MarkerInfo info)
            : base(info)
        {
        }
    }
}

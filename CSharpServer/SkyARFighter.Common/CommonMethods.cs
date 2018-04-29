using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public static class CommonMethods
    {
        public static Random Rander { get; } = new Random(DateTime.Now.Millisecond);
    }
}

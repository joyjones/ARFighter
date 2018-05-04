using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public abstract class GameObject
    {
        public long Id { get; set; } = AutoGenerateID;

        public static long AutoGenerateID
        {
            get { return long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + CommonMethods.Rander.Next(100000).ToString().PadLeft(5, '0')); }
        }
    }
}

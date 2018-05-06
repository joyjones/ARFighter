using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Client
{
    public class Player : PlayerInfo
    {
        public Player(PlayerInfo info)
        {
            foreach (var fd in GetType().GetFields())
            {
                var val = fd.GetValue(info);
                fd.SetValue(this, val);
            }
        }

        public override string ToString()
        {
            var key = "";
            //var key = Account;
            //if (string.IsNullOrEmpty(key))
            //    key = UniqueDeviceId;
            //if (!string.IsNullOrEmpty(key))
            //    key = $"({key})";
            return Nickname + key;
        }
    }
}

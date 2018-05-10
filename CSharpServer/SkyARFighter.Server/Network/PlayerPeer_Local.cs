using SkyARFighter.Common;
using SkyARFighter.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Network
{
    public partial class PlayerPeer : ILoginService
    {
        public void Login(LoginWay way, string token, string password)
        {
            HostGame.EnterPlayer(this, way, token, password);
        }
    }
}

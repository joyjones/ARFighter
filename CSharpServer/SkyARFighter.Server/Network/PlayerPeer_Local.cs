using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Network
{
    public partial class PlayerPeer
    {
        [RemotingMethod(RemotingMethodId.Login)]
        public void Login(LoginWay way, string token, string password)
        {
            HostGame.EnterPlayer(this, way, token, password);
        }
    }
}

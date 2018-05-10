using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.Network
{
    [ServiceContract(Callback = typeof(ILoginServiceCallback))]
    public interface ILoginService
    {
        [RemotingMethod((int)RemotingMethodId.Login)]
        void Login(LoginWay way, string token, string password);
    }

    public interface ILoginServiceCallback
    {
        [RemotingMethod((int)RemotingMethodId.Welcome)]
        void InitPlayer(string accessToken, PlayerInfo info);
    }
}

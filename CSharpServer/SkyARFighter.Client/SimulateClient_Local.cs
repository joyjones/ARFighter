using Newtonsoft.Json;
using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SkyARFighter.Client
{
    public partial class SimulateClient
    {
        [RemotingMethod(RemotingMethodId.SetupWorld)]
        public void SetupWorld(string identityName)
        {
            timer.Enabled = true;
            scene = new GameScene(identityName);
            State = States.InScene;
        }

        public void AddPlayer(long playerId, Matrix cameraTrans)
        {
        }

        [RemotingMethod(RemotingMethodId.CreateSceneModel)]
        public void CreateSceneModel(long playerId, SceneModelType type, Vector3 size, Matrix transform)
        {
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    public class SceneModelInfo
    {
        [JsonProperty("pos")]
        public Vector3 Pos;
        [JsonProperty("scale")]
        public Vector3 Scale;
        [JsonProperty("rotate")]
        public Vector4 Rotation;
    }
}

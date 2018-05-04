using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("player")]
    public class PlayerInfo : DataInfo
    {
        [Column("nickname"), JsonProperty("nickname")]
        public string Nickname;
        [Column("access_token"), JsonProperty("access_token")]
        public string AccessToken;
    }
}

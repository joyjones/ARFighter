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
        [Column("account"), JsonProperty("account")]
        public string Account;
        [Column("password"), JsonIgnore]
        public string Password;
        [Column("unique_device_id"), JsonProperty("unique_device_id")]
        public string UniqueDeviceId;
        [Column("access_token"), JsonIgnore]
        public string AccessToken;
        [Column("password_salt"), JsonIgnore]
        public string PasswordSalt;
        [Column("last_login_time"), JsonProperty("last_login_time")]
        public int LastLoginTime;
    }
}

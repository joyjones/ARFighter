using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("scene")]
    public class SceneInfo : DataInfo
    {
        [Column("name"), JsonProperty("name")]
        public string Name;
    }
}

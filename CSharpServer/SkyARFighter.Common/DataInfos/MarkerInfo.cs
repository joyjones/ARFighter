using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("marker")]
    public class MarkerInfo : DataInfo
    {
        [Column("name"), JsonProperty("name")]
        public string Name;
        [Column("type"), JsonProperty("type")]
        public int Type;
        [Column("width"), JsonProperty("width")]
        public float Width;
        [Column("height"), JsonProperty("height")]
        public float Height;
    }
}

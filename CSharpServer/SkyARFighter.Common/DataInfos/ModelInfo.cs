using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("model")]
    public class ModelInfo : DataInfo
    {
        [Column("name"), JsonProperty("name")]
        public string Name;
        [Column("type"), JsonProperty("type")]
        public int Type;
        [Column("filename"), JsonProperty("filename")]
        public string Filename;
    }
}

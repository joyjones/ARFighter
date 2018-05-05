using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("scene_marker")]
    public class SceneMarkerInfo : DataInfo
    {
        [Column("scene_id"), JsonProperty("scene_id")]
        public long SceneId;
        [Column("marker_id"), JsonProperty("marker_id")]
        public long MarkderId;
        [Column("scale"), JsonProperty("scale")]
        public float Scale;
        [Column("offset_x"), JsonProperty("offset_x")]
        public float OriginOffsetX;
        [Column("offset_y"), JsonProperty("offset_y")]
        public float OriginOffsetY;
        [Column("offset_z"), JsonProperty("offset_z")]
        public float OriginOffsetZ;
        [Column("rotation_x"), JsonProperty("rotation_x")]
        public float OriginRotationX;
        [Column("rotation_y"), JsonProperty("rotation_y")]
        public float OriginRotationY;
        [Column("rotation_z"), JsonProperty("rotation_z")]
        public float OriginRotationZ;
    }
}

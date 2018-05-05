using Chloe.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.DataInfos
{
    [Table("scene_model")]
    public class SceneModelInfo : DataInfo
    {
        [Column("scene_id"), JsonProperty("scene_id")]
        public long SceneId;
        [Column("model_id"), JsonProperty("model_id")]
        public long ModelId;
        [Column("create_player_id"), JsonProperty("create_player_id")]
        public long? CreatePlayerId;
        [Column("description"), JsonProperty("description")]
        public string Description;
        [Column("pos_x"), JsonProperty("pos_x")]
        public float PosX;
        [Column("pos_y"), JsonProperty("pos_y")]
        public float PosY;
        [Column("pos_z"), JsonProperty("pos_z")]
        public float PosZ;
        [Column("scale_x"), JsonProperty("scale_x")]
        public float ScaleX;
        [Column("scale_y"), JsonProperty("scale_y")]
        public float ScaleY;
        [Column("scale_z"), JsonProperty("scale_z")]
        public float ScaleZ;
        [Column("rotation_x"), JsonProperty("rotation_x")]
        public float RotationX;
        [Column("rotation_y"), JsonProperty("rotation_y")]
        public float RotationY;
        [Column("rotation_z"), JsonProperty("rotation_z")]
        public float RotationZ;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public class Matrix
    {
        public Matrix()
        {
            Identity();
        }
        public Matrix(float[] vals)
        {
            Array.Copy(vals, 0, values, 0, 16);
        }
        public void CopyFrom(Matrix mat)
        {
            Array.Copy(mat.values, 0, values, 0, 16);
        }
        public void Identity()
        {
            for (int i = 0; i < values.Length; ++i)
            {
                int r = i / 4, c = i % 4;
                if (r == c)
                    values[i] = 1;
                else
                    values[i] = 0;
            }
        }
        [JsonProperty("r1")]
        public float[] R1
        {
            get => values.Take(4).ToArray();
            set => Array.Copy(value, 0, values, 0, 4);
        }
        [JsonProperty("r2")]
        public float[] R2
        {
            get => values.Skip(4).Take(4).ToArray();
            set => Array.Copy(value, 0, values, 4, 4);
        }
        [JsonProperty("r3")]
        public float[] R3
        {
            get => values.Skip(8).Take(4).ToArray();
            set => Array.Copy(value, 0, values, 8, 4);
        }
        [JsonProperty("r4")]
        public float[] R4
        {
            get => values.Skip(12).ToArray();
            set => Array.Copy(value, 0, values, 12, 4);
        }
        [JsonIgnore]
        public Vector3 Pos
        {
            get => new Vector3(values[12], values[13], values[14]);
            set
            {
                values[12] = value.x;
                values[13] = value.y;
                values[14] = value.z;
            }
        }
        private float[] values = new float[16];
    }
}

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
            Fill(vals);
        }
        public void Fill(float[] vals)
        {
            for (int i = 0; i < 16; ++i)
                values[i] = vals[i];
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
        [JsonProperty("values")]
        public float[] Values
        {
            get => values;
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

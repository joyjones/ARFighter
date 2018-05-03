using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public class Vector3
    {
        public Vector3()
        {
        }
        public Vector3(float _x, float _y, float _z)
        {
            x = _x; y = _y; z = _z;
        }
        public float x = 0;
        public float y = 0;
        public float z = 0;
    }

    public class Vector4
    {
        public Vector4()
        {
        }
        public Vector4(float _x, float _y, float _z, float _w)
        {
            x = _x; y = _y; z = _z; w = _w;
        }
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float w = 0;
    }
}

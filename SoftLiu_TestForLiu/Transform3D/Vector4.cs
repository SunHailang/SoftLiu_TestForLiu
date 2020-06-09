using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.Transform3D
{
    public class Vector4
    {
        public double x, y, z, w;

        public Vector4()
        {

        }

        public Vector4(double _x, double _y, double _z, double _w)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.w = _w;
        }

        public Vector4(Vector4 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public Vector4 Cross(Vector4 v)
        {
            return new Vector4(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x, 0);
        }

        public float Dot(Vector4 v)
        {
            return (float)(this.x * v.x + this.y * v.y + this.z * v.z);
        }

        public Vector4 Normalized
        {
            get
            {
                double Mod = Math.Sqrt(x * x + y * y + z * z + w * w);
                return new Vector4(x / Mod, y / Mod, z / Mod, w / Mod);
            }
        }
    }
}

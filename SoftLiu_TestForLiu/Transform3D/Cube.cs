using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.Transform3D
{
    public class Cube
    {
        Vector4 a = new Vector4(-0.5f, 0.5, 0.5, 1);
        Vector4 b = new Vector4(0.5f, 0.5, 0.5, 1);
        Vector4 c = new Vector4(0.5f, 0.5, -0.5, 1);
        Vector4 d = new Vector4(-0.5f, 0.5, -0.5, 1);

        Vector4 e = new Vector4(-0.5f, -0.5, 0.5, 1);
        Vector4 f = new Vector4(0.5f, -0.5, 0.5, 1);
        Vector4 g = new Vector4(0.5f, -0.5, -0.5, 1);
        Vector4 h = new Vector4(-0.5f, -0.5, -0.5, 1);

        private Triangle3D[] m_triangles = new Triangle3D[12];

        public Cube()
        {
            // top
            m_triangles[0] = new Triangle3D(a, b, c);
            m_triangles[1] = new Triangle3D(a, c, d);
            // bottom
            m_triangles[2] = new Triangle3D(e, h, f);
            m_triangles[3] = new Triangle3D(f, h, g);
            // front
            m_triangles[4] = new Triangle3D(d, c, g);
            m_triangles[5] = new Triangle3D(d, g, h);
            // back
            m_triangles[6] = new Triangle3D(a, e, b);
            m_triangles[7] = new Triangle3D(b, e, f);
            // right
            m_triangles[8] = new Triangle3D(b, f, c);
            m_triangles[9] = new Triangle3D(c, f, g);
            // left
            m_triangles[10] = new Triangle3D(a, d, h);
            m_triangles[11] = new Triangle3D(a, h, e);

        }

        public void Transform(Matrix4x4 m)
        {
            for (int i = 0; i < m_triangles.Length; i++)
            {
                m_triangles[i].Transform(m);
            }
        }

        public void CalculateLighting(Matrix4x4 _Object2World, Vector4 L)
        {
            for (int i = 0; i < m_triangles.Length; i++)
            {
                m_triangles[i].CalculateLighting(_Object2World, L);
            }
        }

        public void Draw(Graphics g, bool isLine)
        {
            for (int i = 0; i < m_triangles.Length; i++)
            {
                m_triangles[i].Draw(g, isLine);
            }
        }
    }
}

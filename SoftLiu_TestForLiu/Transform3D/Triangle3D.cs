using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.Transform3D
{

    public class Triangle3D
    {
        /*
         3D 旋转矩阵
              x轴                    y轴                      z轴
         [1,    0,   0, 0]     [ cos, 0, sin, 0]      [ cos, sin, 0, 0]
         [0,  cos, sin, 0]     [   0, 1,   0, 0]      [-sin, cos, 0, 0]
         [0, -sin, cos, 0]     [-sin, 0, cos, 0]      [   0,   0, 1, 0]
         [0,    0,   0, 1]     [0,    0,   0, 1]      [0,    0,   0, 1]

         3D 平移
                        [ 1,  0,  0, 0]
         [x, y, z, 1] * [ 0,  1,  0, 0] = [x+dx, y+dy, z+dz, 1]
                        [ 0,  0,  1, 0]
                        [dx, dy, dz, 1]

        3D 透视投影 (小孔成像) d表示小孔到影布的距离
        |                                     |
        |----d----|o|------------z------------|
        |                                     |
                        [ 1,  0,  0,   0]
         [x, y, z, 1] * [ 0,  1,  0,   0] = [x, y, z, z/d] =>(透视缩略) [x/(z/d), y/(z/d)]
                        [ 0,  0,  1, 1/d]
                        [ 0,  0,  0,   1]

        */

        private Vector4 a, b, c;

        public Vector4 A, B, C;

        float dot;
        bool cullBack;
        public Triangle3D()
        {

        }

        public Triangle3D(Vector4 a, Vector4 b, Vector4 c)
        {
            this.A = new Vector4(a);
            this.B = new Vector4(b);
            this.C = new Vector4(c);
            this.a = this.A;
            this.b = this.B;
            this.c = this.C;
        }

        // 三角形利用矩阵的乘法变换
        public void Transform(Matrix4x4 m)
        {
            this.a = m.Mul(this.A);
            this.b = m.Mul(this.B);
            this.c = m.Mul(this.C);
        }
        // 计算世界坐标系下的 法向量
        public void CalculateLighting(Matrix4x4 _Object2World, Vector4 L)
        {
            this.Transform(_Object2World);
            Vector4 u = this.b - this.a;
            Vector4 v = this.c - this.a;
            Vector4 normal = u.Cross(v);
            dot = normal.Normalized.Dot(L.Normalized);
            dot = Math.Max(0, dot);
            dot = Math.Min(dot, 1);
            Vector4 E = new Vector4(0, 0, -1, 0);
            // 小于0 剔除不显示
            cullBack = normal.Normalized.Dot(E) < 0;
        }

        // 绘制三角形到 2D窗口
        public void Draw(Graphics g, bool isLine)
        {
            PointF[] lins = this.Get2DPointFArr();
            if (isLine)
            {
                Pen pen = new Pen(Color.Green, 2);
                g.DrawLines(pen, lins);
            }
            else
            {
                if (!cullBack)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddLines(lins);
                    int rgb = (int)(200 * dot) + 55;
                    Brush brushes = new SolidBrush(Color.FromArgb(rgb, rgb, rgb));
                    g.FillPath(brushes, path);
                }
            }
        }

        private PointF[] Get2DPointFArr()
        {
            PointF[] arr = new PointF[4];
            arr[0] = Get2DPointF(this.a);
            arr[1] = Get2DPointF(this.b);
            arr[2] = Get2DPointF(this.c);
            arr[3] = arr[0];
            return arr;
        }

        private PointF Get2DPointF(Vector4 v)
        {
            PointF p = new PointF();
            p.X = (float)(v.x / v.w);
            p.Y = (float)(-v.y / v.w);
            return p;
        }
    }
}

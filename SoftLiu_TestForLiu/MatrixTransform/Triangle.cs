using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.MatrixTransform
{
    public class Triangle
    {
        PointF A, B, C;

        PointF a, b, c;

        public Triangle(PointF _A, PointF _B, PointF _C)
        {
            this.A = _A;
            this.B = _B;
            this.C = _C;
            this.a = A;
            this.b = B;
            this.c = C;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.Red, 2);
            g.DrawLine(pen, this.a, this.b);
            g.DrawLine(pen, this.b, this.c);
            g.DrawLine(pen, this.c, this.a);
        }

        public void Rotate(int degrees)
        {
            float angle = (float)(1 / 360.0f * Math.PI);

            this.a = GetRotatePoint(this.A, angle);
            this.b = GetRotatePoint(this.B, angle);
            this.c = GetRotatePoint(this.C, angle);
            this.A = a;
            this.B = b;
            this.C = c;
        }
        /// <summary>
        /// 2D 旋转矩阵
        /// [X, Y]  *  [  cos, sin]
        ///            [ -sin, cos]
        /// </summary>
        /// <param name="p"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private PointF GetRotatePoint(PointF p, float angle)
        {
            p.X = (float)(p.X * Math.Cos(angle) - p.Y * Math.Sin(angle));
            p.Y = (float)(p.X * Math.Sin(angle) + p.Y * Math.Cos(angle));
            return p;
        }

        /// <summary>
        /// 2D 缩放
        /// [ Kx,  0]
        /// [  0, Ky]
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(float scale)
        {
            this.a = GetScalePoint(this.A, scale);
            this.b = GetScalePoint(this.B, scale);
            this.c = GetScalePoint(this.C, scale);
        }
        private PointF GetScalePoint(PointF p, float scale)
        {
            p.X = (float)(p.X * scale);
            p.Y = (float)(p.Y * scale);
            return p;
        }

    }
}

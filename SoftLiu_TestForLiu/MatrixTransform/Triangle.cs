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

        public Triangle(PointF _A, PointF _B, PointF _C)
        {
            this.A = _A;
            this.B = _B;
            this.C = _C;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.Red, 2);
            g.DrawLine(pen, this.A, this.B);
            g.DrawLine(pen, this.B, this.C);
            g.DrawLine(pen, this.C, this.A);
        }

        public void Rotate(int degrees)
        {
            float angle = (float)(degrees / 360.0f * Math.PI);
            this.A.X = (float)(this.A.X * Math.Cos(angle) - this.A.Y * Math.Sin(angle));
            this.A.Y = (float)(this.A.X * Math.Sin(angle) - this.A.Y * Math.Cos(angle));

            this.B.X = (float)(this.B.X * Math.Cos(angle) - this.B.Y * Math.Sin(angle));
            this.B.Y = (float)(this.B.X * Math.Sin(angle) - this.B.Y * Math.Cos(angle));

            this.C.X = (float)(this.C.X * Math.Cos(angle) - this.C.Y * Math.Sin(angle));
            this.C.Y = (float)(this.C.X * Math.Sin(angle) - this.C.Y * Math.Cos(angle));
        }

        private PointF GetPoint(PointF p, float angle)
        {
            p.X = (float)(p.X * Math.Cos(angle) - p.Y * Math.Sin(angle));
            p.Y = (float)(p.X * Math.Sin(angle) - p.Y * Math.Cos(angle));
            return p;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.Transform3D
{
    public class Triangle3D
    {
        private Vector4 a, b, c;

        public Vector4 A, B, C;

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

        // 绘制三角形到 2D窗口
        public void Draw(Graphics g)
        {
            g.TranslateTransform(300, 300);
            Pen pen = new Pen(Color.Red, 2);
            g.DrawLines(pen, Get2DPointFArr());
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
            p.Y = (float)(v.y / v.w);
            return p;
        }
    }
}
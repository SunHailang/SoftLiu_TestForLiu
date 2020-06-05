using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_TestForLiu.MatrixTransform
{
    public partial class MatrixTransformForm : Form
    {
        Triangle m_triangle;
        int degress = 0;

        public MatrixTransformForm()
        {
            InitializeComponent();
        }

        private void MatrixTransformForm_Load(object sender, EventArgs e)
        {
            PointF A = new PointF(0, -200);
            PointF B = new PointF(200, 200);
            PointF C = new PointF(-200, 200);
            m_triangle = new Triangle(A, B, C);

            //m_triangle.Scale(0.5f);
            //m_triangle.Rotate(90);
        }

        private void MatrixTransformForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(300, 300);
            m_triangle.Draw(e.Graphics);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_triangle.Rotate(degress++);
            this.Invalidate();
        }
    }
}

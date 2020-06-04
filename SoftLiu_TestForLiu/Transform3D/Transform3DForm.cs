using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_TestForLiu.Transform3D
{
    public partial class Transform3DForm : Form
    {
        Triangle3D m_t;

        Matrix4x4 m_scale;

        Matrix4x4 m_rotate;

        int m_angle = 0;

        Matrix4x4 m_view;

        Matrix4x4 m_projection;

        public Transform3DForm()
        {
            InitializeComponent();
        }

        private void Transform3DForm_Load(object sender, EventArgs e)
        {
            m_scale = new Matrix4x4();
            m_scale[1, 1] = 250;
            m_scale[2, 2] = 250;
            m_scale[3, 3] = 250;
            m_scale[4, 4] = 1;

            m_rotate = new Matrix4x4();

            m_view = new Matrix4x4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = 1;

            m_view[4, 4] = 1;

            m_projection = new Matrix4x4();
            m_projection[1, 1] = 1;
            m_projection[2, 2] = 1;
            m_projection[3, 3] = 1;
            m_projection[3, 4] = 1.0f / 250;
            m_projection[4, 4] = 1;

            Vector4 a = new Vector4(0, -0.5f, 0, 1);
            Vector4 b = new Vector4(0.5, 0.5f, 0, 1);
            Vector4 c = new Vector4(-0.5, 0.5f, 0, 1);
            m_t = new Triangle3D(a, b, c);
            m_t.Transform(m_scale);
        }

        private void Transform3DForm_Paint(object sender, PaintEventArgs e)
        {
            m_t.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double angle = (m_angle++) / 360.0f * Math.PI;

            m_rotate[1, 1] = Math.Cos(angle);
            m_rotate[1, 3] = Math.Sin(angle);
            m_rotate[2, 2] = 1;
            m_rotate[3, 1] = -Math.Sin(angle);
            m_rotate[3, 3] = Math.Cos(angle);
            m_rotate[4, 4] = 1;

            // 模型到世界
            Matrix4x4 m = m_scale.Mul(m_rotate);
            // 世界到相机
            Matrix4x4 mv = m.Mul(m_view);
            // 相机到投影
            Matrix4x4 mvp = mv.Mul(m_projection);

            m_t.Transform(mvp);

            this.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_view[4, 3] = (sender as TrackBar).Value;
        }
    }
}

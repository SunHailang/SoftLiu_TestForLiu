using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_TestForLiu.Transform3D
{
    public partial class Transform3DForm : Form
    {
        Triangle3D m_t;
        Cube m_cube;

        Matrix4x4 m_scale;

        Matrix4x4 m_rotateX;
        Matrix4x4 m_rotateY;
        Matrix4x4 m_rotateZ;

        int m_angle = 0;

        Matrix4x4 m_view;

        Matrix4x4 m_projection;

        public Transform3DForm()
        {
            InitializeComponent();
            //Bsort();
            //Qsort();
            Isort();
        }
        /// <summary>
        /// 冒泡排序
        /// </summary>
        private static void Bsort()
        {
            int[] numbs = new int[] { 9, 8, 6, 1, 7, 5, 2, 0 };

            // 比较 多少轮 numbs.Length - 1
            for (int i = 0; i < numbs.Length - 1; i++)
            {
                // 后面比较好的 就不需要比较了
                for (int j = 0; j < numbs.Length - 1 - i; j++)
                {
                    if (numbs[j] > numbs[j + 1])
                    {
                        int temp = numbs[j];
                        numbs[j] = numbs[j + 1];
                        numbs[j + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < numbs.Length; i++)
            {
                Console.Write(numbs[i] + " ");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 快速排序  递归的原理
        /// </summary>
        private static void Qsort()
        {
            int[] nums = new int[] { 6, 10, -1, 4, 9, 8, 6, 1, 7, 5, 2, 0 };

            QsortY(nums, 0, nums.Length - 1);

            for (int i = 0; i < nums.Length; i++)
            {
                Console.Write(nums[i] + " ");
            }
            Console.WriteLine();
        }
        private static void QsortY(int[] nums, int left, int right)
        {
            if (left >= right) return;

            int baseValue = nums[left];
            int low = left;
            int high = right;

            while (low < high)
            {
                while (low < high && baseValue <= nums[high])
                {
                    // 右边的数字 比基数小
                    high--;
                }
                nums[low] = nums[high];
                while (low < high && nums[low] <= baseValue)
                {
                    low++;
                }
                nums[high] = nums[low];
            }
            nums[low] = baseValue;
            QsortY(nums, left, low);
            QsortY(nums, low + 1, right);
        }
        /// <summary>
        /// 插入排序
        /// </summary>
        private static void Isort()
        {
            int[] nums = new int[] { 6, 10, -1, 4, 9, 8, 6, 1, 7, 5, 2, 0 };

            // 遍历所有的数字
            for (int i = 1; i < nums.Length; i++)
            {
                // 判断当前数字比前一个数字小
                if (nums[i] < nums[i + 1])
                {
                    // 把当前数字存起来
                    int temp = nums[i];
                    for (int j = i - 1; j >= 0; j--)
                    {

                        nums[j - 1] = nums[j];
                        nums[j] = temp;
                    }
                }
            }

            for (int i = 0; i < nums.Length; i++)
            {
                Console.Write(nums[i] + " ");
            }
            Console.WriteLine();
        }
        /// <summary>
        ///  防止闪屏 
        /// </summary> 
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void Transform3DForm_Load(object sender, EventArgs e)
        {
            m_scale = new Matrix4x4();
            m_scale[1, 1] = 250;
            m_scale[2, 2] = 250;
            m_scale[3, 3] = 250;
            m_scale[4, 4] = 1;

            m_rotateX = new Matrix4x4();
            m_rotateY = new Matrix4x4();
            m_rotateZ = new Matrix4x4();

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

            //Vector4 a = new Vector4(0, 0.5f, 0, 1);
            //Vector4 b = new Vector4(0.5, -0.5f, 0, 1);
            //Vector4 c = new Vector4(-0.5, -0.5f, 0, 1);
            //m_t = new Triangle3D(a, b, c);

            m_cube = new Cube();
        }

        private void Transform3DForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(300, 300);
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            //m_t.Draw(e.Graphics);
            m_cube.Draw(e.Graphics, checkBox4.Checked);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double angle = (m_angle++) / 360.0f * Math.PI;
            // ============  X
            m_rotateX[1, 1] = 1;
            m_rotateX[2, 2] = Math.Cos(angle);
            m_rotateX[2, 3] = Math.Sin(angle);
            m_rotateX[3, 2] = -Math.Sin(angle);
            m_rotateX[3, 3] = Math.Cos(angle);
            m_rotateX[4, 4] = 1;
            // ============  Y
            m_rotateY[1, 1] = Math.Cos(angle);
            m_rotateY[1, 3] = Math.Sin(angle);
            m_rotateY[2, 2] = 1;
            m_rotateY[3, 1] = -Math.Sin(angle);
            m_rotateY[3, 3] = Math.Cos(angle);
            m_rotateY[4, 4] = 1;
            // ============  Z
            m_rotateZ[1, 1] = Math.Cos(angle);
            m_rotateZ[1, 2] = Math.Sin(angle);
            m_rotateZ[2, 1] = -Math.Sin(angle);
            m_rotateZ[2, 2] = Math.Cos(angle);
            m_rotateZ[3, 3] = 1;
            m_rotateZ[4, 4] = 1;
            //===============================

            /*
             矩阵乘以矩阵的转置 即为撤销 矩阵的变换
             */
            // ==== X
            if (checkBox1.Checked)
            {
                Matrix4x4 tx = m_rotateX.Transpose();
                m_rotateX = m_rotateX.Mul(tx);
            }
            // ==== Y
            if (checkBox2.Checked)
            {
                Matrix4x4 ty = m_rotateY.Transpose();
                m_rotateY = m_rotateY.Mul(ty);
            }
            // ==== Z
            if (checkBox3.Checked)
            {
                Matrix4x4 tz = m_rotateZ.Transpose();
                m_rotateZ = m_rotateZ.Mul(tz);
            }
            Matrix4x4 mAll = m_rotateX.Mul(m_rotateY.Mul(m_rotateZ));
            // 模型到世界
            Matrix4x4 m = m_scale.Mul(mAll);
            Vector4 L = new Vector4(-1, 1, -1, 0);
            //m_t.CalculateLighting(m, L);
            m_cube.CalculateLighting(m, L);

            // 世界到相机
            Matrix4x4 mv = m.Mul(m_view);
            // 相机到投影
            Matrix4x4 mvp = mv.Mul(m_projection);

            //m_t.Transform(mvp);
            m_cube.Transform(mvp);

            this.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_view[4, 3] = (sender as TrackBar).Value;
        }
    }
}

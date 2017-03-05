using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DDA_Line_algorithm
{
    public partial class Form1 : Form
    {
        public Bitmap bmp;
        public Bitmap pixel;
        public Graphics g;
        Thread t;
        delegate void PixelFunc(int x, int y, Color c);

        private void Init()
        {
            if (g != null) g.Dispose();
            pictureBox1.Image = null;
            if (bmp != null) bmp.Dispose();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            if (pixel != null) pixel.Dispose();
            pixel = new Bitmap(1, 1);
            pictureBox1.Image = bmp;
            g = pictureBox1.CreateGraphics();
        }

        private void SetPixel(int x, int y, Color c)
        {
            lock (g)
            {
                pixel.SetPixel(0, 0, Color.Blue);
                g.DrawImageUnscaled(pixel, x, y);
                bmp.SetPixel(x, y, Color.Blue);
            }
        }

        private void Render()
        {

            int xInitial = 10, yInitial = 20, xFinal = 150, yFinal = 200;

            int dx = xFinal - xInitial, dy = yFinal - yInitial, steps, k, xf, yf;

            float xIncrement, yIncrement, x = xInitial, y = yInitial;

            if (Math.Abs(dx) > Math.Abs(dy)) steps = Math.Abs(dx);

            else steps = Math.Abs(dy);
            xIncrement = dx / (float)steps;
            yIncrement = dy / (float)steps;
            PixelFunc func = new PixelFunc(SetPixel);
            for (k = 0; k < steps; k++)
            {
                x += xIncrement;
                xf = (int)x;
                y += yIncrement;
                yf = (int)y;
                try
                {
                    pictureBox1.Invoke(func, xf, yf, Color.Blue);
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
            if (t != null && t.IsAlive) t.Abort();
            t = new Thread(new ThreadStart(Render));
            t.Start();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            g.Dispose();
            bmp.Dispose();
            pixel.Dispose();
        }
    }
}

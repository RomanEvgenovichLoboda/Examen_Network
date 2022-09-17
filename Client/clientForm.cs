using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class clientForm : Form
    {
        bool left, right, up, down;


        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public clientForm()
        {
            InitializeComponent();
        }

        private void clientForm_Load(object sender, EventArgs e)
        {
            clientSocket.Connect(iPEnd);
        }

        private void clientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        private void clientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==(char)Keys.Left)
            {
                left = true;
                right = up = down = false;
                pictureBox1.Image = global::Client.Properties.Resources.tank_left;
                if (pictureBox1.Location.X > 0 )
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y);
                }
            }
            else if(e.KeyValue==(char)Keys.Right)
            {
                right = true;
                left = up = down = false;
                pictureBox1.Image = global::Client.Properties.Resources.tank_right;
                if (pictureBox1.Location.X <= this.Width - 50)
                {
                    
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y);
                }
            }
            else if(e.KeyValue==(char)Keys.Up)
            {
                up = true;
                right = left = down = false;
                pictureBox1.Image = global::Client.Properties.Resources.tank_up;
                if (pictureBox1.Location.Y > 0)
                {
                    
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 5);
                }
            }
            else if(e.KeyValue==(char)Keys.Down)
            {
                down = true;
                right = up = left = false;
                pictureBox1.Image = Properties.Resources.tank_down;
                if (pictureBox1.Location.Y <= this.Height - 50)
                {
                    
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 5);
                }
            }
            else if (e.KeyValue == (char)Keys.Space)
            {
                Bullet_Run();
                //if (true)
                //{
                //    PictureBox bullet = new PictureBox();
                //    bullet.Size = new Size(50, 50);
                //    bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                //    bullet.Image = Properties.Resources.bullet_up;
                //    bullet.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y);
                //}
            }
        }
        void Bullet_Run()
        {
           Invoke(new Action(async () =>
           {
               PictureBox bullet = new PictureBox();
               bullet.Size = new Size(10, 10);
               bullet.SizeMode = PictureBoxSizeMode.StretchImage;
               bullet.Image = Properties.Resources.bullet_up;
               
               if (up)
               {
                   bullet.Image = Properties.Resources.bullet_up;
                   bullet.Location = new Point(pictureBox1.Location.X + 12, pictureBox1.Location.Y);
                   Controls.Add(bullet);
                   for (int i = pictureBox1.Location.Y; i > 0; i--)
                   {
                       bullet.Location = new Point(bullet.Location.X, i);
                       await Task.Delay(10);
                   }
               }

               else if (down)
               {
                   bullet.Image = Properties.Resources.bullet_down;
                   bullet.Location = new Point(pictureBox1.Location.X + 12, pictureBox1.Location.Y);
                   Controls.Add(bullet);
                   for (int i = pictureBox1.Location.Y; i < this.Height - 10; i++)
                   {
                       bullet.Location = new Point(bullet.Location.X, i);
                       await Task.Delay(10);
                   }
               }

               else if (left)
               {
                   bullet.Image = Properties.Resources.bullet_left;
                   bullet.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 12);
                   Controls.Add(bullet);
                   for (int i = pictureBox1.Location.X; i > 0; i--)
                   {
                       bullet.Location = new Point(i,bullet.Location.Y);
                       await Task.Delay(10);
                   }
               }

               else if (right)
               {
                   bullet.Image = Properties.Resources.bullet_right;
                   bullet.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 12);
                   Controls.Add(bullet);
                   for (int i = pictureBox1.Location.X; i < this.Width - 10; i++)
                   {
                       bullet.Location = new Point(i, bullet.Location.Y);
                       await Task.Delay(10);
                   }
                   
               }
               Controls.Remove(bullet);
           })); 
        }
    }
}

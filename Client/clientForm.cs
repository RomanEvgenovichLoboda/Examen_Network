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
using Client.Model;
using Newtonsoft.Json;

namespace Client
{
    public partial class clientForm : Form
    {
        bool left, right, up, down;
        ClientData clientData;

        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public clientForm()
        {
            InitializeComponent();


            clientData = new ClientData(pictureBox1.Location, "tank_up");
            
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
                TankMuves(pictureBox1, "left");
            }
            else if(e.KeyValue==(char)Keys.Right)
            {
                TankMuves(pictureBox1, "right");
               
            }
            else if(e.KeyValue==(char)Keys.Up)
            {
                TankMuves(pictureBox1, "up");
             
            }
            else if(e.KeyValue==(char)Keys.Down)
            {
                TankMuves(pictureBox1, "down");
               
            }
            else if (e.KeyValue == (char)Keys.Space)
            {
                Bullet_Run(pictureBox1);
            }
        }
        void TankMuves(PictureBox tank,string muve = null)
        {
            switch (muve)
            {
                case "up":
                    {
                        up = true;
                        right = left = down = false;
                        tank.Image = Properties.Resources.tank_up;
                        if (tank.Location.Y > 0)
                        {
                            tank.Location = new Point(tank.Location.X, tank.Location.Y - 5);
                        }

                        clientData.ImagePath = "tank_up";
                        clientData.Location = tank.Location;
                        break;
                    }
                case "down":
                    {
                        down = true;
                        right = up = left = false;
                        tank.Image = Properties.Resources.tank_down;
                        if (tank.Location.Y <= this.Height - 50)
                        {
                            tank.Location = new Point(tank.Location.X, tank.Location.Y + 5);
                        }

                        clientData.ImagePath = "tank_down";
                        clientData.Location = tank.Location;
                        break;
                    }
                case "right":
                    {
                        right = true;
                        left = up = down = false;
                        tank.Image = Properties.Resources.tank_right;
                        if (tank.Location.X <= this.Width - 50)
                        {
                            tank.Location = new Point(tank.Location.X + 5, tank.Location.Y);
                        }

                        clientData.ImagePath = "tank_right";
                        clientData.Location = tank.Location;
                        break;
                    }
                case "left":
                    {
                        left = true;
                        right = up = down = false;
                        tank.Image = Properties.Resources.tank_left;
                        if (tank.Location.X > 0)
                        {
                            tank.Location = new Point(tank.Location.X - 5, tank.Location.Y);
                        }

                        clientData.ImagePath = "tank_left";
                        clientData.Location = tank.Location;
                        break;
                    }
                default:
                    break;
            }
            
        }
        void Bullet_Run(PictureBox tank)
        {
           Invoke(new Action(async () =>
           {
               PictureBox bullet = new PictureBox();
               bullet.Size = new Size(10, 10);
               bullet.SizeMode = PictureBoxSizeMode.StretchImage;
               //bullet.Image = Properties.Resources.bullet_up;
               
               if (up)
               {
                   bullet.Image = Properties.Resources.bullet_up;
                   bullet.Location = new Point(tank.Location.X + 12, tank.Location.Y);

//////
                   BulletData bulletData = new BulletData(bullet.Location, "bullet_up");
                   SerilizeToJSON(bulletData);


                   Controls.Add(bullet);


                   //clientData.Bullets.Add(new BulletData(bullet.Location, "bullet_up"));

                   for (int i = tank.Location.Y; i > 0; i--)
                   {

                       bullet.Location = new Point(bullet.Location.X, i);
                       await Task.Delay(10);
                   }
               }

               else if (down)
               {
                   bullet.Image = Properties.Resources.bullet_down;
                   bullet.Location = new Point(tank.Location.X + 12, tank.Location.Y + 20);

///////
                   BulletData bulletData = new BulletData(bullet.Location, "bullet_down");
                   SerilizeToJSON(bulletData);


                   Controls.Add(bullet);
                   for (int i = tank.Location.Y; i < this.Height - 10; i++)
                   {
                       bullet.Location = new Point(bullet.Location.X, i);
                       await Task.Delay(10);
                   }
               }

               else if (left)
               {
                   bullet.Image = Properties.Resources.bullet_left;
                   bullet.Location = new Point(tank.Location.X, tank.Location.Y + 12);

/////
                   BulletData bulletData = new BulletData(bullet.Location, "bullet_left");
                   SerilizeToJSON(bulletData);


                   Controls.Add(bullet);
                   for (int i = tank.Location.X; i > 0; i--)
                   {
                       bullet.Location = new Point(i,bullet.Location.Y);
                       await Task.Delay(10);
                   }
               }

               else if (right)
               {
                   bullet.Image = Properties.Resources.bullet_right;
                   bullet.Location = new Point(tank.Location.X + 20, tank.Location.Y + 12);

 /////
                   BulletData bulletData = new BulletData(bullet.Location, "bullet_right");
                   SerilizeToJSON(bulletData);


                   Controls.Add(bullet);
                   for (int i = tank.Location.X; i < this.Width - 10; i++)
                   {
                       bullet.Location = new Point(i, bullet.Location.Y);
                       await Task.Delay(10);
                   }
                   
               }
               Controls.Remove(bullet);
           })); 
        }
        public async void SerilizeToJSON(object obj)
        {
            string jsonStr = JsonConvert.SerializeObject(obj);
            byte[] data = Encoding.UTF8.GetBytes(jsonStr);
            clientSocket.Send(data);
            await Task.Run( () =>
            {
                int bytes = 0;
                data = new byte[51024];
                StringBuilder builder = new StringBuilder();
                do
                {
                    bytes = clientSocket.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (clientSocket.Available > 0);
                listBox1.Items.Add(builder.ToString());
                //await Task.Delay(10);
            });

        }
    }
}

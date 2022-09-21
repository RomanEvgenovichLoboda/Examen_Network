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
//using Newtonsoft.Json;
//using System.Text.Json.Serialization;
using System.Text.Json;

namespace Client
{
    public partial class clientForm : Form
    {
        //bool left, right, up, down;
        ClientData clientData;

        PictureBox tankEnemy;


        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public clientForm()
        {
            InitializeComponent();


            clientData = new ClientData(pictureBox1.Location, "tank_up.png");
            clientData.BulletMove = "up";
            DoubleBuffered = true;



           
        }

        private void clientForm_Load(object sender, EventArgs e)
        {
            clientSocket.Connect(iPEnd);
            ClientListen();
        }

        private void clientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        private void clientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Left)
            {

                //TankMuves(pictureBox1, "left");
                //left = true;
                //right = up = down = false;
                pictureBox1.Image = Image.FromFile("tank_left.png");//Properties.Resources.tank_left;
                if (pictureBox1.Location.X > 0)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y);
                }

                clientData.ImagePath = "tank_left.png";
                clientData.Location = pictureBox1.Location;
                clientData.BulletMove = "left";
                clientData.IsBullet = false;
                SendData();
            }
            else if (e.KeyValue == (char)Keys.Right)
            {
                //TankMuves(pictureBox1, "right");
                //right = true;
                //left = up = down = false;
                pictureBox1.Image = Image.FromFile("tank_right.png"); //Properties.Resources.tank_right;
                if (pictureBox1.Location.X <= this.Width - 50)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y);
                }

                clientData.ImagePath = "tank_right.png";
                clientData.Location = pictureBox1.Location;
                clientData.BulletMove = "right";
                clientData.IsBullet = false;
                SendData();
            }
            else if (e.KeyValue == (char)Keys.Up)
            {
                //TankMuves(pictureBox1, "up");
                //up = true;
                //right = left = down = false;
                pictureBox1.Image = Image.FromFile("tank_up.png"); //Properties.Resources.tank_up;
                if (pictureBox1.Location.Y > 0)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 5);
                }

                clientData.ImagePath = "tank_up.png";
                clientData.Location = pictureBox1.Location;
                clientData.BulletMove = "up";
                clientData.IsBullet = false;
                SendData();
            }
            else if (e.KeyValue == (char)Keys.Down)
            {
                //TankMuves(pictureBox1, "down");
                //down = true;
                //right = up = left = false;
                pictureBox1.Image = Image.FromFile("tank_down.png");  //Properties.Resources.tank_down;
                if (pictureBox1.Location.Y <= this.Height - 50)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 5);
                }

                clientData.ImagePath = "tank_down.png";
                clientData.Location = pictureBox1.Location;
                clientData.BulletMove = "down";
                clientData.IsBullet = false;
                SendData();
            }
            else if (e.KeyValue == (char)Keys.Space)
            {
                clientData.IsBullet = true;
                Bullet_Run(pictureBox1,clientData);
                SendData();

                //BulletData bulletData = new BulletData(pictureBox1.Location, "bullet_up.png","up");
                //SerilizeToJSON(bulletData);
            }
        }
      
        void Bullet_Run(PictureBox tank, ClientData someClient)
        {
            Invoke(new Action(async () =>
            {
                PictureBox bullet = new PictureBox();
                bullet.Size = new Size(10, 10);
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                //bullet.Image = Properties.Resources.bullet_up;

                if  (someClient.BulletMove.Equals("up")) //(up)//(tank.Image == Image.FromFile("tank_up.png")) //
                {
                    bullet.Image = Properties.Resources.bullet_up;
                    bullet.Location = new Point(tank.Location.X + 12, tank.Location.Y);

                    //////
                    //if (!is_enemy)
                    //{
                    //    clientData.Bullet.ImagePath = "bullet_up.png";
                    //    //clientData.Bullet.Move = "up";
                    //    clientData.IsBullet = true;
                    //    //BulletData bulletData = new BulletData(bullet.Location, "bullet_up.png", "up");
                    //    SendData();

                    //    clientData.IsBullet = false;
                    //}



                    Controls.Add(bullet);


                    //clientData.Bullets.Add(new BulletData(bullet.Location, "bullet_up"));

                    for (int i = tank.Location.Y; i > 0; i--)
                    {

                        bullet.Location = new Point(bullet.Location.X, i);
                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("down"))//(down)//(tank.Image == Image.FromFile("tank_down.png")) //
                {
                    bullet.Image = Properties.Resources.bullet_down;
                    bullet.Location = new Point(tank.Location.X + 12, tank.Location.Y + 20);

                    ///////
                    //if (!is_enemy)
                    //{
                    //    clientData.Bullet.ImagePath = "bullet_down.png";
                    //    //clientData.Bullet.Move = "down";
                    //    clientData.IsBullet = true;
                    //    //BulletData bulletData = new BulletData(bullet.Location, "bullet_down.png", "down");
                    //    SendData();

                    //    clientData.IsBullet = false;
                    //}



                    Controls.Add(bullet);
                    for (int i = tank.Location.Y; i < this.Height - 10; i++)
                    {
                        bullet.Location = new Point(bullet.Location.X, i);
                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("left")) //(left)//(tank.Image == Image.FromFile("tank_left.png"))  //
                {
                    bullet.Image = Properties.Resources.bullet_left;
                    bullet.Location = new Point(tank.Location.X, tank.Location.Y + 12);

                    /////
                    //if (!is_enemy)
                    //{
                    //    clientData.Bullet.ImagePath = "bullet_left.png";
                    //    //clientData.Bullet.Move = "left";
                    //    clientData.IsBullet = true;
                    //    //BulletData bulletData = new BulletData(bullet.Location, "bullet_left.png", "left");
                    //    SendData();

                        
                    //    clientData.IsBullet = false;
                    //}



                    Controls.Add(bullet);
                    for (int i = tank.Location.X; i > 0; i--)
                    {
                        bullet.Location = new Point(i, bullet.Location.Y);
                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("right")) //(right)//(tank.Image == Image.FromFile("tank_right.png"))  //
                {
                    bullet.Image = Properties.Resources.bullet_right;
                    bullet.Location = new Point(tank.Location.X + 20, tank.Location.Y + 12);

                    /////
                    //if (!is_enemy)
                    //{
                    //    clientData.Bullet.ImagePath = "bullet_right.png";
                    //    //clientData.Bullet.Move = "left";
                    //    clientData.IsBullet = true;
                    //    //BulletData bulletData = new BulletData(bullet.Location, "bullet_right.png", "right");
                    //    SendData();
                    //}



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
        public  void SendData()
        {
            string jsonStr = JsonSerializer.Serialize(clientData); //JsonConvert.SerializeObject(clientData);
            byte[] data = Encoding.UTF8.GetBytes(jsonStr);


            listBox1.Items.Add(jsonStr);
            var enemyD = JsonSerializer.Deserialize<ClientData>(jsonStr);
            listBox1.Items.Add(enemyD);
            clientSocket.Send(data);
            
        }




        public void ClientListen()
        {
            //ClientData clientData = new ClientData();
            //clientData.Location = pictureBox1.Location;
            //clientData.ImagePath = "tank_up.png";
            //SerilizeToJSON(clientData);
            //clientSocket.Connect(iPEnd);
            Task.Run(() =>
            {
                do
                {
                    int bytes = 0;
                    byte[] data = new byte[51024];
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        //BeginInvoke(new Action(() => bytes = clientSocket.Receive(data)));
                        bytes = clientSocket.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (clientSocket.Available > 0);
                    Invoke(new Action(() => listBox1.Items.Add(builder.ToString())));

                    try
                    {
                        string temp = builder.ToString();
                        var enemyD = JsonSerializer.Deserialize<ClientData>(temp);  //JsonConvert.DeserializeObject<ClientData>(temp);

                        if(!enemyD.IsBullet)
                        {
                            if (tankEnemy == null)
                            {
                                tankEnemy = new PictureBox();
                                tankEnemy.Size = pictureBox1.Size;
                                tankEnemy.SizeMode = PictureBoxSizeMode.StretchImage;
                                Invoke(new Action(() => Controls.Add(tankEnemy)));

                            }
                            tankEnemy.Location = enemyD.Location;
                            tankEnemy.Image = Image.FromFile(enemyD.ImagePath);
                        }
                        else Bullet_Run(tankEnemy,enemyD);
                        
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                        MessageBox.Show(ex.Message.ToString());
                    }
                    



                } while (true);



            });
        }
    }
}
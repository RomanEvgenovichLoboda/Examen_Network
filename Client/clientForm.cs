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
using System.Text.Json;
using Client.View;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class clientForm : Form
    {
        //bool left, right, up, down;
        ClientData clientData;



        List<PictureBox> enemys = new List<PictureBox>();
        //PictureBox tankEnemy;


        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public clientForm()
        {
            InitializeComponent();


            clientData = new ClientData(pictureBox1.Location, "tank_up.png");
            clientData.BulletMove = "up";
            clientData.Location = pictureBox1.Location;   
            clientData.IsBullet = false;
                

            DoubleBuffered = true;
            RegistrForm rform = new RegistrForm(clientData);
            rform.ShowDialog();
            Text = clientData.Name;


            clientSocket.Connect(iPEnd);
            ClientListen();
            SendData();
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

                
                pictureBox1.Image = Properties.Resources.tank_left;//Image.FromFile("tank_left.png");//
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
                
                pictureBox1.Image = Properties.Resources.tank_right;//Image.FromFile("tank_right.png"); //
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
               
                pictureBox1.Image = Properties.Resources.tank_up;//Image.FromFile("tank_up.png"); //
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
                
                pictureBox1.Image = Properties.Resources.tank_down;//Image.FromFile("tank_down.png");  //
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

            }
        }
      
        void Bullet_Run(PictureBox tank, ClientData someClient)
        {
            Invoke(new Action(async () =>
            {
                PictureBox bullet = new PictureBox();
                bullet.Size = new Size(7, 7);
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                //bullet.Image = Properties.Resources.bullet_up;

                if  (someClient.BulletMove.Equals("up")) //(up)//(tank.Image == Image.FromFile("tank_up.png")) //
                {
                    bullet.Image = Properties.Resources.bullet_up;
                    bullet.Location = new Point(tank.Location.X + tank.Width/2 - bullet.Width/2, tank.Location.Y - 2);

                    Controls.Add(bullet);
                    //clientData.Bullets.Add(new BulletData(bullet.Location, "bullet_up"));

                    for (int i = bullet.Location.Y; i > 0; i--)
                    {

                        bullet.Location = new Point(bullet.Location.X, i);

                        if (bullet.Location.X >= pictureBox1.Location.X && bullet.Location.X <= pictureBox1.Location.X + pictureBox1.Width && bullet.Location.Y >= pictureBox1.Location.Y && bullet.Location.Y <= pictureBox1.Location.Y+pictureBox1.Height)
                        {
                            pictureBox1.Location = new Point(0, 0);
                            clientData.IsBullet = false;
                            clientData.Location = pictureBox1.Location;
                            SendData();
                        }

                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("down"))//(down)//(tank.Image == Image.FromFile("tank_down.png")) //
                {
                    bullet.Image = Properties.Resources.bullet_down;
                    bullet.Location = new Point(tank.Location.X + tank.Width/ 2 - bullet.Width/2, tank.Location.Y + tank.Height + 2);

                    

                    Controls.Add(bullet);
                    for (int i = bullet.Location.Y; i < this.Height - 10; i++)
                    {
                        bullet.Location = new Point(bullet.Location.X, i);
                        if (bullet.Location.X >= pictureBox1.Location.X && bullet.Location.X <= pictureBox1.Location.X + pictureBox1.Width && bullet.Location.Y >= pictureBox1.Location.Y && bullet.Location.Y <= pictureBox1.Location.Y + pictureBox1.Height)
                        {
                            pictureBox1.Location = new Point(0, 0);
                            clientData.IsBullet = false;
                            clientData.Location = pictureBox1.Location;
                            SendData();
                        }
                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("left")) //(left)//(tank.Image == Image.FromFile("tank_left.png"))  //
                {
                    bullet.Image = Properties.Resources.bullet_left;
                    bullet.Location = new Point(tank.Location.X - 2, tank.Location.Y + tank.Height/ 2 - bullet.Height/2);

                   
                    Controls.Add(bullet);
                    for (int i = bullet.Location.X; i > 0; i--)
                    {
                        bullet.Location = new Point(i, bullet.Location.Y);
                        if (bullet.Location.X >= pictureBox1.Location.X && bullet.Location.X <= pictureBox1.Location.X + pictureBox1.Width && bullet.Location.Y >= pictureBox1.Location.Y && bullet.Location.Y <= pictureBox1.Location.Y + pictureBox1.Height)
                        {
                            pictureBox1.Location = new Point(0, 0);
                            clientData.IsBullet = false;
                            clientData.Location = pictureBox1.Location;
                            SendData();
                        }

                        await Task.Delay(10);
                    }
                }

                else if (someClient.BulletMove.Equals("right")) //(right)//(tank.Image == Image.FromFile("tank_right.png"))  //
                {
                    bullet.Image = Properties.Resources.bullet_right;
                    bullet.Location = new Point(tank.Location.X + tank.Width + 2, tank.Location.Y + tank.Height/ 2 - bullet.Width/2);

                   

                    Controls.Add(bullet);
                    for (int i = bullet.Location.X; i < this.Width - 10; i++)
                    {
                        bullet.Location = new Point(i, bullet.Location.Y);
                        if (bullet.Location.X >= pictureBox1.Location.X && bullet.Location.X <= pictureBox1.Location.X + pictureBox1.Width && bullet.Location.Y >= pictureBox1.Location.Y && bullet.Location.Y <= pictureBox1.Location.Y + pictureBox1.Height)
                        {
                            pictureBox1.Location = new Point(0, 0);
                            clientData.IsBullet = false;
                            clientData.Location = pictureBox1.Location;
                            SendData();
                        }
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


            //listBox1.Items.Add(jsonStr);
            //var enemyD = JsonSerializer.Deserialize<ClientData>(jsonStr);
            //listBox1.Items.Add(enemyD);
            clientSocket.Send(data);
            
        }


        public void ClientListen()
        {
            
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
                    //Invoke(new Action(() => listBox1.Items.Add(builder.ToString())));

                    try
                    {
                        string temp = builder.ToString();
                        var enemyD = JsonSerializer.Deserialize<ClientData>(temp);  //JsonConvert.DeserializeObject<ClientData>(temp);

                       
                        if (!enemyD.IsBullet)
                        {
                            if (enemys.Count == 0)
                            {
                                PictureBox enemyBox = new PictureBox();
                                enemyBox.Size = pictureBox1.Size;
                                enemyBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                enemyBox.Name = enemyD.Name;
                                enemyBox.Location = enemyD.Location;
                                enemyBox.Image = Image.FromFile(enemyD.ImagePath);
                                lock (enemys)
                                {
                                    enemys.Add(enemyBox);
                                }
                                Invoke(new Action(() => Controls.Add(enemyBox)));
                            }
                            else
                            {
                                bool is_in = false;
                                foreach (var item in enemys)
                                {
                                    if (item.Name == enemyD.Name)
                                    {
                                        item.Image = Image.FromFile(enemyD.ImagePath);
                                        item.Location = enemyD.Location;
                                        is_in = true;
                                    }
                                }
                                if (!is_in)
                                {
                                    PictureBox enemyBox = new PictureBox();
                                    enemyBox.Size = pictureBox1.Size;
                                    enemyBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                    enemyBox.Name = enemyD.Name;
                                    enemyBox.Location = enemyD.Location;
                                    enemyBox.Image = Image.FromFile(enemyD.ImagePath);
                                    lock (enemys)
                                    {
                                        enemys.Add(enemyBox);
                                    }
                                    Invoke(new Action(() => Controls.Add(enemyBox)));
                                }
                            }

                        }
                        else
                        {
                            foreach (var item in enemys)
                            {
                                if (item.Name == enemyD.Name)
                                {
                                    Bullet_Run(item, enemyD);
                                }
                            }
                        }

                            
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
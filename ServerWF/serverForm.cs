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

namespace ServerWF
{
    public partial class serverForm : System.Windows.Forms.Form
    {
        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clients = new List<Socket>();
        //Socket clientSocket;
        public serverForm()
        {
            InitializeComponent();
            Listen();
        }
        async void Listen()
        {
            int counter = 1;
            await Task.Run(async () => {
                try
                {
                    serverSocket.Bind(iPEnd);
                    serverSocket.Listen(10);
                    label1.Invoke(new Action(async () => { label1.Text = $"Server listen on {IP}:{PORT}"; }));
                    int bytes = 2;

                    while (true)
                    {
                        await Task.Run(() => {

                        //counter++;
                        
                        //MessageBox.Show(counter.ToString());
                        Socket clientSocket = serverSocket.Accept();
                            lock(clients)
                            {
                                clients.Add(clientSocket);
                                //MessageBox.Show("ok!");
                            }
                        Invoke(new Action(() => label2.Text = counter++.ToString()));
                            Task.Run(() =>
                            {
                                do
                                {
                                    bytes = 0;
                                    byte[] buffer = new byte[51024];
                                    StringBuilder builder = new StringBuilder();
                                    do
                                    {
                                        bytes = clientSocket.Receive(buffer);
                                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                                    } while (clientSocket.Available > 0);
                                    listBox1.Items.Add(builder.ToString());
                                    /////send
                                    byte[] data = Encoding.Unicode.GetBytes(builder.ToString());
                                    foreach (Socket item in clients)
                                    {
                                        if(item!= clientSocket)item.Send(data);
                                    }


                                } while (bytes>0);
                                
                                clientSocket?.Close();
                            });

                        });
                        //clientSocket?.Close();
                    }
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket?.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                label1.Invoke(new Action(async () => { label1.Text = "Server end..."; }));

            });
        }
        private void serverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (clients)
            {
                foreach (Socket item in clients)
                {
                    item.Close();
                }
            }
                //clientSocket?.Close();
                //serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket?.Close();
        }
    }
}

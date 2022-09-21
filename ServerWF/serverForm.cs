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
using Client;
using Client.Model;
using System.Text.Json;

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
                    label1.Invoke(new Action(() => { label1.Text = $"Server listen on {IP}:{PORT}"; }));
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
                                    byte[] buffer = new byte[1024];
                                    StringBuilder builder = new StringBuilder();
                                    do
                                    {
                                        bytes = clientSocket.Receive(buffer);
                                        builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                                    } while (clientSocket.Available > 0);
                                    Invoke(new Action(() => listBox1.Items.Add(builder.ToString())));
                                    //var serializeData = JsonSerializer.Deserialize<ClientData>(builder.ToString());
                                    //Invoke(new Action(() => listBox1.Items.Add(serializeData)));
                                    /////send
                                    byte[] data = Encoding.Unicode.GetBytes(builder.ToString());
                                    foreach (Socket item in clients)
                                    {
                                        if(item!= clientSocket)item.Send(data);
                                    }


                                } while (bytes>0);
                                
                                clientSocket?.Close();
                                lock (clients)
                                {
                                    foreach (var item in clients)
                                    {
                                        if (item == clientSocket) clients.Remove(item);
                                    }
                                    
                                    //MessageBox.Show("ok!");
                                }
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
                label1.Invoke(new Action(() => { label1.Text = "Server end..."; }));

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

using System.Net.Sockets;
using System.Net;

namespace Examen_Network
{
    public partial class serverForm : Form
    {
        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket clientSocket;
        public serverForm()
        {
            InitializeComponent();
        }
        async void Listen()
        {
            await Task.Run(() => {
                try
                {
                    serverSocket.Bind(iPEnd);
                    serverSocket.Listen(10);
                    clientSocket = serverSocket.Accept();
                    int bytes = 2;
                    do
                    {
                        
                        label1.Text = $"Server listen on {IP}:{PORT}";
                        bytes = 0;
                        byte[] buffer = new byte[51024];
                        do
                        {
                            bytes = clientSocket.Receive(buffer);
                        } while (clientSocket.Available > 0);
                        string str = $"message__{DateTime.Now.ToString().Replace('.', '_').Replace(':', '_')}.wav";
                        if (bytes > 0)
                        {
                            File.WriteAllBytes(str, buffer);
                            //listBox1.Items.Add(str);

                        }
                    } while (bytes != 0);
                    clientSocket?.Close();
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket?.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                label1.Text = "Server end...";
            });
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket?.Close();
            //serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket?.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
using Client.Model;
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
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Client.View
{
    public partial class RegistrForm : Form
    {
        ClientData clData;
    //
        const int PORT = 8088;
        const string IP = "127.0.0.1";
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public bool is_Ok;

    //
        public RegistrForm()
        {
            InitializeComponent();
        }
        public RegistrForm(ClientData c_data)
        {
            InitializeComponent();
            clData = c_data;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //if (textBox_Login.Text != "")
            //{
            //    clData.Name = textBox_Login.Text;
            //    this.Close();
            //}
            try
            {

                if (textBox_Login.Text != "" && textBox_Password.Text != "")
                {
                    // ClientData user_data;
                    if (checkBox1.Checked) clData.IsRegistration = true;


                    clData.IsAutorisation = true;
                    clData.Name = textBox_Login.Text;
                    clData.Pssw = textBox_Password.Text;

                    string str = JsonSerializer.Serialize(clData);
                    byte[] data = Encoding.Unicode.GetBytes(str);
                    clientSocket.Send(data);
                    clData.IsRegistration = false;
                    clData.IsAutorisation = false;

                    int bytes = 0;
                    byte[] buffer = new byte[1024];
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        bytes = clientSocket.Receive(buffer);
                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    } while (clientSocket.Available > 0);
                    MessageBox.Show(builder.ToString());
                    if (builder.ToString() == "Welcome =)")
                    {
                        clData.Name = textBox_Login.Text;
                        is_Ok = true;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void RegistrForm_Load(object sender, EventArgs e)
        {
            clientSocket.Connect(iPEnd);
        }

        private void RegistrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}

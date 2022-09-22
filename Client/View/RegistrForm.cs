using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.View
{
    public partial class RegistrForm : Form
    {
        ClientData clData;
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
            if(textBox1.Text != "")
            {
                clData.Name = textBox1.Text;
                this.Close();
            }
        }
    }
}

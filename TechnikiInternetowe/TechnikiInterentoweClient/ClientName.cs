using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnikiInterentoweClient
{
    public partial class ClientName : Form
    {
        private string client_name;
        public ClientName()
        {
            InitializeComponent();
        }

        public string getClientName()
        {
            return this.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
                return;

            this.DialogResult = DialogResult.OK;
        }

        private void ClientName_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
        }
    }
}

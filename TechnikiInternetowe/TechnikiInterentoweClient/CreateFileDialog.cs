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
    public partial class CreateFileDialog : Form
    {
        public CreateFileDialog()
        {
            InitializeComponent();
        }

        public string getFileNameToCreate()
        {
            return this.fileName.Text;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}

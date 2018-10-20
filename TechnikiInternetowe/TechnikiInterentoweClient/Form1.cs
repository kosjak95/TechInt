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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region UI Event Hander
        private void btnSendReq_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/Hello/" + txtRestRequestURL.Text;

            setOutput("Client Created!");

            string strResponse = rClient.makeRequest();

            setOutput(strResponse);
        }

        #endregion

        private void setOutput(string requestText)
        {
            try
            {
                System.Diagnostics.Debug.Write(requestText + Environment.NewLine);
                txtRestResponse.Text = requestText + Environment.NewLine;
                txtRestResponse.SelectionStart = txtRestResponse.TextLength;
                txtRestResponse.ScrollToCaret();
            }catch(Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
        }

    }
}

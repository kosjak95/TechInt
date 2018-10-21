﻿using System;
using System.Windows.Forms;

namespace TechnikiInterentoweClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            filesList.View = View.Details;
            filesList.Columns.Add("File Name");
            filesList.Columns.Add("Updated");
            filesList.GridLines = true;

            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/Files/";
            string strResponse = rClient.makeRequest();

            UpdateFilesList(strResponse);
        }

        /// <summary>
        /// Decode files' names from raw data, after request  
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private string[] DecodeFilesNames(string rawData)
        {
            rawData = rawData.Substring(2, rawData.Length - 4);
            return rawData.Split(new string[] { "\",\"" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Update list view with files name
        /// </summary>
        /// <param name="strResponse"></param>
        private void UpdateFilesList(string strResponse)
        {
            var filesNames = DecodeFilesNames(strResponse);

            foreach (string fileName in filesNames)
            {
                filesList.Items.Add(new ListViewItem(fileName));
            }
        }

        #region UI Event Hander
        private void btnSendReq_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/Hello/" + txtRestRequestURL.Text;

            string strResponse = rClient.makeRequest();

            SetOutput(strResponse);
        }

        #endregion

        private void SetOutput(string requestText)
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

        private void txtRestResponse_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRestRequestURL_TextChanged(object sender, EventArgs e)
        {

        }

        private void filesList_Click(object sender, EventArgs e)
        {
            int i = filesList.SelectedIndices[0];
            string s = filesList.Items[i].Text;
            MessageBox.Show(s);
        }
    }
}

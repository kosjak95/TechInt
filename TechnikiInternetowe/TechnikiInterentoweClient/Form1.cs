﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
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

        private void SetOutput(string requestText)
        {
            try
            {
                System.Diagnostics.Debug.Write(requestText + Environment.NewLine);
                txtRestResponse.Text = requestText + Environment.NewLine;
                txtRestResponse.SelectionStart = txtRestResponse.TextLength;
                txtRestResponse.ScrollToCaret();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
        }

        #region UI Event Hander
        private void filesList_Click(object sender, EventArgs e)
        {
            int i = filesList.SelectedIndices[0];
            string selectedRow = filesList.Items[i].Text;

            SendReqToServerWithOpen(selectedRow);
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            TabPage tab = (TabPage)((Button)sender).Parent;
            string fileName = tab.Text;
            string content = ((TextBox)tab.Controls[0]).Text;

            //RestClient rClient = new RestClient();
            //rClient.endPoint = "http://localhost:8080/UpdateContent/" + fileName + "/"+content;
            //string strResponse = rClient.makeRequest();
        }

        /// <summary>
        /// Create new tabPage with TextBox
        /// </summary>
        /// <param name="title">title of TabPage</param>
        /// <param name="fileContent">text to set inside TextBox</param>
        private TabPage OpenNewTabPage(string title, string fileContent)
        {
            TabPage tp = new TabPage(title);
            tabs.TabPages.Add(tp);
            tp.Dock = DockStyle.Fill;

            TextBox fileContentTB = new TextBox();
            fileContentTB.Dock = DockStyle.Fill;
            fileContentTB.Location = new System.Drawing.Point(0, Convert.ToInt32(0.2 * Convert.ToInt32(tp.Size.Height.ToString())));
            fileContentTB.Multiline = true;
            fileContentTB.ScrollBars = ScrollBars.Vertical;
            fileContentTB.AcceptsReturn = true;
            fileContentTB.AcceptsTab = true;
            fileContentTB.WordWrap = true;
            tp.Controls.Add(fileContentTB);
            fileContentTB.Text = fileContent;

            Button saveButton = new Button();
            tp.Controls.Add(saveButton);
            saveButton.Click += new EventHandler(SaveButtonOnClick);
            saveButton.Text = "Save";
            saveButton.Dock = DockStyle.Top;

            return tp;
        }

        /// <summary>
        /// Send request to server about read only acces to file
        /// </summary>
        /// <param name="selectedRow"></param
        private void SendReqToServerWithOpen(string fileNameWithoutFormat)
        {
            //TODO: when we add modify date, it can be wrong
            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/OpenFile/" + fileNameWithoutFormat;
            string strResponse = rClient.makeRequest();

            KeyValuePair<int, string> keyValuePair = JsonConvert.DeserializeObject<KeyValuePair<int, string>>(strResponse);

            OpenNewTabPage(fileNameWithoutFormat, keyValuePair.Value);
        }
        #endregion

        private void createNewFileButton_Click(object sender, EventArgs e)
        {
            //RestClient rClient = new RestClient();
            //rClient.endPoint = "http://localhost:8080/TryCreate/" + newFileNameTextBox.Text;
            //string strResponse = rClient.makeRequest();

            OpenNewTabPage(newFileNameTextBox.Text, "");
        }

    }
}
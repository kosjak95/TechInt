using System;
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

        #region UI Event Hander
        private void filesList_Click(object sender, EventArgs e)
        {
            int i = filesList.SelectedIndices[0];
            string selectedRow = filesList.Items[i].Text;

            SendReqToServerWithOpen(selectedRow);
        }

        /// <summary>
        /// Create new tabPage with TextBox
        /// </summary>
        /// <param name="title">title of TabPage</param>
        /// <param name="fileContent">text to set inside TextBox</param>
        private void OpenNewTabPage(string title, string fileContent)
        {
            TabPage tp = new TabPage(title);
            tabs.TabPages.Add(tp);
            TextBox fileContentTB = new TextBox();
            fileContentTB.Dock = DockStyle.Fill;
            tp.Controls.Add(fileContentTB);
            fileContentTB.Text = fileContent;
        }

        /// <summary>
        /// Send request to server about read only acces to file
        /// </summary>
        /// <param name="selectedRow"></param
        private void SendReqToServerWithOpen(string selectedRow)
        {
            //TODO: when we add modify date, it can be wrong
            RestClient rClient = new RestClient();
            string fileNameWithoutFormat = selectedRow.Substring(0, selectedRow.Length - 4);
            rClient.endPoint = "http://localhost:8080/OpenFile/" + fileNameWithoutFormat;
            string strResponse = rClient.makeRequest();
            strResponse = strResponse.Substring(1, strResponse.Length - 2);

            OpenNewTabPage(fileNameWithoutFormat, strResponse);
        }
        #endregion

        private void createNewFileButton_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(newFileNameTextBox.Text, "");
        }
    }
}
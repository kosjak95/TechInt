using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace TechnikiInterentoweClient
{
    public partial class Form1 : Form
    {
        private List<FileData> filesListFromJson;
        private RestClient rClient;

        public Form1()
        {
            InitializeComponent();

            filesList.View = View.Details;
            filesList.Columns.Add("File Id");
            filesList.Columns.Add("File Name");
            filesList.Columns.Add("Updated");
            filesList.Columns.Add("Version");
            filesList.Columns.Add("IsEdited");
            filesList.GridLines = true;

            rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/Files/";
            string strResponse = rClient.makeRequest();

            UpdateFilesList(strResponse);
        }

        /// <summary>
        /// Update list view with files name
        /// </summary>
        /// <param name="strResponse"></param>
        private async void UpdateFilesList(string strResponse)
        {
            filesListFromJson = new JavaScriptSerializer().Deserialize<List<FileData>>(strResponse);

            foreach (FileData file in filesListFromJson)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems[0].Text = (filesListFromJson.IndexOf(file) + 1).ToString();
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, file.Name));
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, file.LastUpdateTs));
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, file.Version.ToString()));
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, file.IsEdited.ToString()));
                filesList.Items.Add(listViewItem);
            }
        }

        #region UI Event Hander

        private void filesList_Click(object sender, MouseEventArgs e)
        {
            int i = filesList.SelectedIndices[0];
            string selectedRow = filesList.Items[i].SubItems[1].Text;

            SendReqToServerWithOpen(selectedRow);
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            TabPage tab = (TabPage)((Button)sender).Parent;
            string fileName = tab.Text;
            string content = ((TextBox)tab.Controls[0]).Text;

            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/UpdateContent/";
            bool Response = rClient.makePostRequest(new { file_name = fileName, file_data = content });

            TabControl tc = (TabControl)tab.Parent;
            tc.TabPages.Remove(tab);
        }

        /// <summary>
        /// Create new tabPage with TextBox
        /// </summary>
        /// <param name="title">title of TabPage</param>
        /// <param name="fileContent">text to set inside TextBox</param>
        private TabPage OpenNewTabPage(string title, string fileContent, bool isEdited)
        {
            TabPage tp = new TabPage(title);
            tabs.TabPages.Add(tp);
            tabs.SelectedTab = tp;
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

            if (!isEdited)
            {
                Button saveButton = new Button();
                tp.Controls.Add(saveButton);
                saveButton.Click += new EventHandler(SaveButtonOnClick);
                saveButton.Text = "Save";
                saveButton.Dock = DockStyle.Top;
            }
            //Button closeButton = new Button();
            //tp.Controls.Add(closeButton);
            ////closeButton.Click += new EventHandler(SaveButtonOnClick);
            //closeButton.Text = "Close";
            //closeButton.Dock = DockStyle.Top;

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

            FileContent file_content = JsonConvert.DeserializeObject<FileContent>(strResponse);
            //KeyValuePair<int, string> keyValuePair = JsonConvert.DeserializeObject<KeyValuePair<int, string>>(strResponse);

            OpenNewTabPage(fileNameWithoutFormat, file_content.FileContent1, file_content.IsEdited);
        }
        #endregion

        private void createNewFileButton_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/TryCreate/";
            if (rClient.makePostRequest(new { file_name = newFileNameTextBox.Text }))
            {
                OpenNewTabPage(newFileNameTextBox.Text, "", false);
                rClient = new RestClient();
                rClient.endPoint = "http://localhost:8080/Files/";
                string strResponse = rClient.makeRequest();
                filesList.Items.Clear();
                UpdateFilesList(strResponse);
            }
            else
            {
                MessageBox.Show("Sorry You can't create this file");
            }
        }

        private void tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                if (rClient == null)
                {
                    rClient = new RestClient();
                }

                filesList.Items.Clear();
                rClient.endPoint = "http://localhost:8080/Files/";
                string strResponse = rClient.makeRequest();

                UpdateFilesList(strResponse);
            }
        }

        private void tabs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                return;
            }

            TabControl page = (TabControl)sender;
            if (page.SelectedTab.AccessibilityObject.Name.Equals("KK Reader"))
            {
                return;
            }

            if (page.SelectedTab.Controls.Count <= 1)
            {
                page.SelectedTab.Dispose();
                return;
            }

            RestClient restClient = new RestClient();
            rClient.endPoint = "http://localhost:8080/ReleaseFileCludge/";

            string fileName = page.SelectedTab.AccessibilityObject.Name;
            rClient.makePostRequest(new { fileName });
            page.SelectedTab.Dispose();


        }
    }
}
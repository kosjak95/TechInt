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
            bindingSource = new BindingSource();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource;

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
            dataGridView1.Rows.Clear();

            filesListFromJson = new JavaScriptSerializer().Deserialize<List<FileData>>(strResponse);

            int i = 0;
            foreach (FileData file in filesListFromJson)
            {
                file.FileId = (++i).ToString();
                bindingSource.Add(file);

            }
            dataGridView1.Refresh();
        }

        #region UI Event Hander

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            TabPage tab = (TabPage)((Button)sender).Parent;
            string fileName = tab.Text;
            string content = ((TextBox)tab.Controls[0]).Text;
            if (rClient == null)
            {
                rClient = new RestClient();
            }

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
            return tp;
        }

        /// <summary>
        /// Send request to server about read only acces to file
        /// </summary>
        /// <param name="selectedRow"></param
        private void SendReqToServerWithOpen(string fileNameWithoutFormat)
        {
            if (rClient == null)
            {
                rClient = new RestClient();
            }

            rClient.endPoint = "http://localhost:8080/OpenFile/" + fileNameWithoutFormat;
            string strResponse = rClient.makeRequest();

            FileContent file_content = JsonConvert.DeserializeObject<FileContent>(strResponse);

            OpenNewTabPage(fileNameWithoutFormat, file_content.FileContent1, file_content.IsEdited);
        }
        #endregion

        private void createAndOpenNewFile()
        {
            CreateFileDialog createDialog = new CreateFileDialog();

            string fileNameFromUser = "";
            if (createDialog.ShowDialog(this) == DialogResult.OK)
            {
                fileNameFromUser = createDialog.getFileNameToCreate();
                createDialog.Dispose();
            }
            else
            {
                createDialog.Dispose();
                return;
            }

            if (rClient == null)
            {
                rClient = new RestClient();
            }

            rClient.endPoint = "http://localhost:8080/TryCreate/";
            if (rClient.makePostRequest(new { file_name = fileNameFromUser }))
            {

                rClient.endPoint = "http://localhost:8080/OpenFile/" + fileNameFromUser;
                string strResponse = rClient.makeRequest();
                FileContent file_content = JsonConvert.DeserializeObject<FileContent>(strResponse);

                OpenNewTabPage(fileNameFromUser, file_content.FileContent1, file_content.IsEdited);

                if (rClient == null)
                {
                    rClient = new RestClient();
                }

                rClient.endPoint = "http://localhost:8080/Files/";
                strResponse = rClient.makeRequest();

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

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                rClient.endPoint = "http://localhost:8080/Files/";
                string strResponse = rClient.makeRequest();

                UpdateFilesList(strResponse);
            }
        }

        private void tabs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TabControl page = (TabControl)sender;
            if (e.Button == MouseButtons.Left)
            {
                return;
            }

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null)
            {
                return;
            }

            if (dgv.CurrentRow.Index >= filesListFromJson.Count)
            {
                return;
            }

            string file_name = filesListFromJson[dgv.CurrentRow.Index].Name;

            SendReqToServerWithOpen(file_name);
        }

        private void tabs_MouseClick(object sender, MouseEventArgs e)
        {
            TabControl page = (TabControl)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (page.SelectedTab.AccessibilityObject.Name.Equals("    +"))
                {
                    TabPage addPage = tabs.TabPages[tabs.TabPages.Count - 1];
                    tabs.TabPages.Remove(addPage);
                    createAndOpenNewFile();
                    tabs.TabPages.Add(addPage);
                }
            }
        }
    }
}
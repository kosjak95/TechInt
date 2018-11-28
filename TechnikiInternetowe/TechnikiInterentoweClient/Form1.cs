using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TechnikiInterentoweCommon;
using Message = TechnikiInterentoweCommon.Message;

namespace TechnikiInterentoweClient
{
    public partial class Form1 : Form
    {
        private List<FullFileData> filesListFromJson;
        private RestClient rClient;
        private ClientWebSocket clientSocket = null;
        private string client_name;
        ChatForm chatForm = null;
        bool isOnline;

        public Form1()
        {
            filesListFromJson = null;
            isOnline = true;
            ClientName clientName = new ClientName();
            DialogResult result = clientName.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                client_name = clientName.getClientName();
                clientName.Dispose();
            }
            else
            {
                clientName.Dispose();
                throw new NoUserNameException();
            }

            clientSocket = new ClientWebSocket();
            clientSocket.Setup();
            clientSocket.Start();

            InitializeComponent();
            bindingSource = new BindingSource();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource;

            UpdateFilesList();
        }

        /// <summary>
        /// Save on device file with json contains info about files(source is filesListFromJson member)
        /// </summary>
        private void SaveJsonWithFilesOnDevice()
        {
            if (!isOnline)
                return;

            using (StreamWriter file = File.CreateText(Path.GetDirectoryName(
                                                       Path.GetDirectoryName(
                                                       System.IO.Directory.GetCurrentDirectory())) + @"\App_Data\files.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, filesListFromJson);
            }
        }

        /// <summary>
        /// Load data from device to filesListFromJson, if Internet connection is disable
        /// </summary>
        private void loadFilesFromDevice()
        {
            try
            {
                using (StreamReader r = new StreamReader(Path.GetDirectoryName(
                                                         Path.GetDirectoryName(
                                                         System.IO.Directory.GetCurrentDirectory())) + @"\App_Data\files.txt"))
                {
                    string json = r.ReadToEnd();
                    filesListFromJson = JsonConvert.DeserializeObject<List<FullFileData>>(json);
                }
            }catch(Exception e)
            {
                MessageBox.Show("Your Internet connection lost. \n We are sorry, but it's first start-up" +
                    " of application on this computer. We cannot load data.");
            }
        }

        /// <summary>
        /// Update list view with files name
        /// </summary>
        /// <param name="strResponse"></param>
        private async void UpdateFilesList()
        {
            if (isOnline)
            {
                if (rClient == null)
                {
                    rClient = new RestClient();
                }
                rClient.endPoint = "http://localhost:8080/Files/";
                try
                {
                    isOnline = true;
                    string strResponse = rClient.makeRequest();
                    dataGridView1.Rows.Clear();

                    filesListFromJson = new JavaScriptSerializer().Deserialize<List<FullFileData>>(strResponse);

                    SaveJsonWithFilesOnDevice();
                }
                catch (Exception e)
                {
                    isOnline = false;
                    loadFilesFromDevice();
                }
            }
            int i = 0;
            foreach (FullFileData file in filesListFromJson)
            {
                file.FileId = (++i);
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
            TabPage addPage = tabs.TabPages[tabs.TabPages.Count - 1];
            tabs.TabPages.Remove(addPage);

            TabPage tp = new TabPage(title);
            tabs.TabPages.Add(tp);
            tabs.TabPages.Add(addPage);
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
            CommonFileContent file_content = SendReqToOpenFileAndReturnContentOfIt(new UserAndFileNamesPair()
                                                                                  { FileName = fileNameWithoutFormat ,
                                                                                    UserName = client_name });
            OpenNewTabPage(fileNameWithoutFormat, file_content.FileContent1, file_content.IsEdited);
        }
        #endregion

        private CommonFileContent SendReqToOpenFileAndReturnContentOfIt(UserAndFileNamesPair userAndFileNames)
        {
            if (isOnline)
            {
                if (rClient == null)
                {
                    rClient = new RestClient();
                }

                rClient.endPoint = "http://localhost:8080/OpenFile/" + userAndFileNames.FileName + "/" + userAndFileNames.UserName;
                string strResponse = rClient.makeRequest();

                return JsonConvert.DeserializeObject<CommonFileContent>(strResponse);
            }
            CommonFileContent fileFromDevice = new CommonFileContent();
            foreach (FullFileData fileContent in filesListFromJson)
            {
                if(fileContent.Name.Equals(userAndFileNames.FileName))
                {
                    fileFromDevice.EditorName = fileContent.EditorName;
                    fileFromDevice.FileContent1 = fileContent.FileContent;
                    fileFromDevice.FileId = fileContent.FileId;
                    fileFromDevice.IsEdited = fileContent.IsEdited;
                    fileFromDevice.Name = fileContent.Name;
                }
            }
            return fileFromDevice;
        }

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
                CommonFileContent file_content = SendReqToOpenFileAndReturnContentOfIt(new UserAndFileNamesPair()
                                                                                      { FileName = fileNameFromUser,
                                                                                        UserName = client_name });
                OpenNewTabPage(fileNameFromUser, file_content.FileContent1, file_content.IsEdited);

                UpdateFilesList();
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

                UpdateFilesList();
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

            if (rClient == null)
            {
                rClient = new RestClient();
            }
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
            if (e.ColumnIndex.Equals(5))
            {
                if (dgv.CurrentCell.FormattedValue.ToString().Equals("") ||
                    dgv.CurrentCell.FormattedValue.ToString().Equals(client_name))
                {
                    return;
                }

                chatForm = new ChatForm(client_name, dgv.CurrentCell.FormattedValue.ToString(), clientSocket, this);

                chatForm.Show(this);
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
                    createAndOpenNewFile();
                }
            }
        }

        ~Form1()
        {
            if (clientSocket != null)
            {
                clientSocket.Stop();
            }
        }


        public void MakeChatNull()
        {
            this.chatForm = null;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = (500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (clientSocket.msgsList.Count > 0)
            {
                Message message = clientSocket.msgsList[0];
                switch (message.Key)
                {
                    case MsgType.SYSTEM_ACTION_MSG:
                        {
                            UpdateFilesList();
                            clientSocket.msgsList.RemoveAt(0);
                            break;
                        }
                    case MsgType.AUTHORIZATION_MSG:
                        {
                            Message msg = new Message() { Key = MsgType.AUTHORIZATION_MSG,
                                                          Destination = null,
                                                          Sender = client_name,
                                                          Value = client_name };
                            clientSocket.sendMsg(new JavaScriptSerializer().Serialize(msg));
                            clientSocket.msgsList.RemoveAt(0);
                            break;
                        }
                    case MsgType.CHAT_MSG:
                        {
                            if (chatForm == null)
                            {
                                chatForm = new ChatForm(client_name, message.Sender, clientSocket, this);
                            }

                            bool isOpen = false;
                            foreach (Form f in Application.OpenForms)
                            {
                                if (f.Equals(chatForm))
                                {
                                    isOpen = true;
                                }
                            }
                            if (!isOpen)
                            {
                                chatForm.Show(this);
                            }

                            break;
                        }
                }

            }
        }

    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
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
        Timer offlineTimer, onlineTimer;

        public Form1()
        {
            filesListFromJson = new List<FullFileData>();
            LoadFilesFromDevice();
            SetConnectionStatus(false);
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
            offlineTimer = new Timer
            {
                Interval = 1000
            };
            offlineTimer.Tick += new EventHandler(Timer_TickOfline);
            offlineTimer.Start();
            onlineTimer = new Timer
            {
                Interval = 500
            };
            onlineTimer.Tick += new EventHandler(Timer_Tick);

            clientSocket = new ClientWebSocket();
            clientSocket.Setup();

            InitializeComponent();
            bindingSource = new BindingSource();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource;

            UpdateFilesList();
        }

        void SetConnectionStatus(bool status)
        {
            isOnline = status;
            if (status)
            {
                this.Icon = Properties.Resources.onliceIcon;
                return;
            }
            this.Icon = Properties.Resources.offlineIcon;
        }

        /// <summary>
        /// Save on device file with json contains info about files(source is filesListFromJson member)
        /// </summary>
        private void SaveJsonWithFilesOnDevice()
        {
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
        private void LoadFilesFromDevice()
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
            }
            catch (Exception)
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
                string strResponse = MakePutRequest("Files/");
                dataGridView1.Rows.Clear();

                filesListFromJson = new JavaScriptSerializer().Deserialize<List<FullFileData>>(strResponse);

                SaveJsonWithFilesOnDevice();
            }
            else
            {
                SetConnectionStatus(false);
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
            if (isOnline)
            {
                MakePostRequest("UpdateContent/", new { file_name = fileName, file_data = content });
            }
            else
            {
                FullFileData fileData = filesListFromJson.Find(item => item.Name.Equals(fileName));
                fileData.FileContent = content;
                fileData.Version++;
                fileData.EditorName = "";
                fileData.IsEdited = false;
            }
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

            TextBox fileContentTB = new TextBox
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(0, Convert.ToInt32(0.2 * Convert.ToInt32(tp.Size.Height.ToString()))),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = true
            };
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
            {
                FileName = fileNameWithoutFormat,
                UserName = client_name
            });
            OpenNewTabPage(fileNameWithoutFormat, file_content.FileContent1, file_content.IsEdited);
        }
        #endregion

        private void ConfigureBeforeRequest(string routeAndArgs)
        {
            if (rClient == null)
            {
                rClient = new RestClient();
            }
            rClient.EndPoint = "http://localhost:8080/" + routeAndArgs;
        }

        private string MakePutRequest(string routeAndArgs)
        {
            ConfigureBeforeRequest(routeAndArgs);
            return rClient.MakeRequest();
        }

        private bool MakePostRequest(string route, object inObject)
        {
            ConfigureBeforeRequest(route);
            return rClient.MakePostRequest(inObject);
        }

        private CommonFileContent SendReqToOpenFileAndReturnContentOfIt(UserAndFileNamesPair userAndFileNames)
        {
            if (isOnline)
            {
                string strResponse = MakePutRequest("OpenFile/" + userAndFileNames.FileName + "/" + userAndFileNames.UserName);
                return JsonConvert.DeserializeObject<CommonFileContent>(strResponse);
            }
            CommonFileContent fileFromDevice = new CommonFileContent();
            FullFileData fileContent = filesListFromJson.Find(item => item.Name.Equals(userAndFileNames.FileName));
            fileFromDevice.EditorName = fileContent.EditorName;
            fileFromDevice.FileContent1 = fileContent.FileContent;
            fileFromDevice.FileId = fileContent.FileId;
            fileFromDevice.IsEdited = fileContent.IsEdited;
            fileFromDevice.Name = fileContent.Name;
            return fileFromDevice;
        }

        private void CreateAndOpenNewFile()
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

            if (isOnline)
            {
                if (MakePostRequest("TryCreate/", new { file_name = fileNameFromUser }))
                {
                    CommonFileContent file_content = SendReqToOpenFileAndReturnContentOfIt(new UserAndFileNamesPair()
                    {
                        FileName = fileNameFromUser,
                        UserName = client_name
                    });
                    OpenNewTabPage(fileNameFromUser, file_content.FileContent1, file_content.IsEdited);

                    UpdateFilesList();
                }
                return;
            }
            else
            {
                FullFileData fileData = filesListFromJson.Find(item => item.Name.Equals(fileNameFromUser));
                if (fileData == null)
                {
                    fileData = new FullFileData
                    {
                        EditorName = client_name,
                        FileContent = "",
                        IsEdited = false,
                        Name = fileNameFromUser,
                        Version = 0
                    };
                    filesListFromJson.Add(fileData);
                    OpenNewTabPage(fileNameFromUser, fileData.FileContent, fileData.IsEdited);
                    UpdateFilesList();
                    return;
                }
            }
            MessageBox.Show("Sorry You can't create this file");
        }

        private void Tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                UpdateFilesList();
            }
        }

        private void Tabs_MouseDoubleClick(object sender, MouseEventArgs e)
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

            string fileName = page.SelectedTab.AccessibilityObject.Name;
            MakePostRequest("ReleaseFileCludge/", new { fileName });
            page.SelectedTab.Dispose();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView dgv))
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

        private void Tabs_MouseClick(object sender, MouseEventArgs e)
        {
            TabControl page = (TabControl)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (page.SelectedTab.AccessibilityObject.Name.Equals("    +"))
                {
                    CreateAndOpenNewFile();
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
            Timer timer = new Timer
            {
                Interval = 500
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        
        private void Timer_TickOfline(object sender, EventArgs e)
        {
            switch(clientSocket.isConnected())
            {
                case WebSocket4Net.WebSocketState.Open:
                    {
                        offlineTimer.Stop();
                        onlineTimer.Start();
                        UpdateFilesList();
                        SaveJsonWithFilesOnDevice();
                        break;
                    }
                case WebSocket4Net.WebSocketState.Closed:
                case WebSocket4Net.WebSocketState.None:
                    {
                        clientSocket.Start();
                        break;
                    }
            }
          
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (clientSocket.msgsList.Count > 0)
            {
                Message message = clientSocket.msgsList[0];
                switch (message.Key)
                {
                    case MsgType.REFRESH_FILES_LIST_MSG:
                        {
                            UpdateFilesList();
                            clientSocket.msgsList.RemoveAt(0);
                            break;
                        }
                    case MsgType.FAIL_SYNC_FILES_MSG:
                        {
                            InformUserAboutFailSyncFilesAfterReEstablishedConnection(message.Value);
                            clientSocket.msgsList.RemoveAt(0);
                            break;
                        }
                    case MsgType.AUTHORIZATION_MSG:
                        {
                            clientSocket.SendMsg(new Message()
                            {
                                Key = MsgType.AUTHORIZATION_MSG,
                                Value = client_name,
                                Sender = client_name,
                                Destination = "Server"
                            });
                            SetConnectionStatus(true);
                            string serializedObject = new JavaScriptSerializer().Serialize(new SynchronizeAfterConnectionEstablishedMsg()
                            {
                                filesList = filesListFromJson,
                                sender = client_name
                            });
                            MakePostRequest("SynchronizeAfterConnectionEstablished/",
                                            new { synchMsg = serializedObject });
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
            if (clientSocket.isConnected() != WebSocket4Net.WebSocketState.Open)
            {
                onlineTimer.Stop();
                offlineTimer.Start();
                SetConnectionStatus(false);
            }
        }

        private void InformUserAboutFailSyncFilesAfterReEstablishedConnection(string serializedListOfFailSyncFiles)
        {
            List<string> filesList = new JavaScriptSerializer().Deserialize<List<string>>(serializedListOfFailSyncFiles);
            StringBuilder builder = new StringBuilder();
            builder.Append("Nie udało się zsynchronizować następujących plików:\n");
            foreach(string fileName in filesList)
            {
                builder.Append(fileName).Append("\n");
            }
            builder.Append("Dane zostały utracone");

            new System.Threading.Thread(() =>
            {
                MessageBox.Show(builder.ToString(), "Fail synchronize files",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }).Start();
        }
    }
}
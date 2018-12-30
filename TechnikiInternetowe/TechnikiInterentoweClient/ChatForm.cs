using System;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TechnikiInterentoweCommon;
using Message = TechnikiInterentoweCommon.Message;

namespace TechnikiInterentoweClient
{
    public partial class ChatForm : Form
    {
        private string clientName;
        private ClientWebSocket socket;
        private Form1 parent;
        private Timer timer;

        public ChatForm(string clientName, string destinationName, ClientWebSocket socket, Form1 parent)
        {
            this.clientName = clientName;
            this.socket = socket;
            InitializeComponent();
            this.Text = destinationName;
            this.parent = parent;
        }

        private void send_button_Click(object sender, EventArgs e)
        {
            this.tbChat.Text += "Ja: " + tbMsg.Text + "\r\n";
            socket.SendMsg(new Message()
            {
                Key = TechnikiInterentoweCommon.MsgType.CHAT_MSG,
                Destination = this.Text,
                Sender = clientName,
                Value = this.tbMsg.Text
            });
            tbMsg.Text = "";
        }

        public void addMsgOnScreen(Message msg)
        {
            tbChat.Text += msg.Sender + ": ";
            tbChat.Text += msg.Value + "\r\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = (500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (socket.msgsList.Count > 0)
            {
                Message msg = socket.msgsList[0];
                if (msg != null)
                {
                    if (msg.Key.Equals(MsgType.CHAT_MSG))
                    {
                        this.addMsgOnScreen(msg);
                    }
                    socket.msgsList.RemoveAt(0);
                }
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            this.parent.MakeChatNull();
        }
    }
}

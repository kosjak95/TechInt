using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace TechnikiInterentoweClient
{
    public partial class ChatForm : Form
    {
        private string clientName;
        ClientWebSocket socket;

        public ChatForm(string clientName, string destinationName, ClientWebSocket socket)
        {
            this.clientName = clientName;
            this.socket = socket;
            InitializeComponent();
            this.Text = destinationName;
        }

        private void send_button_Click(object sender, EventArgs e)
        {
            this.tbChat.Text += "\n Ja: " + tbMsg.Text;
            Message msg = new Message() { Key = 3, Destination = this.Text, Sender = clientName, Value = this.tbMsg.Text };
            socket.sendMsg(new JavaScriptSerializer().Serialize(msg));
            tbMsg.Text = "";
        }

        public void addMsgOnScreen(Message msg)
        {
            tbChat.Text += "\n" + msg.Sender + ": ";
            tbChat.Text += msg.Value;
        }

        private void ChatForm_Activated(object sender, EventArgs e)
        {
            if (socket.msgsList.Count > 0)
            {
                foreach(Message msg in socket.msgsList)
                {
                    if(msg.Key.Equals(3))
                    {
                        this.addMsgOnScreen(msg);
                    }
                }
            }
        }
    }
}

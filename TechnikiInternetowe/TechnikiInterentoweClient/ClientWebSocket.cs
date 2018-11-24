using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace TechnikiInterentoweClient
{
    public class ClientWebSocket
    {
        private WebSocket websocketClient;
        private string url;
        private string protocol;
        private WebSocketVersion version;
        public List<Message> msgsList;

        public void Setup()
        {
            msgsList = new List<Message>();
            this.url = "ws://localhost:8081";
            this.protocol = "basic";
            this.version = WebSocketVersion.Rfc6455;

            websocketClient = new WebSocket(this.url, this.protocol, this.version);

            websocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocketClient_Error);
            websocketClient.Opened += new EventHandler(websocketClient_Opened);
            websocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketClient_MessageReceived);
        }

        public void Start()
        {
            websocketClient.Open();
        }

        private void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Message message = new JavaScriptSerializer().Deserialize<Message>(e.Message);
            msgsList.Add(message);
        }

        private void websocketClient_Opened(object sender, EventArgs e)
        {
            Console.Write("Succesfully connected");
        }

        private void websocketClient_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        public void sendMsg(string msg)
        {
            websocketClient.Send(msg);
        }

        public void Stop()
        {
            websocketClient.Close();

            Console.WriteLine("Client disconnected");
        }
    }
}
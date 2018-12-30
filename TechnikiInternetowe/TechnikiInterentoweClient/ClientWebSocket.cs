using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TechnikiInterentoweCommon;
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
            this.url = "ws://" + Properties.Settings.Default.Host.ToString() + ":8081";
            this.protocol = "basic";
            this.version = WebSocketVersion.Rfc6455;

            websocketClient = new WebSocket(this.url, this.protocol, this.version);

            websocketClient.Error += new EventHandler<ErrorEventArgs>(WebsocketClient_Error);
            websocketClient.Opened += new EventHandler(WebsocketClient_Opened);
            websocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(WebsocketClient_MessageReceived);
        }

        public WebSocketState isConnected()
        {
            return websocketClient.State;
        }

        public void Start()
        {
            websocketClient.Open();
        }

        private void WebsocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Message message = new JavaScriptSerializer().Deserialize<Message>(e.Message);
            msgsList.Add(message);
        }

        private void WebsocketClient_Opened(object sender, EventArgs e)
        {
            Console.Write("Succesfully connected");
        }

        private void WebsocketClient_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        public void SendMsg(Message msg)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            websocketClient.Send(serializer.Serialize(msg));
        }

        public void Stop()
        {
            websocketClient.Close();

            Console.WriteLine("Client disconnected");
        }
    }
}
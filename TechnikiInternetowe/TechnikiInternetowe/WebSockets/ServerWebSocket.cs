using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Script.Serialization;

namespace TechnikiInternetowe.WebSockets
{
    public class ServerWebSocket
    {
        private const int Port = 8081;
        private WebSocketServer webSocketServer;
        private static ServerWebSocket serverSocketInstance = null;
        private static readonly object m_oPadLock = new object();
        private List<Client> listOfClientsSessions;

        public static ServerWebSocket Instance
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (serverSocketInstance == null)
                    {
                        serverSocketInstance = new ServerWebSocket();
                    }
                    return serverSocketInstance;
                }

            }
        }
        private ServerWebSocket()
        {
            listOfClientsSessions = new List<Client>();
            webSocketServer = new WebSocketServer();
            webSocketServer.Setup(Port);
            webSocketServer.NewSessionConnected += webSocketServer_NewSessionConnected;
            webSocketServer.NewMessageReceived += webSocketServer_NewMessageReceived;
            webSocketServer.NewDataReceived += webSocketServer_NewDataReceived;
            webSocketServer.SessionClosed += webSocketServer_SessionClosed;

            ThreadPool.QueueUserWorkItem(new WaitCallback((x) => this.runServer()));
        }

        private void webSocketServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.Write("SessionClosed");
        }

        private void webSocketServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.Write("NewDataReceive");
        }

        private void webSocketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.Write(value);
            //session.Send("received" + value);
            
            Message message = new JavaScriptSerializer().Deserialize<Message>(value);
            //1. server update actions
            //2. naming of clients
            //3. chat
            switch(message.Key)
            {
                case 1:
                    {
                        throw new NotImplementedException();
                        break;
                    }
                case 2:
                    {
                        foreach(Client client in listOfClientsSessions )
                        {
                            if (client.socket == session)
                            {
                                listOfClientsSessions.Remove(client);
                                listOfClientsSessions.Add(new Client() { clientName = message.Value, socket = session });
                                break;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        foreach(Client client in listOfClientsSessions)
                        {
                            client.socket.Send(message.Value);
                        }
                        break;
                    }
            }
            //TODO: Any handle of client msgs
        }

        private void webSocketServer_NewSessionConnected(WebSocketSession session)
        {
            listOfClientsSessions.Add(new Client() { clientName = "", socket = session });
            KeyValuePair<int, string> initMsgToClient = new KeyValuePair<int, string>(2, "name");
            session.Send(new JavaScriptSerializer().Serialize(initMsgToClient));

            Console.Write("SessionConnected");
        }

        public void closeServer()
        {
            webSocketServer.Stop();
        }

        public void sendToAll(string msg)
        {
            foreach(Client client in listOfClientsSessions)
            {
                client.socket.Send(msg);
            }
        }

        public async void runServer()
        {
            webSocketServer.Start();
        }

        private class Client
        {
            public string clientName;
            public WebSocketSession socket;

        }

    }

    public class Message
    {
        public int Key;
        public string Value;
    }

}
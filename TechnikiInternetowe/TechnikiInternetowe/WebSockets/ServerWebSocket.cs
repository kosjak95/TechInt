using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Script.Serialization;
using TechnikiInterentoweCommon;

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

            Message message = new JavaScriptSerializer().Deserialize<Message>(value);
            switch(message.Key)
            {
                case MsgType.SYSTEM_ACTION_MSG:
                    {
                        throw new NotImplementedException();
                        break;
                    }
                case MsgType.AUTHORIZATION_MSG:
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
                case MsgType.CHAT_MSG:
                    {
                        foreach(Client client in listOfClientsSessions)
                        {
                            if (client.clientName.Equals(message.Destination))
                            {
                                client.socket.Send(new JavaScriptSerializer().Serialize(new Message()
                                {
                                    Key = message.Key,
                                    Destination = message.Destination,
                                    Sender = message.Sender,
                                    Value = message.Value
                                }));
                            }
                        }
                        break;
                    }
            }
        }

        private void webSocketServer_NewSessionConnected(WebSocketSession session)
        {
            listOfClientsSessions.Add(new Client() { clientName = "", socket = session });
            Message initMsgToClient = new Message() { Key = MsgType.AUTHORIZATION_MSG, Destination = null, Value = "name" };
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
}
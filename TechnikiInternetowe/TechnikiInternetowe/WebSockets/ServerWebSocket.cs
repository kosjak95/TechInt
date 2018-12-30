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
            webSocketServer.NewSessionConnected += WebSocketServer_NewSessionConnected;
            webSocketServer.NewMessageReceived += WebSocketServer_NewMessageReceived;
            webSocketServer.NewDataReceived += WebSocketServer_NewDataReceived;
            webSocketServer.SessionClosed += WebSocketServer_SessionClosed;

            ThreadPool.QueueUserWorkItem(new WaitCallback((x) => this.runServer()));
        }

        private void WebSocketServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.Write("SessionClosed");
        }

        private void WebSocketServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.Write("NewDataReceive");
        }

        private void WebSocketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.Write(value);

            Message message = new JavaScriptSerializer().Deserialize<Message>(value);
            switch(message.Key)
            {
                case MsgType.REFRESH_FILES_LIST_MSG:
                    {
                        throw new NotImplementedException();
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

        private void WebSocketServer_NewSessionConnected(WebSocketSession session)
        {
            listOfClientsSessions.Add(new Client() { clientName = "", socket = session });
            Message initMsgToClient = new Message() { Key = MsgType.AUTHORIZATION_MSG, Destination = null, Value = "name" };
            session.Send(new JavaScriptSerializer().Serialize(initMsgToClient));

            Console.Write("SessionConnected");
        }

        public void CloseServer()
        {
            webSocketServer.Stop();
        }

        public void SendToAll(Message msg)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (Client client in listOfClientsSessions)
            {
                msg.Destination = client.clientName;
                msg.Sender = "Server";
                client.socket.Send(serializer.Serialize(msg));
            }
        }

        public void SendToOne(string receiver, List<string> filesNameslist)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (Client client in listOfClientsSessions)
            {
                if(!client.clientName.Equals(receiver))
                {
                    continue;
                }
                client.socket.Send(serializer.Serialize(new Message()
                {
                    Destination = client.clientName,
                    Sender = "Server",
                    Key = MsgType.FAIL_SYNC_FILES_MSG,
                    Value = serializer.Serialize(filesNameslist)
                }));
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
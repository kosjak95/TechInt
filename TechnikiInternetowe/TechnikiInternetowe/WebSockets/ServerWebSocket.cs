using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Threading;

namespace TechnikiInternetowe.WebSockets
{
    public class ServerWebSocket
    {
        private const int Port = 8081;
        private static WebSocketServer webSocketServer;
        private static ServerWebSocket serverSocketInstance = null;
        private static readonly object m_oPadLock = new object();

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
            session.Send("received" + value);
            //TODO: Any handle of client msgs
        }

        private void webSocketServer_NewSessionConnected(WebSocketSession session)
        {
            Console.Write("SessionConnected");
        }

        public void closeServer()
        {
            webSocketServer.Stop();
        }

        public async void runServer()
        {
            webSocketServer.Start();
        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TechnikiInternetowe.WebSockets
{
    public class ServerWebSocket
    {
        private const int Port = 8081;
        IPAddress ip;
        TcpListener server;
        TcpClient client;
        bool serverStatus;
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

            ip = Dns.GetHostEntry("localhost").AddressList[0];
            server = new TcpListener(Port);
            client = default(TcpClient);
            serverStatus = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback((x) => this.runServer()));
        }

        public void turnOffServer()
        {
            serverStatus = false;
        }

        public async void runServer()
        {
            try
            {
                server.Start();
                Console.Write("Server is running");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write("We cannot run server: " + e.Message);
            }

            while (serverStatus)
            {
                client = server.AcceptTcpClient();

                byte[] buffer = new byte[200];
                NetworkStream stream = client.GetStream();

                stream.Read(buffer, 0, buffer.Length);
                StringBuilder builder = new StringBuilder();

                foreach (byte b in buffer)
                {
                    if (b.Equals(00))
                    {
                        break;
                    }
                    else
                    {
                        builder.Append(Convert.ToChar(b).ToString());
                    }
                }

                Console.Write(builder.ToString());
            }
        }
    }
}
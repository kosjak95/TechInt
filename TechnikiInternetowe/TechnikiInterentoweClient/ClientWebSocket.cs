using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TechnikiInterentoweClient
{
    public class ClientWebSocket
    {
        static string serverIP = "localhost";
        static int port = 8081;

        static public void sendMsg(string msg)
        {
            TcpClient client = new TcpClient(serverIP,port);

            int byteCount = Encoding.ASCII.GetByteCount(msg);
            byte[] data = new byte[byteCount];
            data = Encoding.ASCII.GetBytes(msg);

            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
        }
    }
}

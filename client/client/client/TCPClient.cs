using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace client
{
    class TCPClient
    {
        private string str;

        public  TCPClient(string str)
        {
            this.str = str;
        }

        public string SendMsg()
        {
            byte[] bytes = new byte[1024];

            IPEndPoint end = new IPEndPoint(IPAddress.Loopback,11000);
       
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        

           sock.Connect(end);

            string message = str;
            byte[] msg = Encoding.UTF8.GetBytes(message);

            int byteSent = sock.Send(msg);

            int byteRec = sock.Receive(bytes);

            str = Encoding.UTF8.GetString(bytes,0,byteRec);

            

            sock.Shutdown(SocketShutdown.Both);
            sock.Close();

            return str;

        }
    }
}

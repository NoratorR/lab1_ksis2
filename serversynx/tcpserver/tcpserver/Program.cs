using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace tcpserver
{
    class Program
    {
        private Socket sock;
        static void Main(string[] args)
        {
            Program prg = new Program();

            prg.StartListen();

        }

        public void StartListen()
        {
           
            IPEndPoint endP = new IPEndPoint(IPAddress.Any, 11000);

             sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(endP);
            sock.Listen(10);
            Console.WriteLine("Server works");

            while (true)
                changeDataWithClient();
        }


        public void changeDataWithClient()
        {
            Socket handl = sock.Accept();


            byte[] bytes = new byte[1024];
            int byterec = handl.Receive(bytes);

            string str;
            str = ReverseStr(Encoding.UTF8.GetString(bytes,0,byterec));

            handl.Send(Encoding.UTF8.GetBytes(str));

            if (Encoding.UTF8.GetString(bytes, 0, byterec).IndexOf("<TheEnd>") > -1)
            {
                Console.WriteLine("Connection ended!");
            }

            handl.Shutdown(SocketShutdown.Both);
            handl.Close();
        }


    
        public string ReverseStr(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }


    }
}

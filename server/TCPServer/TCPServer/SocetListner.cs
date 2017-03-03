using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class SocetListner
    {

        private Socket serverSocket;

        private class ConnectionInfo
        {
            public Socket Sck;
            public byte[] Buffer;
        }

        private List<ConnectionInfo> connectList = new List<ConnectionInfo>();

      
        
        public void Start()
        {
            var endP = new IPEndPoint(IPAddress.Any, 11000);
             serverSocket = new Socket(endP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(endP);
            serverSocket.Listen(10);
            while (true)
            {
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
                Thread.Sleep(1000);
            }
        }

        public void AcceptCallback(IAsyncResult arg)
        {
            ConnectionInfo connection = new ConnectionInfo();

            Socket s = (Socket)arg.AsyncState;
            connection.Sck = s.EndAccept(arg);
            connection.Buffer = new byte[1024];
            lock(connectList) connectList.Add(connection);

            connection.Sck.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), connection);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);


        }

        private void ReceiveCallback(IAsyncResult arg)
        {
           ConnectionInfo connection =  (ConnectionInfo)arg.AsyncState;
            string revStr;

            int bytesRead = connection.Sck.EndReceive(arg);
            if (0 != bytesRead)
            {
                lock (connectList)
                    foreach (ConnectionInfo conn in connectList)
                        if (connection != conn)
                        {
                            revStr = ReverseStr(Encoding.UTF8.GetString(connection.Buffer));

                            conn.Sck.Send(Encoding.UTF8.GetBytes(revStr), bytesRead, SocketFlags.None);
                        }
                connection.Sck.BeginReceive( connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), connection);
            }
            else CloseConnection(connection);



        }
        public string ReverseStr(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void CloseConnection(ConnectionInfo cls)
        {
            cls.Sck.Close();
            lock (connectList) connectList.Remove(cls);
        }

    }
}


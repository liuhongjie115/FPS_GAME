using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();

        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }


        public void Start()
        {
            clientSocket.BeginReceive(msg.data, msg.startIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult async)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(async);
                if(count==0)
                {
                    Close();
                }
                else
                {
                    msg.ReadMessage(count,OnProcessMessage);
                }
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }
            finally
            {
                Start();
            }
        }

        private void OnProcessMessage(string methodStr,string data)
        {
            server.HandlerRequest(methodStr,data, this);
        }

        public void SendMessage(string methodStr,string s)
        {
            clientSocket.Send(msg.PackData(methodStr, s));
        }

        private void Close()
        {
            if(clientSocket!=null)
            {
                clientSocket.Close();
                server.RemoveClient(this);
                Console.WriteLine("移除一个客户端:"+(clientSocket.RemoteEndPoint as IPEndPoint).Address);
            }
        }
    }
}

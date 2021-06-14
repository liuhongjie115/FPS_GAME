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
            if (clientSocket == null || clientSocket.Connected == false) return;
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
                msg.ReadMessage(count,OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }
            finally
            {
                
            }
        }

        private void OnProcessMessage(object vo)
        {
            server.HandlerRequest(vo, this);
        }

        public void SendMessage(object vo)
        {
            try
            {
                clientSocket.Send(msg.PackData(vo));
            }
            catch(Exception e)
            {

            }

        }

        public void Close()
        {
            server.RemoveClient(this);
            if (clientSocket!=null)
            {
                if(clientSocket.Connected)
                {
                    clientSocket.Close();
                }
            }
            Console.WriteLine("移除一个客户端");
        }
    }
}

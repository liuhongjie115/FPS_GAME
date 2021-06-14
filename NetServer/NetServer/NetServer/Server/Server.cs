using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class Server
    {
        private IPEndPoint iPEndPoint;
        private Socket serverSocket;

        private List<Client> clientList;

        private ModuleManager moduleManager;
        internal ModuleManager ModuleManager { get => moduleManager; }

        public Server() { }
        public Server(string ipStr,int port)
        {
            SetIPAndPort(ipStr, port);
        }

        private void SetIPAndPort(string ipStr,int port)
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {

            clientList = new List<Client>();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack, null);
            ModuleManager.Bind(this);
            TimerManager.Init();
        }

        private void AcceptCallBack(IAsyncResult async)
        {
            Socket clientSocket = serverSocket.EndAccept(async);
            Console.WriteLine("收到一位用户尝试连接服务器" + (clientSocket.RemoteEndPoint as IPEndPoint).Address);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            ModuleManager.netModule.AddClient(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void HandlerRequest(object vo,Client client)
        {
            ModuleManager.HandlerRequest(vo, client);
        }

        public void RemoveClient(Client client)
        {
            lock(clientList)
            {
                if (clientList.Contains(client))
                {
                    clientList.Remove(client);
                }
                ModuleManager.netModule.RemoveClient(client);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class ChatCtrl
    {
        private Server server;
        public ChatCtrl(Server server)
        {
            this.server = server;
            server.ModuleManager.RegisterNet(this);
        }

        public void m_chat_module_toc(string data,Client client)
        {
            Console.WriteLine(data);

        }

        public void m_chat_module_tos(string data, Client client)
        {
            client.SendMessage("m_chat_module_toc", data);
        }
    }
}

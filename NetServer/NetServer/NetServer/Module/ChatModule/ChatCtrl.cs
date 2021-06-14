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
            ModuleManager.RegisterNet(this);
        }

        public void m_chat_module_toc(string msg,Client client)
        {
            m_chat_module_toc vo = new m_chat_module_toc();
            vo.msg = msg;
            client.SendMessage(vo);
        }

        public void m_chat_module_tos(m_chat_module_tos data, Client client)
        {

            m_chat_module_toc(data.msg, client);

        }
    }
}

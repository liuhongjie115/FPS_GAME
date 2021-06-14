using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class NetModule
    {
        private Dictionary<Client, double> clientHertDict = new Dictionary<Client, double>();
        private float disConnect = 5f;


        private Server server;
        public NetModule(Server server)
        {
            this.server = server;
            ModuleManager.RegisterNet(this);
        }

        public void AddClient(Client client)
        {
            if (!clientHertDict.ContainsKey(client))
            {
                clientHertDict.Add(client, TimeUtil.GetNowTime());
            }
            m_hert_toc(client);
            TimerManager.AddTimerInterval(client, (Action<Client>)CheckHert, 1,new object[] {client});
            Console.WriteLine("添加循环");
        }

        public void RemoveClient(Client client)
        {
            if (clientHertDict.ContainsKey(client))
            {
                TimerManager.DelTimer(client);
                clientHertDict.Remove(client);
                m_net_dis_toc(client);
            }
        }
        //接受客户端心跳包
        public void m_hert_tos(m_hert_tos vo,Client client)
        {
            if(!clientHertDict.ContainsKey(client))
            {
                client.Close();
            }
            else
            {
                clientHertDict[client] = TimeUtil.GetNowTime();
            }
            Console.WriteLine("接受心跳包");
        }

        //发送客户端心跳包
        public void m_hert_toc(Client client)
        {
            Console.WriteLine("发送心跳包");
            if (!clientHertDict.ContainsKey(client))
            {
                clientHertDict.Add(client, TimeUtil.GetNowTime());
            }
            m_hert_toc vo = new m_hert_toc();
            client.SendMessage(vo);
        }
    
        //发送客户端断线
        public void m_net_dis_toc(Client client)
        {
            m_net_dis_toc vo = new m_net_dis_toc();
            client.SendMessage(vo);
            Console.WriteLine("断线");
        }

        private void CheckHert(Client client)
        {
            bool flag = false;
            if (TimeUtil.GetNowTime() - clientHertDict[client] > disConnect)
            {
                client.Close();
            }
            else
            {
                flag = true;
            }
            if(flag)
            {
                m_hert_toc(client);
            }
        }
    }
}

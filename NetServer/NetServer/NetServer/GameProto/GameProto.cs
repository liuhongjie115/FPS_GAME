using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class GameProto
    {
    }
    [System.Serializable]
    public class p_Time
    {
        public int hour;
        public int min;
        public int secd;
    }

    [System.Serializable]
    public class m_chat_module_tos
    {
        public string msg;
    }
    [System.Serializable]
    public class m_chat_module_toc
    {
        public int eoorCode;
        public string msg;
        public p_Time time;
    }

    [System.Serializable]
    public class m_hert_tos
    {

    }

    [System.Serializable]
    public class m_hert_toc
    {

    }
    [System.Serializable]
    public class m_net_dis_toc
    {

    }
}

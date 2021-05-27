using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class ProtoManager
    {
        private Server server;
        public ProtoManager(Server server)
        {
            this.server = server;
        }

        public void HandlerRequest(string methodStr,Byte[] data,Client client)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集
            dynamic obj = assembly.CreateInstance(methodStr);
            
        }
    }
}

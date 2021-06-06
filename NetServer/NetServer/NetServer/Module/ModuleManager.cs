using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class ModuleManager
    {
        private Server server;
        private ChatCtrl chatCtrl;

        private List<Object> registerList = new List<object>();

        public ModuleManager()
        {


        }

        public void Bind(Server server)
        {
            this.server = server;
            chatCtrl = new ChatCtrl(server);

            //所有模块都在以上这里初始化；
        }

        public void RegisterNet(Object ctrl)
        {
            registerList.Add(ctrl);
            //HandlerRequest("m_chat_module_toc", "123", null);
        }

        public void HandlerRequest(string methodStr, string data, Client client)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集
            foreach (Object item in registerList)
            {
                Type type = item.GetType();
                MethodInfo methodInfo = type.GetMethod(methodStr);
                if(methodInfo!=null)
                {
                    methodInfo.Invoke(item,new object[] {data,client});
                }
            }
        }
    }
}

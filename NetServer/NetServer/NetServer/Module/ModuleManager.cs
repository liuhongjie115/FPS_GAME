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
        private static Server server;
        public static ChatCtrl chatCtrl;
        public static NetModule netModule;

        private static List<Object> registerList = new List<object>();


        public static void Bind(Server server)
        {
            ModuleManager.server = server;
            chatCtrl = new ChatCtrl(server);
            netModule = new NetModule(server);

            //所有模块都在以上这里初始化；
        }

        public static void RegisterNet(Object ctrl)
        {
            registerList.Add(ctrl);
            //HandlerRequest("m_chat_module_toc", "123", null);
        }

        public static void HandlerRequest(object vo, Client client)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集
            foreach (Object item in registerList)
            {
                Type type = item.GetType();
                MethodInfo methodInfo = type.GetMethod(vo.GetType().Name);
                if(methodInfo!=null)
                {
                    methodInfo.Invoke(item,new object[] {vo, client });
                }
            }
        }
    }
}

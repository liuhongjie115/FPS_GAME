using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public struct MsgItem
{
    public MethodInfo methodInfo;
    public System.Object item;
    public object[] paras;
    public MsgItem(MethodInfo methodInfo,System.Object item,object[] paras)
    {
        this.methodInfo = methodInfo;
        this.item = item;
        this.paras = paras;
    }

    public void Invoke()
    {
        methodInfo.Invoke(item, paras);
    }
}

public class ModuleManager
{
    //private static ModuleManager instance;
    //public static ModuleManager Instance
    //{
    //    get
    //    {
    //        if(instance==null)
    //        {
    //            instance = new ModuleManager();
    //        }
    //        return instance;
    //    }
    //}

    private static List<System.Object> registerList;
    private static Queue<MsgItem> msgQueues;

    public static ChatModule chatModule;

    static ModuleManager()
    {
        registerList = new List<System.Object>();
        msgQueues = new Queue<MsgItem>();
    }

    public static void Update()
    {
        if(msgQueues!=null)
        {
            int i = 0;
            while (i < 10&&msgQueues.Count > 0)
            {
                MsgItem msgItem = msgQueues.Dequeue();
                msgItem.Invoke();
                i++;
            }
        }
       
    }

    public static void Bind()
    {
        chatModule = new ChatModule();

        //以上写模块注册
    }

    public static void RegisterNet(System.Object ctrl)
    {
        registerList.Add(ctrl);
    }


    /// <summary>
    /// 协议转发响应
    /// </summary>
    /// <param name="o"></param>
    public static void HandlerRequest(object o)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集
        foreach (System.Object item in registerList)
        {
            Type type = item.GetType();
            MethodInfo methodInfo = type.GetMethod(o.GetType().Name);
            if (methodInfo != null)
            {
                //methodInfo.Invoke(item, new object[] { data});
                msgQueues.Enqueue(new MsgItem(methodInfo, item, new object[] { o }));
            }
        }
    }

}

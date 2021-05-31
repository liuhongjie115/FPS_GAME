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
    private static ModuleManager instance;
    public static ModuleManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new ModuleManager();
            }
            return instance;
        }
    }

    private List<System.Object> registerList;
    private Queue<MsgItem> msgQueues;

    public ChatCtrl chatCtrl;

    public ModuleManager()
    {
        registerList = new List<System.Object>();
        msgQueues = new Queue<MsgItem>();
    }

    public void Update()
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

    public void Bind()
    {
        chatCtrl = new ChatCtrl();

        //以上写模块注册
    }

    public void RegisterNet(System.Object ctrl)
    {
        registerList.Add(ctrl);
    }

    public void HandlerRequest(string methodStr, string data)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集
        foreach (System.Object item in registerList)
        {
            Type type = item.GetType();
            MethodInfo methodInfo = type.GetMethod(methodStr);
            if (methodInfo != null)
            {
                //methodInfo.Invoke(item, new object[] { data});
                msgQueues.Enqueue(new MsgItem(methodInfo, item, new object[] { data }));
            }
        }
    }

}

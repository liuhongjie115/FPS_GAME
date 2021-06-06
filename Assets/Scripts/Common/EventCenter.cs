using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();    //字典
    //添加无参的
    public static void AddListener(EventType eventType, CallBack callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))                            //查看字典是否有Key  -> eventType;
        {
            m_EventTable.Add(eventType, null);                                  //没有Key则添加
        }

        Delegate d = m_EventTable[eventType];                                      //取出Key的值

        if (d != null && d.GetType() != callBack.GetType())                       //值是否为空或者与传参的委托类型是否一样
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1},要添加的委托是{2}", eventType, d.GetType(), callBack.GetType()));
        }
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;   //将委托合并
    }

    //添加一个参数
    public static void AddListener<T>(EventType eventType,CallBack<T> callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))                            //查看字典是否有Key  -> eventType;
        {
            m_EventTable.Add(eventType, null);                                  //没有Key则添加
        }

        Delegate d = m_EventTable[eventType];                                      //取出Key的值

        if (d != null && d.GetType() != callBack.GetType())                       //值是否为空或者与传参的委托类型是否一样
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1},要添加的委托是{2}", eventType, d.GetType(), callBack.GetType()));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;   //将委托合并
    }
   
    //移除无参
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(String.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }else if(d.GetType()!=callBack.GetType()){
                throw new Exception(String.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托是{1},要移除的委托是{2}",eventType,d.GetType(),callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;

    }
    //移除一个参数
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(String.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(String.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托是{1},要移除的委托是{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
    }
    //无参
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        if(m_EventTable.TryGetValue(eventType,out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
    //一个参数
    public static void Broadcast<T>(EventType eventType,T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
}

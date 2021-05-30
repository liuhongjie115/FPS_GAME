using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

public class Message
{
    public byte[] data = new byte[2048];
    public int startIndex;
    public int RemainSize
    {
        get => data.Length - startIndex;
    }
    public Message() { }   
    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="newDataAmount">新读取数据的个数</param>
    public void ReadMessage(int newDataAmount, Action<string, string> callBack)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);//包头长度读取
            if (startIndex - 4 >= count)   //长度足够再读
            {
                //string s = Encoding.UTF8.GetString(data, 4, count);
                int methodCount = BitConverter.ToInt32(data, 4); //方法长度
                string methodStr = Encoding.UTF8.GetString(data, 8, methodCount);
                //byte[] useData = new byte[2048];
                //Array.Copy(data, 8, useData, 0, count-4);
                string useData = Encoding.UTF8.GetString(data, 8 + methodCount, count - 4 - methodCount);
                callBack(methodStr, useData);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);
            }
        }
    }

    /// <summary>
    /// 打包数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] PackData(string methodStr, string data)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] methodBytes = Encoding.UTF8.GetBytes(methodStr);
        int dataAmount = dataBytes.Length + methodBytes.Length+4;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        byte[] methodAmountBytes = BitConverter.GetBytes(methodStr.Length);
        dataAmountBytes = dataAmountBytes.Concat(methodAmountBytes).ToArray().Concat(methodBytes).ToArray().Concat(dataBytes).ToArray();

        return dataAmountBytes;
    }
}

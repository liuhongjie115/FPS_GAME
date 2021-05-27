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
    /// ��������
    /// </summary>
    /// <param name="newDataAmount">�¶�ȡ���ݵĸ���</param>
    public void ReadMessage(int newDataAmount)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);//��ͷ���ȶ�ȡ
            if (startIndex - 4 >= count)   //�����㹻�ٶ�
            {
                string s = Encoding.UTF8.GetString(data, 4, count);
                Debug.Log(s);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);
            }
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] PackData(string data)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int dataAmount = data.Length;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        dataAmountBytes = dataAmountBytes.Concat(dataBytes).ToArray();
        return dataAmountBytes;
    }
}

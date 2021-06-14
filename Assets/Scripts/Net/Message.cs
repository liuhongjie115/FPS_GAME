using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

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
    public void ReadMessage(int newDataAmount, Action<object> callBack)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);//��ͷ���ȶ�ȡ
            Debug.Log("startIndex:"+startIndex);
            Debug.Log("count:" + count);
            if (startIndex - 4 >= count)   //�����㹻�ٶ�
            {
                Debug.Log("����");
                byte[] useData = new byte[2048];
                Array.Copy(data, 4, useData, 0, count);

                object o = FormatterByteObject(useData);
                callBack(o);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);

                //string s = Encoding.UTF8.GetString(data, 4, count);
                //int methodCount = BitConverter.ToInt32(data, 4); //��������
                //string methodStr = Encoding.UTF8.GetString(data, 8, methodCount);
                ////byte[] useData = new byte[2048];
                ////Array.Copy(data, 8, useData, 0, count-4);
                //string useData = Encoding.UTF8.GetString(data, 8 + methodCount, count - 4 - methodCount);
                //callBack(methodStr, useData);
                //Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                //startIndex -= (count + 4);
            }
        }
    }

    /// <summary>
    /// ���л��������
    /// </summary>
    /// <param name="proto"></param>
    /// <returns></returns>
    public byte[] PackData(object proto)
    {
        byte[] dataBytes = FormatterObjectBytes(proto);
        int amountCount = dataBytes.Length;
        byte[] amountBytes = BitConverter.GetBytes(amountCount);
        amountBytes = amountBytes.Concat(dataBytes).ToArray();
        return amountBytes;
    }

    ///// <summary>
    ///// �������
    ///// </summary>
    ///// <param name="data"></param>
    ///// <returns></returns>
    //public byte[] PackData(string methodStr, string data)
    //{
    //    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
    //    byte[] methodBytes = Encoding.UTF8.GetBytes(methodStr);
    //    int dataAmount = dataBytes.Length + methodBytes.Length+4;
    //    byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
    //    byte[] methodAmountBytes = BitConverter.GetBytes(methodStr.Length);
    //    dataAmountBytes = dataAmountBytes.Concat(methodAmountBytes).ToArray().Concat(methodBytes).ToArray().Concat(dataBytes).ToArray();
    //    Debug.Log("���ݳ��ȣ�" + dataAmount);
    //    Debug.Log("���ݳ��ȣ�" + (dataAmountBytes.Length-4));
    //    return dataAmountBytes;
    //}

    public byte[] PackData(object proto, bool isFirst = false)
    {
        byte[] dataBytes = null;
        if (isFirst)
        {
            dataBytes = BitConverter.GetBytes(DataType.START);
            byte[] classBytes = Encoding.UTF8.GetBytes(proto.GetType().Name);
            dataBytes = dataBytes.Concat(classBytes).ToArray();
        }
        else
        {
            dataBytes = new byte[0];
        }


        FieldInfo[] fieldInfos = proto.GetType().GetFields();
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            //��int����
            if (fieldInfo.FieldType == typeof(int))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.INT);
                byte[] intBytes = BitConverter.GetBytes((int)fieldInfo.GetValue(proto));
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(intBytes).ToArray();
            }
            else if (fieldInfo.FieldType == typeof(double))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.DOUBLE);
                byte[] doubleBytes = BitConverter.GetBytes((double)fieldInfo.GetValue(proto));
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(doubleBytes).ToArray();
            }
            else if (fieldInfo.FieldType == typeof(float))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.INT);
                byte[] floatBytes = BitConverter.GetBytes((float)fieldInfo.GetValue(proto));
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(floatBytes).ToArray();
            }
            else if (fieldInfo.FieldType == typeof(char))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.CHAR);
                byte[] charBytes = BitConverter.GetBytes((char)fieldInfo.GetValue(proto));
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(charBytes).ToArray();
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.BOOL);
                byte[] boolBytes = BitConverter.GetBytes((bool)fieldInfo.GetValue(proto));
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(boolBytes).ToArray();
            }
            else if (fieldInfo.FieldType == typeof(string))
            {
                byte[] dataType = BitConverter.GetBytes(DataType.STRING);
                string data = (string)fieldInfo.GetValue(proto);
                byte[] stringBytes = Encoding.UTF8.GetBytes(data);
                byte[] lengthBytes = BitConverter.GetBytes(stringBytes.Length);
                dataBytes = dataBytes.Concat(dataType).ToArray();
                dataBytes = dataBytes.Concat(lengthBytes).ToArray();
                dataBytes = dataBytes.Concat(stringBytes).ToArray();
            }
            //�����ͼ̳�IEnumerable���Ƿ���   ��List����
            else if (fieldInfo.FieldType.GetInterface("IEnumerable", false) != null && fieldInfo.FieldType.IsGenericType)
            {
                var listVal = fieldInfo.GetValue(proto) as IEnumerable<object>;
                if (listVal != null)
                {
                    byte[] dataType = BitConverter.GetBytes(DataType.LIST);
                    dataBytes = dataBytes.Concat(dataType).ToArray();    //д��ͷ����ʾ�Ǽ���
                    int count = listVal.Count();
                    dataBytes = dataBytes.Concat(BitConverter.GetBytes(count)).ToArray();   //д�뼯��Ԫ�ظ���
                    List<byte[]> tempDataBytesList = new List<byte[]>();
                    List<byte[]> lengthBytesList = new List<byte[]>();
                    foreach (var aa in listVal)
                    {
                        byte[] tempDataBytes = PackData(aa);                       //����
                        byte[] LengthBytes = BitConverter.GetBytes(tempDataBytes.Length);  //ÿ��list����
                        tempDataBytesList.Add(tempDataBytes);
                        lengthBytesList.Add(LengthBytes);
                    }
                    for (int i = 0; i < count; i++)
                    {
                        dataBytes = dataBytes.Concat(tempDataBytesList[i]).ToArray();   //д�볤��
                        dataBytes = dataBytes.Concat(lengthBytesList[i]).ToArray();  //д������
                    }
                }
            }
            else if (fieldInfo.FieldType.IsArray)
            {
                Array arrayVal = fieldInfo.GetValue(proto) as Array;
                if (arrayVal != null)
                {
                    byte[] dataType = BitConverter.GetBytes(DataType.ARRAY);
                    dataBytes = dataBytes.Concat(dataType).ToArray();    //д��ͷ����ʾ������
                    int count = arrayVal.Length;            //���鳤��
                    dataBytes = dataBytes.Concat(BitConverter.GetBytes(count)).ToArray();   //д������Ԫ�ظ���
                }
            }
        }
        return dataBytes;
    }

    /// <summary>
    /// ���л�Ϊbyte����
    /// </summary>
    /// <param name="proto"></param>
    /// <returns></returns>
    public byte[] FormatterObjectBytes(object proto)
    {
        Assembly ass = Assembly.GetExecutingAssembly();
        Debug.Log(ass.FullName);
        if (proto == null)
            throw new ArgumentNullException("proto");
        byte[] buff;
        try
        {
            using (var ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, proto);
                buff = ms.GetBuffer();
            }
        }
        catch (Exception er)
        {
            throw new Exception(er.Message);
        }
        return buff;
    }

    public static object FormatterByteObject(byte[] buff)
    {
        Debug.Log("��ʼ�����л�");
        if (buff == null)
            throw new ArgumentNullException("buff");
        object obj;
        try
        {
            using (var ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Binder = new UBinder();
                obj = iFormatter.Deserialize(ms);
            }
        }
        catch (Exception er)
        {
            throw new Exception(er.Message);
        }
        return obj;
    }

    public class UBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            Type[] types = ass.GetTypes();        //��ǰ���������е�����
            typeName = typeName.Substring(typeName.IndexOf('.') + 1);
            Type type = ass.GetType(typeName);
            Debug.Log("Type:" + type);
            return type;
        }
    }


}

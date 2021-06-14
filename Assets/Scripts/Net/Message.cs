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
    /// 解析数据
    /// </summary>
    /// <param name="newDataAmount">新读取数据的个数</param>
    public void ReadMessage(int newDataAmount, Action<object> callBack)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);//包头长度读取
            Debug.Log("startIndex:"+startIndex);
            Debug.Log("count:" + count);
            if (startIndex - 4 >= count)   //长度足够再读
            {
                Debug.Log("解析");
                byte[] useData = new byte[2048];
                Array.Copy(data, 4, useData, 0, count);

                object o = FormatterByteObject(useData);
                callBack(o);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);

                //string s = Encoding.UTF8.GetString(data, 4, count);
                //int methodCount = BitConverter.ToInt32(data, 4); //方法长度
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
    /// 序列化打包数据
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
    ///// 打包数据
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
    //    Debug.Log("数据长度：" + dataAmount);
    //    Debug.Log("数据长度：" + (dataAmountBytes.Length-4));
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
            //是int类型
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
            //该类型继承IEnumerable且是泛型   ，List类型
            else if (fieldInfo.FieldType.GetInterface("IEnumerable", false) != null && fieldInfo.FieldType.IsGenericType)
            {
                var listVal = fieldInfo.GetValue(proto) as IEnumerable<object>;
                if (listVal != null)
                {
                    byte[] dataType = BitConverter.GetBytes(DataType.LIST);
                    dataBytes = dataBytes.Concat(dataType).ToArray();    //写入头，表示是集合
                    int count = listVal.Count();
                    dataBytes = dataBytes.Concat(BitConverter.GetBytes(count)).ToArray();   //写入集合元素个数
                    List<byte[]> tempDataBytesList = new List<byte[]>();
                    List<byte[]> lengthBytesList = new List<byte[]>();
                    foreach (var aa in listVal)
                    {
                        byte[] tempDataBytes = PackData(aa);                       //数据
                        byte[] LengthBytes = BitConverter.GetBytes(tempDataBytes.Length);  //每个list长度
                        tempDataBytesList.Add(tempDataBytes);
                        lengthBytesList.Add(LengthBytes);
                    }
                    for (int i = 0; i < count; i++)
                    {
                        dataBytes = dataBytes.Concat(tempDataBytesList[i]).ToArray();   //写入长度
                        dataBytes = dataBytes.Concat(lengthBytesList[i]).ToArray();  //写入数据
                    }
                }
            }
            else if (fieldInfo.FieldType.IsArray)
            {
                Array arrayVal = fieldInfo.GetValue(proto) as Array;
                if (arrayVal != null)
                {
                    byte[] dataType = BitConverter.GetBytes(DataType.ARRAY);
                    dataBytes = dataBytes.Concat(dataType).ToArray();    //写入头，表示是数组
                    int count = arrayVal.Length;            //数组长度
                    dataBytes = dataBytes.Concat(BitConverter.GetBytes(count)).ToArray();   //写入数组元素个数
                }
            }
        }
        return dataBytes;
    }

    /// <summary>
    /// 序列化为byte数据
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
        Debug.Log("开始反序列化");
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
            Type[] types = ass.GetTypes();        //当前程序集下所有的类名
            typeName = typeName.Substring(typeName.IndexOf('.') + 1);
            Type type = ass.GetType(typeName);
            Debug.Log("Type:" + type);
            return type;
        }
    }


}

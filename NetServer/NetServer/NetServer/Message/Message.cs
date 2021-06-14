using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    class Message
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
                if (startIndex - 4 >= count)   //长度足够再读
                {
                    byte[] useData = new byte[2048];
                    Array.Copy(data, 4, useData, 0, count);

                    object o = FormatterByteObject(useData);
                    callBack(o);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
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
        /// <summary>
        /// 序列化为byte数据
        /// </summary>
        /// <param name="proto"></param>
        /// <returns></returns>
        public byte[] FormatterObjectBytes(object proto)
        {
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
    }

    public class UBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {//
            Assembly ass = Assembly.GetExecutingAssembly();
            typeName = typeName.Substring(typeName.IndexOf('.') + 1);
            Type type = ass.GetType("NetServer."+typeName);
            Console.WriteLine(type.ToString());
            return type;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net
{
    private static NetSocket netSocket;


    public static NetSocket NetSocket { get => netSocket;}

    public static void Init()
    {
        netSocket = new NetSocket();
        netSocket.Init();
    }


    public static void SendMessage(object proto)
    {
        netSocket.SendMessage(proto);
    }



}

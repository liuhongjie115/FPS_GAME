using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net
{
    private static Net instance;
    private NetSocket netSocket;

    public static Net Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new Net();
            }
            return instance;
        }
    }

    public void Init()
    {
        netSocket = new NetSocket();
        netSocket.Init();
    }
}

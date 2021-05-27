using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetSocket
{
    private const string IP = "127.0.0.1";
    private const int PORT = 4396;
    private Socket clientSocket;
    private Message msg;

    public void Init()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        msg = new Message();
        try
        {
            clientSocket.Connect(IP, PORT);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        if (clientSocket == null || clientSocket.Connected == false) return;
        clientSocket.BeginReceive(msg.data, msg.startIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendMessage(string str)
    {
        clientSocket.Send(msg.PackData(str));
    }
}

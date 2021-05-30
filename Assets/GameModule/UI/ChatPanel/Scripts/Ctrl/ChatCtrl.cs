using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCtrl
{

    public ChatPanel chatPanel;
    public ChatCtrl()
    {
        ModuleManager.Instance.RegisterNet(this);
    }

    public void m_chat_module_toc(string data)
    {
        Debug.Log(data);
        chatPanel.AddMessage(data);
    }

    public void m_chat_module_tos(string data)
    {
        Net.Instance.NetSocket.SendMessage("m_chat_module_toc", data);
    }
}

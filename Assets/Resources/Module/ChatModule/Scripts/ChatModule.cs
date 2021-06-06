using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatModule:BaseModule
{

    public ChatPanel chatPanel;
    public ChatPanel1 chatPanel1;
    public ChatPanel2 chatPanel2;

    public override void InitModule()
    {
        OpenChatPanel();
    }

    public void OpenChatPanel()
    {
        if(chatPanel==null)
        {
            chatPanel = new ChatPanel();
    
        }
        if(!chatPanel.IsShowInGame())
        {
            chatPanel.SetDataKV("Open");
        }
    }

    public void OpenChatPanel2()
    {
        if (chatPanel2 == null)
        {
            chatPanel2 = new ChatPanel2();

        }
        if (!chatPanel2.IsShowInGame())
        {
            chatPanel2.SetDataKV("Open");
        }
    }

    public void OpenChatPanel1()
    {
        if (chatPanel1 == null)
        {
            chatPanel1 = new ChatPanel1();

        }
        if (!chatPanel1.IsShowInGame())
        {
            chatPanel1.SetDataKV("Open");
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel1 : BasePanel
{
    public Text chatText;
    public InputField input;
    public Button btnOpen;
    public Button btnClose;

    public ChatPanel1():base("ChatModule")
    {
        
    }

    public override void InitUI()
    {
        btnOpen.onClick.AddListener(() => { ModuleManager.chatModule.OpenChatPanel2(); });
        btnClose.onClick.AddListener(() => { Close(); });
    }

    public override void Open()
    {
        base.Open();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void OnClickBtn()
    {
        ModuleManager.chatModule.m_chat_module_tos(input.text);
    }

    public override void OnGUI<T>(T a)
    {

    }

    public void AddMessage(string data)
    {
        chatText.text = chatText.text + "\n" + data;
    }


}

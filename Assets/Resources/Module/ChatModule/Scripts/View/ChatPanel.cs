using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    public Text chatText;
    public InputField input;
    public Button btnOpen;
    public Button btnClose;

    public ChatPanel():base("ChatModule")
    {
        
    }

    public override void InitUI()
    {
        //btnOpen = this.GetCompoent("btnOpen", typeof(Button)) as Button;
        btnOpen.onClick.AddListener(() => { ModuleManager.Instance.chatModule.OpenChatPanel1(); });
        //btnClose = this.GetCompoent("btnClose", typeof(Button)) as Button;
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
        m_chat_module_tos(input.text);
    }

    public void m_chat_module_tos(string msg)
    {
        ModuleManager.Instance.chatModule.m_chat_module_tos(msg);
    }

    public void m_chat_module_toc(string msg)
    {

    }

    public override void OnGUI<T>(T a)
    {
    
    }

    public void AddMessage(string data)
    {
        chatText.text = chatText.text + "\n" + data;
    }


}

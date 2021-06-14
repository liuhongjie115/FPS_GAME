using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    public Text lblText;
    public InputField inputMsg;
    public Button btnOpen;
    public Button btnClose;
    public Button btnSend;

    public ChatPanel():base("ChatModule")
    {
        
    }

    public override void InitUI()
    {
        //btnOpen = this.GetCompoent("btnOpen", typeof(Button)) as Button;
        btnOpen.onClick.AddListener(() => { ModuleManager.chatModule.OpenChatPanel1(); });
        //btnClose = this.GetCompoent("btnClose", typeof(Button)) as Button;
        btnClose.onClick.AddListener(() => { Close(); });
        btnSend.onClick.AddListener(() => { OnClickBtn(); });
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
        m_chat_module_tos(inputMsg.text);
    }

    public void m_chat_module_tos(string msg)
    {
        ModuleManager.chatModule.m_chat_module_tos(msg);
    }


    public override void OnGUI<T>(T a)
    {
    
    }

    public void AddMessage(string data)
    {
        lblText.text = lblText.text + "\n" + data;
    }


}

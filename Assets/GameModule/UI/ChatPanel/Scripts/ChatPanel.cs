using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    public Text chatText;
    public InputField input;
    public Button btnOK;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        ModuleManager.Instance.chatCtrl.chatPanel = this;
    }

    public void OnClickBtn()
    {
        ModuleManager.Instance.chatCtrl.m_chat_module_tos(input.text);
    }

    public void AddMessage(string data)
    {
        chatText.text = chatText.text + "\n" + data;
    }
}

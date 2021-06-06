using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel : DisplayObj
{

    public bool isPosOut = false;

    public BasePanel(string url):base(url)
    {

    }

    public virtual void Open()
    {
        WindowsManager.Instance.Open(this);
    }

    public virtual void OnOpen()
    {

    }

    public virtual void Close()
    {
        WindowsManager.Instance.Close(this);
    }

    public virtual void OnClose()
    {

    }

    public bool IsShowInGame()
    {
        if(this.go)
        {
            return !isPosOut && this.activeSelf;
        }
        return false;
    }


}

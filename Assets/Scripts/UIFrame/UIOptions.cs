using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    BasePanel,
    BaseView
}

public class UIOptions : MonoBehaviour
{
    public bool fullPanel = false;
    public UIType uiType = UIType.BasePanel;

}

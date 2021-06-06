using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager
{
    private static LayerManager instance;

    public static LayerManager Instance { get
        {
            if(instance==null)
            {
                instance = new LayerManager();
            }
            return instance;
        }
    }

    public Transform RectTranWindows { get => rectTranWindows;}
    public Transform RectTranFullWindows { get => rectTranFullWindows;  }
    public Camera UiCamera { get => uiCamera; }

    private const int SORT = 100;

    private Camera uiCamera;
    private Canvas uiRoot;
    private Transform rectTranRoot;

    private Transform rectTranWindows;
    private Transform rectTranFullWindows;

    public void Init()
    {
        InitCamera();
        InitLayer();
    }


    private void InitCamera()
    {
        uiCamera = GameObject.Find("UIRoot/Camera").GetComponent<Camera>();
    }

    private void InitLayer()
    {
        uiRoot = GameObject.Find("UIRoot").GetComponent<Canvas>();
        rectTranRoot = GameObject.Find("UIRoot/rectTranRoot").transform;

        rectTranWindows = (new GameObject("rectTranWindows")).AddComponent<RectTransform>();
        rectTranWindows.SetParent(rectTranRoot);
        rectTranWindows.localScale = Vector3.one;

        rectTranFullWindows = (new GameObject("rectTranFullWindows")).AddComponent<RectTransform>();
        rectTranFullWindows.SetParent(rectTranRoot);
        rectTranFullWindows.localScale = Vector3.one;
    }


    public float GetWidth()
    {
        return (this.rectTranRoot as RectTransform).sizeDelta.x;
    }

    public float GetHeight()
    {
        return (this.rectTranRoot as RectTransform).sizeDelta.y;
    }

}

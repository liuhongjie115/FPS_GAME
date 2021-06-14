using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager
{

    public static Transform RectTranWindows { get => rectTranWindows;}
    public static Transform RectTranFullWindows { get => rectTranFullWindows;  }
    public static Camera UiCamera { get => uiCamera; }

    private const int SORT = 100;

    private static Camera uiCamera;
    private static Canvas uiRoot;
    private static Transform rectTranRoot;

    private static Transform rectTranWindows;
    private static Transform rectTranFullWindows;

    public static void Init()
    {
        InitCamera();
        InitLayer();
    }


    private static void InitCamera()
    {
        uiCamera = GameObject.Find("UIRoot/Camera").GetComponent<Camera>();
    }

    private static void InitLayer()
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


    public static float GetWidth()
    {
        return (rectTranRoot as RectTransform).sizeDelta.x;
    }

    public static float GetHeight()
    {
        return (rectTranRoot as RectTransform).sizeDelta.y;
    }

}

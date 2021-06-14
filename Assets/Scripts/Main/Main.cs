using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main mainInstance;

    public static Main MainInstance { get => mainInstance; }

    private void Awake()
    {
        mainInstance = this;
        Init();
        EventCenter.Broadcast(EventType.GAME_AFTER);
    }

    private void Init()
    {
        InitNet();
        InitModule();
        InitUIFrame();
    }

    private void InitNet()
    {
        Net.Init();
    }

    private void InitModule()
    {
        ModuleManager.Bind();
    }
    private void InitUIFrame()
    {
        LayerManager.Init();
    }
    private void Update()
    {
        //ModuleManager.Update();
        ResPool.Update();
        TimerManager.Update();
    }


}

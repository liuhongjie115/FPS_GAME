using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitNet();
        InitModule();
    }

    private void InitNet()
    {
        Net.Instance.Init();
    }

    private void InitModule()
    {
        ModuleManager.Instance.Bind();
    }

    private void Update()
    {
        ModuleManager.Instance.Update();
    }
}

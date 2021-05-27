using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitNet();
    }

    private void InitNet()
    {
        Net.Instance.Init();
    }
}

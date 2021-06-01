using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Start()
    {
        GameObject go = new GameObject("Main");
        go.AddComponent<Main>();
        Destroy(gameObject);
    }
}

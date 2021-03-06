using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public BaseModule()
    {
        ModuleManager.RegisterNet(this);
        EventCenter.AddListener(EventType.GAME_AFTER, InitModule);
    }

    public abstract void InitModule();

}

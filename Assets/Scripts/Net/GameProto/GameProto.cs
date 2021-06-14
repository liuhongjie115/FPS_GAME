using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType
{
    public const int START = 80;
    public const int INT = 90;
    public const int FLOAT = 92;
    public const int DOUBLE = 93;
    public const int CHAR = 94;
    public const int BOOL = 95;
    public const int STRING = 96;
    public const int ARRAY = 97;
    public const int LIST = 98;
    public const int CUSTOM = 99;
}

public class GameProto
{

}


[System.Serializable]
public class p_Time
{
    public int hour;
    public int min;
    public int secd;
}

[System.Serializable]
public class m_chat_module_tos
{
    public string msg;
}
[System.Serializable]
public class m_chat_module_toc
{
    public int eoorCode;
    public string msg;
    public p_Time time;
}


[System.Serializable]
public class m_hert_tos
{

}

[System.Serializable]
public class m_hert_toc
{

}
[System.Serializable]
public class m_net_dis_toc
{

}
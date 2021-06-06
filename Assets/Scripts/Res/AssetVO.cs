using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetVO
{
    private string url;
    private int refCount;

    public Object prefab;

    public Dictionary<Object, bool> objDict = new Dictionary<Object, bool>();

    public string Url { get => url;}

    public AssetVO(string url, Object prefab)
    {
        this.url = url;
        this.prefab = prefab;
        this.refCount = 0;
    }

    public Object GetRes()
    {
        foreach (KeyValuePair<Object, bool> kvp in objDict)
        {
            if(kvp.Value==false)
            {
                refCount++;
                objDict[kvp.Key] = true;
                return kvp.Key;
            }
        }
        Object obj = GameObject.Instantiate(prefab);
        objDict.Add(obj, true);
        return obj;
    }

    public void Remove(Object obj)
    {
        if(objDict.ContainsKey(obj))
        {
            objDict[obj] = false;
            refCount--;
        }
    }

    public int GetRefCount()
    {
        return this.refCount;
    }

}

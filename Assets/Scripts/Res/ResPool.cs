using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResPool
{
    private static ResPool instance;

    public static ResPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResPool();
            }
            return instance;
        }
    }


    private Dictionary<string, AssetVO> assetDict = new Dictionary<string, AssetVO>();


    float time = 10f;
    float timer = 0f;

    public void Update()
    {
        if(timer>time)
        {
            CheckResRef();
            timer = 0f;
        }
        timer += Time.deltaTime;
    }

    public void LoadRes(string url, System.Action<AssetVO> OnLoadComplete)
    {
        if(assetDict.ContainsKey(url))
        {
            OnLoadComplete(assetDict[url]);
        }
        else
        {
             Main.MainInstance.StartCoroutine(LoadAsyn(url, OnLoadComplete));
        }
    }

    private IEnumerator LoadAsyn(string url, System.Action<AssetVO> OnLoadComplete)
    {

        ResourceRequest resourceRequest = Resources.LoadAsync<Object>(url);
        yield return resourceRequest;
        AssetVO assetVO = new AssetVO(url, resourceRequest.asset);
        assetDict.Add(url, assetVO);
        OnLoadComplete(assetVO);
    }


    private void CheckResRef()
    {
        List<AssetVO> needDelAssetVOS = new List<AssetVO>();
        foreach (KeyValuePair<string, AssetVO> kvp in assetDict)
        {
            if(kvp.Value.GetRefCount()<=0)
            {
                needDelAssetVOS.Add(kvp.Value);
            }
        }
        for (int i= 0;i<needDelAssetVOS.Count;i++)
        {
            assetDict.Remove(needDelAssetVOS[i].Url);
        }
    }
}

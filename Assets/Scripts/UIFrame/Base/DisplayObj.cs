using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class DisplayObj
{
    protected string url;

    protected GameObject go;
    protected Transform tran;

    protected bool activeSelf = false;
    protected Vector2 anchoredPosition = Vector2.zero;
    private Vector2 realAnchoredPosition = Vector2.zero;
    protected Vector3 scale = Vector3.one;
    protected Vector3 euler = Vector3.zero;
    protected Transform parent = null;


    protected bool isLoading = false;
    protected bool isLoadFinish = false;


    private AssetVO assetVO;

    private Dictionary<string, Object[]> methodCallBackDict = new Dictionary<string, Object[]>();

    private Object onGUICallbackParam;

    public Vector2 RealAnchoredPosition { get => realAnchoredPosition; set => realAnchoredPosition = value; }
    public GameObject Go { get => go;}
    public Transform Tran { get => tran;}

    public DisplayObj(string url=null)
    {
        this.url = url;
        System.Type type = this.GetType();
        this.url = "Module/" + this.url + "/Prefabs/" + type.Name;
        LoadAsset();
    }

    private void LoadAsset()
    {
        if (this.go==null)
        {
            isLoading = true;
            isLoadFinish = false;
            ResPool.LoadRes(url, OnLoadComplete);
        }
    }

    private void OnLoadComplete(AssetVO assetVO)
    {
        isLoading = false;
        isLoadFinish = true;
        this.assetVO = assetVO;
        this.go = assetVO.GetRes() as GameObject;
        this.tran = this.go.transform;
        OnGUICallBack();
    }


    public void SetDataKV(string method,Object[] param=null)
    {

        if (methodCallBackDict.ContainsKey(method))
        {
            methodCallBackDict.Remove(method);
        }
        methodCallBackDict.Add(method, param);
        if (isLoadFinish&&!isLoading)
        {
            MethodCallBack();
        }
    }

    private void OnGUICallBack()
    {
        InitProperty();
        FindCompoents();
        InitUI();
        OnGUI(onGUICallbackParam);
        MethodCallBack();
    }

    private void MethodCallBack()
    {
        foreach (KeyValuePair<string, Object[]> kvp in methodCallBackDict)
        {
            string methodStr = kvp.Key;
            Object[] param = kvp.Value;
            Assembly assembly = Assembly.GetExecutingAssembly();  //获得当前程序集

            System.Type type = this.GetType();
            MethodInfo methodInfo = type.GetMethod(methodStr);
            if (methodInfo != null)
            {
                methodInfo.Invoke(this, param);
            }
        }
        methodCallBackDict.Clear();
    }

    public void SetData(Object param)
    {
        this.onGUICallbackParam = param;
    }

    public abstract void OnGUI<T>(T a);

    private void InitProperty()
    {
        Debug.Log(this.go);
        if(this.go)
        {
            this.go.SetActive(this.activeSelf);
            Debug.Log("parent:" + this.parent);
            if (this.parent)
            {
          
                this.tran.SetParent(this.parent);
            }
            (this.tran as RectTransform).anchoredPosition = this.anchoredPosition;
            this.tran.localEulerAngles = this.euler;
            this.tran.localScale = this.scale;
      
        }
    }

    public abstract void InitUI();

    private void FindCompoents()
    {
        Dictionary<string, System.Type> prefixType = GameObjectUtil.PrefixType;
        FieldInfo[] fieldInfos = this.GetType().GetFields();
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            Debug.Log("pinfo.Name:" + fieldInfo.Name);
            foreach (KeyValuePair<string, System.Type> kvp in prefixType)
            {
                if (fieldInfo.Name.Length>kvp.Key.Length&& fieldInfo.Name.Substring(0, kvp.Key.Length).Equals(kvp.Key))
                {
                    fieldInfo.SetValue(this, GameObjectUtil.FindCompoent(this.go, fieldInfo.Name));
                }
            }
            
          
        }
        //foreach (PropertyInfo pinfo in propertys)
        //{
        //    Response.Write("<br>" + pinfo.Name + "," + pinfo.GetValue(sp, null) + "<br>");
        //}
    }


    public void SetActive(bool isOn=true)
    {
        if(this.go)
        {
            this.go.SetActive(isOn);
        }
        this.activeSelf = isOn;
    }

    public bool IsActive()
    {
        if (this.go)
        {
            return this.go.activeSelf;
        }
        return this.activeSelf;
    }

    public void SetAnchoredPosition(Vector2 pos)
    {
        if (this.go)
        {
            (this.tran as RectTransform).anchoredPosition = pos;
        }
        this.anchoredPosition = pos;
    }

    public Vector2 GetAnchoredPosition()
    {
        if (this.go)
        {
            return (this.tran as RectTransform).anchoredPosition;
        }
        return this.anchoredPosition;
    }

    public void SetScale(Vector3 scale)
    {
        if (this.go)
        {
            this.tran.localScale = scale;
        }
        this.scale = scale;
    }

    public Vector3 GetScale()
    {
        if (this.go)
        {
            return this.tran.localScale;
        }
        return this.scale;
    }

    public void SetEuler(Vector3 euler)
    {
        if (this.go)
        {
            this.tran.localEulerAngles = euler;
        }
        this.euler = euler;
    }

    public Vector3 GetEuler()
    {
        if (this.go)
        {
            return this.tran.localEulerAngles;
        }
        return this.euler;
    }

    public void SetParent(Transform parent)
    {
        if (this.go)
        {
            this.tran.parent = parent;
        }
        this.parent = parent;
        Debug.Log("setparent:" + this.parent);
    }

    public Transform GetParent()
    {
        if (this.go)
        {
            return this.tran.parent;
        }
        return this.parent;
    }

    public Component GetComponent(System.Type type)
    {
        if(this.go)
        {
            return this.go.GetComponent(type);
        }
        return null;
    }

    public Object GetComponent(string path,System.Type type)
    {
        if (this.go)
        {
            Transform t = FindTransform(path);
            return t.GetComponent(type);
        }
        return null;
    }


    public Transform FindTransform(string path)
    {
        if (this.go)
        {
            return this.tran.Find(path);
        }
        return null;
    }


    public virtual void Remove()
    {
        if(this.assetVO!=null)
        {
            this.assetVO.Remove(this.go);
            this.assetVO = null;
            this.go = null;
            this.tran = null;
        }
    }
}

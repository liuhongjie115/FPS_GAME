using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowsManager
{
    private static Stack<BasePanel> displayObjs;

    private static GameObject captureBox;
    private static GameObject captureMask;
    private static Image imgCaptureBox;

    static WindowsManager()
    {
        displayObjs = new Stack<BasePanel>();
    }

    public static void Open(BasePanel obj)
    {
        BasePanel topObj = null;
        Debug.Log("Count:"+displayObjs.Count);
        if(displayObjs.Count>0)
        {
            topObj = displayObjs.Peek();
        }
        if (topObj != null&& topObj != obj)
        {
            PausePanel(topObj);
            topObj.OnClose();
        }
        if(topObj != obj)
        {
            displayObjs.Push(obj);
            Debug.Log("Ìí¼Ó");
            obj.SetActive(true);
            if (obj.GetParent() == null)
            {
                UIOptions uiOptions = obj.GetComponent(typeof(UIOptions)) as UIOptions;
                if (uiOptions.fullPanel)
                {
                    obj.SetParent(LayerManager.RectTranFullWindows);
                }
                else
                {
                    obj.SetParent(LayerManager.RectTranWindows);
                }
            }
            obj.SetAnchoredPosition(Vector2.zero);
            obj.SetScale(Vector3.zero);
            obj.Tran.DOScale(Vector3.one, 0.2f);
            obj.OnOpen();
        }
    }

    public static void Close(BasePanel obj =null)
    {
        BasePanel topObj = null;
        if (displayObjs.Count > 0)
        {
            topObj = displayObjs.Pop();
            Debug.Log("ÒÆ³ý");
        }
        if(topObj!=null)
        {
            topObj.SetActive(false);
            topObj.OnClose();
            if ((obj != null && obj == topObj)||obj==null)
            {
                obj = topObj;
                topObj = null;
                if (displayObjs.Count > 0)
                {
                    topObj = displayObjs.Peek();
                }
                if (topObj != null)
                {
                    ResumePanel(topObj);
                    obj.OnOpen();
                }
                ClearCaptureScreen();
            }
        }
        
    }



    private static void PausePanel(BasePanel obj)
    {
        UIOptions uiOptions = obj.GetComponent(typeof(UIOptions)) as UIOptions;
        //È«ÆÁÒÆ³ýÆÁÄ»Íâ
        if (uiOptions.fullPanel)
        {
            MoveOutSceen(obj);
        }
        //·ÇÈ«ÆÁ½ØÍ¼
        else
        {
            Debug.Log("½ØÆÁ");
            CaptureScreen();
            obj.SetActive(false);
        }
    }

    private static void ResumePanel(BasePanel obj)
    {
        UIOptions uiOptions = obj.GetComponent(typeof(UIOptions)) as UIOptions;
        //È«ÆÁÒÆ½øÆÁÄ»
        if (uiOptions.fullPanel)
        {
            MoveInSceen(obj);
        }
        //·ÇÈ«ÆÁ½ØÍ¼
        else
        {
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// ÒÆ³öÆÁÄ»
    /// </summary>
    /// <param name="obj"></param>
    private static void MoveOutSceen(BasePanel obj)
    {
        obj.RealAnchoredPosition = obj.GetAnchoredPosition();
        obj.SetAnchoredPosition(new Vector2(-50000, -50000));
    }

    /// <summary>
    /// ÒÆ½øÆÁÄ»
    /// </summary>
    /// <param name="obj"></param>
    private static void MoveInSceen(BasePanel obj)
    {
        obj.SetAnchoredPosition(obj.RealAnchoredPosition);
    }

    /// <summary>
    /// ½ØÆÁ
    /// </summary>
    private static void CaptureScreen()
    {
        Rect rect = new Rect(0, 0, LayerManager.GetWidth(), LayerManager.GetHeight());
        Texture2D tex = ScreenShot(LayerManager.UiCamera, rect);
        CreateCaptureBox(tex, rect);
    }

    private static void ClearCaptureScreen()
    {
        if (imgCaptureBox)
        {
            imgCaptureBox.enabled = false;
        }
    }

    private static Texture2D ScreenShot(Camera camera, Rect rect)
    {
        if (captureMask != null)
        {
            captureMask.SetActive(false);
        }
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        if (captureMask != null)
        {
            captureMask.SetActive(true);
        }
        return screenShot;
    }

    private static void CreateCaptureBox(Texture2D tex,Rect rect)
    {
        if(captureBox==null)
        {
            captureBox = new GameObject("CreateCaptureBox");
            captureBox.transform.SetParent(LayerManager.RectTranWindows);
            captureBox.transform.localScale = Vector3.one;
            captureBox.AddComponent<RectTransform>().sizeDelta = new Vector2(LayerManager.GetWidth(), LayerManager.GetHeight());

            captureMask = new GameObject("captureMask");
            captureMask.transform.SetParent(captureBox.transform);
            captureMask.transform.localScale = Vector3.one;
            captureMask.AddComponent<RectTransform>().sizeDelta = new Vector2(LayerManager.GetWidth(), LayerManager.GetHeight());
            captureMask.AddComponent<Image>().color = new Color(0, 0, 0,200/255f);
            imgCaptureBox = captureBox.AddComponent<Image>();
            Button btn = captureBox.AddComponent<Button>();
            btn.onClick.AddListener(()=> Close());
        }
        imgCaptureBox.enabled = true;
        captureBox.transform.SetAsFirstSibling();
        imgCaptureBox.sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }
}

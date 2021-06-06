using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoModuleEditor 
{
    [MenuItem("Assets/生成C#模块")]
    public static void Init()
    {
        string moduleName = Selection.activeObject.name;
        Debug.Log(moduleName);
        Debug.Log(moduleName.Substring(moduleName.Length - 6));
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在游戏未运行状态下编辑", "确定");
            return;
        }
        if(moduleName.Substring(moduleName.Length-5).Equals("Module"))
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        }
    }
}

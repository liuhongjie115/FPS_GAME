using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoModuleEditor 
{
    [MenuItem("Assets/����C#ģ��")]
    public static void Init()
    {
        string moduleName = Selection.activeObject.name;
        Debug.Log(moduleName);
        Debug.Log(moduleName.Substring(moduleName.Length - 6));
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("����", "������Ϸδ����״̬�±༭", "ȷ��");
            return;
        }
        if(moduleName.Substring(moduleName.Length-5).Equals("Module"))
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        }
    }
}

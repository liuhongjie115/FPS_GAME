using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectUtil
{
    static Dictionary<string, System.Type> prefixType = new Dictionary<string, System.Type>();

    static GameObjectUtil()
    {
        prefixType.Add("btn", typeof(Button));
        prefixType.Add("sp", typeof(Image));
        prefixType.Add("lbl", typeof(Text));
        prefixType.Add("input", typeof(InputField));
        prefixType.Add("tab", typeof(ToggleGroup));
        prefixType.Add("toggle", typeof(Toggle));
    }

    public static Dictionary<string, System.Type> PrefixType { get => prefixType;}


    /// <summary>
    /// …Ó∂»±È¿˙
    /// </summary>
    /// <param name="go"></param>
    /// <param name="goName"></param>
    /// <returns></returns>
    public static Component FindCompoent(GameObject go, string goName)
    {
        foreach (KeyValuePair<string, System.Type> kvp in prefixType)
        {
            if (goName.Substring(0, kvp.Key.Length).Equals(kvp.Key))
            {
                System.Type type = kvp.Value;
                foreach (Transform t in go.GetComponentsInChildren<Transform>())
                {
                    if(t.name.Equals(goName))
                    {
                        return t.GetComponent(type);
                    }
                }
            }
        }
        return null;
    }

}

using UnityEngine;
using System;

[Serializable]
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;
    //{
    //    get
    //    {
    //        return panelType.ToString();
    //    }
    //    set
    //    {
    //        UIPanelType type =(UIPanelType)System.Enum.Parse(typeof(UIPanelType), value);
    //        panelType = type;
    //    }
    //}
    public string path;

    // 反序列化   从文本信息 到对象
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }

    public void OnBeforeSerialize()
    {

    }
}

[Serializable]
public class UIPanelText : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelTextType textType;
    public string textTypeString;
    public string content;

    public void OnAfterDeserialize()
    {
        UIPanelTextType type = (UIPanelTextType)System.Enum.Parse(typeof(UIPanelTextType), textTypeString);
        textType = type;
    }

    public void OnBeforeSerialize()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{

    private Text text;
    private float showTime = 1;
    private string message = null;
    private List<UIPanelTextType> msgList;

    private void Update()
    {
        if (message != null)
        {
            ShowMessage(message);
            message = null;
        }

        if (msgList != null)
        {
            ShowMessage(msgList);
            msgList = null;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        text = GetComponent<Text>();
        text.enabled = false;
        text.enabled = false;
        uiMng.InjectMessagePanel(this);
    }

    public void ShowMessageSync(string msg)
    {
        message = msg;
    }

    public void ShowMessage(string msg)
    {
        text.CrossFadeAlpha(1, 0.2f, false);
        text.text = msg;
        text.enabled = true;
        Invoke("Hide", showTime);
    }

    public void ShowMessageSync(List<UIPanelTextType> msgList)
    {
        this.msgList = msgList;
    }

    public void ShowMessage(List<UIPanelTextType> msgList)
    {
        string msg = "";
        string temp;
        Dictionary<UIPanelTextType, string> dict = uiMng.GetPanelTextDict();
        foreach (UIPanelTextType type in msgList)
        {
            dict.TryGetValue(type, out temp);
            if(temp != null)
            {
                msg += temp + " ";
            }
        }
        ShowMessage(msg);
    }

    private void Hide()
    {
        text.CrossFadeAlpha(0, 1, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Request;

public class RegisterPanel : BasePanel
{

    private InputField usernameIF;
    private InputField userpwdIF;
    private InputField reuserpwdIF;
    private RegisterRequest registerRequest;
    private bool exit = false;

    private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLable/UsernameInput").GetComponent<InputField>();
        userpwdIF = transform.Find("UserpwdLable/UserpwdInput").GetComponent<InputField>();
        reuserpwdIF = transform.Find("ReUserpwdLable/ReUserpwdInput").GetComponent<InputField>();
        transform.Find("RegistreButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }

    private void Update()
    {
        if (exit)
        {
            ExitAnim();
            usernameIF.text = "";
            userpwdIF.text = "";
            reuserpwdIF.text = "";
            uiMng.PopPanel();
            exit = false;
        }
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        EnterAnim();
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        List<UIPanelTextType> msgs = new List<UIPanelTextType>();
        if (returnCode == ReturnCode.Success)
        {
            msgs.Add(UIPanelTextType.Register_Msg0);
            exit = true;
        }
        else
        {
            msgs.Add(UIPanelTextType.Register_Msg1);
        }
        uiMng.ShowMessageSync(msgs);
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        string pattern = @"^[A-Za-z0-9_]+$";
        string pattern1 = @"^[A-Za-z0-9_#!?]+$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
        System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(pattern1);

        List<UIPanelTextType> msgs = new List<UIPanelTextType>();
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msgs.Add(UIPanelTextType.Register_Msg2);
        }
        if (usernameIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msgs.Add(UIPanelTextType.Register_Msg3);
        }
        if (!regex.IsMatch(usernameIF.text))
        {
            msgs.Add(UIPanelTextType.Register_Msg4);
        }
        if (string.IsNullOrEmpty(userpwdIF.text))
        {
            msgs.Add(UIPanelTextType.Register_Msg5);
        }
        if (userpwdIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msgs.Add(UIPanelTextType.Register_Msg6);
        }
        if (!regex1.IsMatch(userpwdIF.text))
        {
            msgs.Add(UIPanelTextType.Register_Msg7);
        }
        if (userpwdIF.text != reuserpwdIF.text)
        {
            msgs.Add(UIPanelTextType.Register_Msg8);
        }
        if (msgs.Count != 0)
        {
            uiMng.ShowMessage(msgs);
            return;
        }
        else
        {
            //发送到服务器端进行注册
            registerRequest.SendRequest(usernameIF.text, userpwdIF.text);
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        ExitAnim();
        usernameIF.text = "";
        userpwdIF.text = "";
        reuserpwdIF.text = "";
        uiMng.PopPanel();
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public override void OnSetLanguage(Dictionary<UIPanelTextType, string> panelTextDict)
    {
        string temp;
        panelTextDict.TryGetValue(UIPanelTextType.Register_NameLable, out temp);
        transform.Find("UsernameLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_NameInputHolder, out temp);
        transform.Find("UsernameLable/UsernameInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_PwdLable, out temp);
        transform.Find("UserpwdLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_PwdInputHodler, out temp);
        transform.Find("UserpwdLable/UserpwdInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_RePwdLable, out temp);
        transform.Find("ReUserpwdLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_RePwdInputHolder, out temp);
        transform.Find("ReUserpwdLable/ReUserpwdInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_RegisterBtn, out temp);
        transform.Find("RegistreButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Register_CloseBtn, out temp);
        transform.Find("CloseButton/Text").GetComponent<Text>().text = temp;
    }

    private void EnterAnim()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.5f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void ExitAnim()
    {
        transform.DOScale(0f, 0.5f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.5f);
    }
}

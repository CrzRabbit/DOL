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
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
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
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
            exit = true;
        }
        else
        {
            uiMng.ShowMessageSync("注册失败");
        }
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        string pattern = @"^[A-Za-z0-9_]+$";
        string pattern1 = @"^[A-Za-z0-9_#!?]+$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
        System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(pattern1);

        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (usernameIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msg += "用户名长度为6~20位 ";
        }
        if (!regex.IsMatch(usernameIF.text))
        {
            msg += "用户名只能为字母数字下划线 ";
        }
        if (string.IsNullOrEmpty(userpwdIF.text))
        {
            msg += "密码不能为空 ";
        }
        if (userpwdIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msg += "密码长度为6~20位 ";
        }
        if (!regex1.IsMatch(userpwdIF.text))
        {
            msg += "密码含不能使用的特殊字符 ";
        }
        if (userpwdIF.text != reuserpwdIF.text)
        {
            msg += "两次输入密码不一致 ";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
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

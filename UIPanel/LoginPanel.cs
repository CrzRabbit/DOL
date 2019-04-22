using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoginPanel : BasePanel
{

    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;

    private bool flag = false;

    private void Start()
    {
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);

        usernameIF = transform.Find("UsernameLable/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("UserpwdLable/UserpwdInput").GetComponent<InputField>();
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        usernameIF.text = "wangjiangchuan";
        passwordIF.text = "wang0010";

        loginRequest = transform.GetComponent<LoginRequest>();
    }

    private void LateUpdate()
    {
        if (flag)
        {
            uiMng.PopPanel();
            uiMng.PushPanel(UIPanelType.Menu);
            SceneManager.LoadScene(2);
            flag = false;
        }
    }

    private void Update()
    {

    }

    public override void OnEnter()
    {
        EnterAnim();
    }

    public override void OnPause()
    {
        HideAnim();
    }

    public override void OnResume()
    {
        EnterAnim();
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public override void OnSetLanguage(Dictionary<UIPanelTextType, string> panelTextDict)
    {
        //transform.Find("UsernameLable").GetComponent<Text>().text = panelTextDict.TryGetValue
        string temp;
        panelTextDict.TryGetValue(UIPanelTextType.Login_NameLable, out temp);
        transform.Find("UsernameLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_NameInputHolder, out temp);
        transform.Find("UsernameLable/UsernameInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_PwdLable, out temp);
        transform.Find("UserpwdLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_PwdInputHolder, out temp);
        transform.Find("UserpwdLable/UserpwdInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_LoginBtn, out temp);
        transform.Find("LoginButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_RegistreBtn, out temp);
        transform.Find("RegisterButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Login_CloseBtn, out temp);
        transform.Find("CloseButton/Text").GetComponent<Text>().text = temp;
    }

    private void OnLoginClick()
    {
        PlayClickSound();

        string pattern = @"^[A-Za-z0-9_]+$";
        string pattern1 = @"^[A-Za-z0-9_#!?]+$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
        System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(pattern1);

        List<UIPanelTextType> msgs = new List<UIPanelTextType>();
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msgs.Add(UIPanelTextType.Login_Msg0);
        }
        if (usernameIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msgs.Add(UIPanelTextType.Login_Msg1);
        }
        if (!regex.IsMatch(usernameIF.text))
        {
            msgs.Add(UIPanelTextType.Login_Msg2);
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msgs.Add(UIPanelTextType.Login_Msg3);
        }
        if (passwordIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msgs.Add(UIPanelTextType.Login_Msg4);
        }
        if (!regex1.IsMatch(passwordIF.text))
        {
            msgs.Add(UIPanelTextType.Login_Msg5);
        }
        if (msgs.Count != 0)
        {
            uiMng.ShowMessage(msgs);
            return;
        }
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            flag = true;
        }
        else
        {
            List<UIPanelTextType> msgs = new List<UIPanelTextType>();
            msgs.Add(UIPanelTextType.Login_Msg6);
            uiMng.ShowMessageSync(msgs);
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
        //打开的动画
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.5f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void HideAnim()
    {
        transform.DOScale(0f, 0.5f);
        transform.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}

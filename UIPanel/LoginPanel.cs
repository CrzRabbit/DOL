using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void OnLoginClick()
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
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空 ";
        }
        if (passwordIF.text.Length < 6 || usernameIF.text.Length > 20)
        {
            msg += "密码长度为6~20位 ";
        }
        if (!regex1.IsMatch(passwordIF.text))
        {
            msg += "密码含不能使用的特殊字符 ";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
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
            uiMng.ShowMessageSync("用户名或密码，无法登录，请重新输入!");
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

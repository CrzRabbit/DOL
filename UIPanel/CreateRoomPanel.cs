using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Model;
using UnityEngine.SceneManagement;

public class CreateRoomPanel : BasePanel
{
    private CreateRoomRequest createRoomRequest;
    private InputField nameText;
    private InputField pwdText;
    private Dropdown sceneDropdown;
    private Dropdown levelDropdown;
    private Dropdown maxCountDropdown;

    private RoomInfo roomInfo;
    private bool startServerFlag = false;

    private void Start()
    {
        createRoomRequest = GetComponent<CreateRoomRequest>();
        nameText = transform.Find("RoomNameLable/RoomNameInput").GetComponent<InputField>();
        pwdText = transform.Find("RoomPwdLable/RoomPwdInput").GetComponent<InputField>();
        sceneDropdown = transform.Find("RoomSceneLable/Dropdown").GetComponent<Dropdown>();
        levelDropdown = transform.Find("RoomLevelLable/Dropdown").GetComponent<Dropdown>();
        maxCountDropdown = transform.Find("RoomMaxCountLable/Dropdown").GetComponent<Dropdown>();

        transform.Find("CreateButton").GetComponent<Button>().onClick.AddListener(OnCreateClick);
        transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(OnCancelClick);
    }

    private void Update()
    {
        if (startServerFlag)
        {
            uiMng.ClearPanel();
            uiMng.PushPanel(UIPanelType.RoomReady);
            //SceneManager.LoadScene(3);
            startServerFlag = false;
            facade.StartServer(roomInfo.RoomIp, roomInfo.RoomPort);
        }
    }

    private void LateUpdate()
    {
        if(startServerFlag)
        {

        }
    }

    private void OnCreateClick()
    {
        string pattern = @"^[A-Za-z0-9_]+$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

        string pattern1 = @"^[0-9]+$";
        System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(pattern1);

        string name = nameText.text;
        string msg = "";
        if (name.Equals(""))
        {
            msg += "房间名不能为空 ";
        }
        if (name.Length > 20)
        {
            msg += "房间名过长 ";
        }
        if (!regex.IsMatch(name))
        {
            msg += "房间名必须为字符数字下划线 ";
        }
        string owner = uiMng.GetPlayerInfo().PlayerName;
        string pwd = pwdText.text;
        if ((pwd.Length != 0 && pwd.Length != 6) || (pwd.Length != 0 && !regex1.IsMatch(pwd)))
        {
            msg += "房间密码为空或者六位数字 ";
        }
        else if (pwd.Length == 0)
        {
            pwd = "@";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }
        string ip = Network.player.ipAddress;
        int port = 20010;
        int scene = sceneDropdown.value;
        int state = 0;
        int level = levelDropdown.value;
        int curCount = 1;
        int maxCount = 2 * (maxCountDropdown.value + 2);
        roomInfo = new RoomInfo(0, name, owner, pwd, ip, port, scene, state, level, curCount, maxCount);
        createRoomRequest.SendRequest(name, owner, pwd, ip, port, scene, state, level, curCount, maxCount);
    }

    private void OnCancelClick()
    {
        //OnResponse(ReturnCode.Success);
        OnExit();
        uiMng.EnterLobby();
        //uiMng.PopPanel();
    }

    public void OnResponse(ReturnCode retCode)
    {
        if (ReturnCode.Success == retCode)
        {
            uiMng.ShowMessageSync("创建房间成功");
            startServerFlag = true;
        }
        if (ReturnCode.Fail == retCode)
        {
            uiMng.ShowMessageSync("创建房间失败");
        }
    }

    public override void OnEnter()
    {
        //Start();
        base.OnEnter();
        gameObject.SetActive(true);
        EnterAnim();
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
        ExitAnim();
    }

    private void EnterAnim()
    {
        //transform.localScale = Vector3.zero;
        //transform.DOScale(1f, 0.5f);
        transform.localPosition = new Vector3(-1300, 100, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void ExitAnim()
    {
        //transform.DOScale(0f, 0.5f);
        Tweener tweener = transform.DOLocalMove(new Vector3(-1300, 100, 0), 0.5f);
    }
}

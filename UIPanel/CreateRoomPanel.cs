using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Model;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

        nameText.text = "test1";

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
        List<UIPanelTextType> msgs = new List<UIPanelTextType>();
        if (name.Equals(""))
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg0);
        }
        if (name.Length > 20)
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg1);
        }
        if (!regex.IsMatch(name))
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg2);
        }
        string owner = uiMng.GetPlayerInfo().PlayerName;
        string pwd = pwdText.text;
        if ((pwd.Length != 0 && pwd.Length != 6) || (pwd.Length != 0 && !regex1.IsMatch(pwd)))
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg3);
        }
        else if (pwd.Length == 0)
        {
            pwd = "@";
        }
        if (msgs.Count != 0)
        {
            uiMng.ShowMessage(msgs);
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
        List<UIPanelTextType> msgs = new List<UIPanelTextType>();
        if (ReturnCode.Success == retCode)
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg4);
            startServerFlag = true;
        }
        if (ReturnCode.Fail == retCode)
        {
            msgs.Add(UIPanelTextType.CreateRoom_Msg5);
        }
        uiMng.ShowMessageSync(msgs);
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

    public override void OnSetLanguage(Dictionary<UIPanelTextType, string> panelTextDict)
    {
        string temp;
        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_NameLable, out temp);
        transform.Find("RoomNameLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_NameInputHolder, out temp);
        transform.Find("RoomNameLable/RoomNameInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_PwdLable, out temp);
        transform.Find("RoomPwdLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_PwdInputLable, out temp);
        transform.Find("RoomPwdLable/RoomPwdInput/Placeholder").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_SceneLable, out temp);
        transform.Find("RoomSceneLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_LevelLable, out temp);
        transform.Find("RoomLevelLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_MaxCountLable, out temp);
        transform.Find("RoomMaxCountLable").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_CreatBtn, out temp);
        transform.Find("CreateButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.CreateRoom_CancleBtn, out temp);
        transform.Find("CancelButton/Text").GetComponent<Text>().text = temp;
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

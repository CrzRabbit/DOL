using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Model;
using System.Collections;
using System.Threading;

public class RoomItemPanel : BasePanel
{
    private RoomInfo roomInfo;
    private Image lockState;
    private Text roomName;
    private Text roomScene;
    private Text roomOwner;
    private Text roomLevel;
    private Text roomCurCount;
    private Text roomMaxCount;
    private int state = 0;
    private IEnumerator coroutine;
    private RoomListPanel roomListPanel;

    private string ip;
    private string port;

    public void Start()
    {
        if (lockState == null)
        {
            GetComponent<Button>().onClick.AddListener(OnItemClick);
        }
        lockState = transform.Find("LockStateImg").GetComponent<Image>();
        roomName = transform.Find("RoomName").GetComponent<Text>();
        roomScene = transform.Find("RoomScene").GetComponent<Text>();
        roomOwner = transform.Find("RoomOwner").GetComponent<Text>();
        roomLevel = transform.Find("RoomLevel").GetComponent<Text>();
        roomCurCount = transform.Find("RoomCurCount").GetComponent<Text>();
        roomMaxCount = transform.Find("RoomMaxCount").GetComponent<Text>();

        coroutine = ResumeState(1.0f);
    }

    public void SetInfo(RoomInfo roomInfo, RoomListPanel roomListPanel)
    {
        this.roomInfo = roomInfo;
        this.roomListPanel = roomListPanel;
        ShowInfo();
    }

    private void ShowInfo()
    {
        string color = "";
        if (roomInfo.RoomCurCount == roomInfo.RoomMaxCount)
        {
            color = "#787878";
        }
        else if (roomInfo.RoomState == 0)
        {
            color = "#23d24c";
        }
        else if (roomInfo.RoomState == 1)
        {
            color = "#fe2f2f";
        }
        else
        {
            color = "#323232";
        }

        string formatStr = "<color={0}> {1} </color>";

        this.roomName.text = string.Format(formatStr, color, this.roomInfo.RoomName);
        this.roomOwner.text = string.Format(formatStr, color, this.roomInfo.RoomOwner);
        this.roomScene.text = string.Format(formatStr, color, this.roomInfo.RoomScene);
        this.roomLevel.text = string.Format(formatStr, color, this.roomInfo.RoomLevel);
        this.roomCurCount.text = string.Format(formatStr, color, this.roomInfo.RoomCurCount);
        this.roomMaxCount.text = string.Format(formatStr, color, this.roomInfo.RoomMaxCount);


        this.ip = this.roomInfo.RoomIp;
        this.port = this.roomInfo.RoomPort.ToString();
    }

    private void OnItemClick()
    {
        if (state == 0)
        {
            roomListPanel.ChooseItem(this);
            state = 1;
            StartCoroutine(ResumeState(1.0f));
        }
        else if(state == 1)
        {
            state = 0;
            //TODO: double click

            JoinRoom();
        }
    }

    public void JoinRoom()
    {
        int curCount = int.Parse(roomCurCount.text.Split(' ')[1]);
        int maxCount = int.Parse(roomMaxCount.text.Split(' ')[1]);
        if (curCount >= maxCount)
        {
            uiMng.ShowMessage("房间已满");
            return;
        }
        facade.ConnectServer(ip, int.Parse(port));
        uiMng.PushPanelSync(UIPanelType.RoomReady);
        //facade.SendPlayerInfo();
    }

    private IEnumerator ResumeState(float time)
    {
        yield return new WaitForSeconds(time);
        state = 0;
    }

    public override void OnEnter()
    {
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
        //transform.localPosition = new Vector3(-1300, 100, 0);
        //transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void ExitAnim()
    {
        //transform.DOScale(0f, 0.5f);
        //Tweener tweener = transform.DOLocalMove(new Vector3(-1300, 100, 0), 0.5f);
    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}

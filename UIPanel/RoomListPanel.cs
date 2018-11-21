using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Model;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomListPanel : BasePanel
{
    private VerticalLayoutGroup content;
    private GameObject roomItemPrefab;
    private ListRoomRequest listRoomRequest;
    private RoomItemPanel targetRoomItem;

    private void Start()
    {
        if (!content)
        {
            content = transform.Find("RoomList/Content").GetComponent<VerticalLayoutGroup>();
            transform.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
            transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(OnBackClick);
            transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(OnJoinClick);
            listRoomRequest = GetComponent<ListRoomRequest>();
            roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
            transform.Find("Scrollbar_Real").GetComponent<Scrollbar>().value = 0;
        }
    }

    public void LoadRoomList()
    {
        if(!content)
        {
            Start();
        }

        foreach(RoomItemPanel roomItem in content.GetComponentsInChildren<RoomItemPanel>())
        {
            roomItem.DestroySelf();
        }

        List<RoomInfo> infos = uiMng.GetRoomsInfo();

        if (infos != null && infos.Count > 0)
        {
            foreach (RoomInfo roomInfo in infos)
            {
                GameObject roomItem = Instantiate(roomItemPrefab) as GameObject;
                roomItem.transform.SetParent(content.transform);
                roomItem.GetComponent<RoomItemPanel>().Start();
                roomItem.GetComponent<RoomItemPanel>().SetInfo(roomInfo, this);
                roomItem.GetComponent<BasePanel>().UIMng = uiMng;
                roomItem.GetComponent<BasePanel>().Facade = facade;
            }
        }
        transform.Find("Scrollbar_Real").GetComponent<Scrollbar>().value = 0;
        int roomCount = GetComponentsInChildren<RoomItemPanel>().Length;
        Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + content.spacing));
    }

    private void Update()
    {
        
    }

    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }

    private void OnJoinClick()
    {
        targetRoomItem.JoinRoom();
    }

    private void OnBackClick()
    {
        uiMng.EnterLobby();
    }

    public void ChooseItem(RoomItemPanel roomItemPanel)
    {
        //TODO: change target room item and changed bk color for it
        this.targetRoomItem = roomItemPanel;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        uiMng.InjectRoomListPanel(this);
        uiMng.ShowRoomsInfo();
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

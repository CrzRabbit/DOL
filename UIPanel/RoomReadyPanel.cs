using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Model;
using UnityEngine.UI;

public class RoomReadyPanel : BasePanel
{
    private VerticalLayoutGroup content;
    private GameObject playerItemPrefab;
    private UpdateRoomRequest updateRoomRequest;
    private RemoveRoomRequest removeRoomRequest;
    private Button gameStartButton;
    private Text readyStateText;
    private RoomPlayerInfo roomPlayerInfo;

    private void Start()
    {
        if(content)
        {
            return;
        }
        content = transform.Find("PlayerList").GetComponent<VerticalLayoutGroup>();
        transform.Find("ReadyButton").GetComponent<Button>().onClick.AddListener(OnReadyClick);
        transform.Find("GameStartButton").GetComponent<Button>().onClick.AddListener(OnGameStartClick);
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(OnBackClick);
        playerItemPrefab = Resources.Load("UIPanel/PlayerItem") as GameObject;
        gameStartButton = transform.Find("GameStartButton").GetComponent<Button>();
        readyStateText = transform.Find("ReadyButton/Text").GetComponent<Text>();
        updateRoomRequest = GetComponent<UpdateRoomRequest>();
        removeRoomRequest = GetComponent<RemoveRoomRequest>();
        uiMng.ClearPanel(UIPanelType.CreateRoom);
        uiMng.ClearPanel(UIPanelType.ListRoom);
    }

    public void LoadPlayersInfo()
    {
        if (!content)
        {
            Start();
        }

        foreach (PlayerItemPanel playerItem in content.GetComponentsInChildren<PlayerItemPanel>())
        {
            playerItem.DestroySelf();
        }

        foreach (RoomPlayerInfo roomPlayerInfo in facade.GetRoomPlayersInfo())
        {
            GameObject roomPlayerItem = Instantiate(playerItemPrefab) as GameObject;
            roomPlayerItem.transform.SetParent(content.transform);
            roomPlayerItem.GetComponent<PlayerItemPanel>().Start();
            roomPlayerItem.GetComponent<PlayerItemPanel>().SetInfo(roomPlayerInfo, this);
        }

        int roomCount = GetComponentsInChildren<RoomItemPanel>().Length;
        Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (playerItemPrefab.GetComponent<RectTransform>().sizeDelta.y + content.spacing));
    }

    public void SetRoomPlayerInfo(RoomPlayerInfo roomPlayerInfo)
    {
        this.roomPlayerInfo = roomPlayerInfo;
        gameStartButton.gameObject.SetActive(this.roomPlayerInfo.isRoomOwner);
        if (facade.GetCurrentRoomInfo().RoomState == 1)
        {
            readyStateText.text = "Join";
            return;
        }
        readyStateText.text = this.roomPlayerInfo.readyState ? "Not Ready" : "Ready";
    }

    private void Update()
    {

    }

    private void OnReadyClick()
    {
        if (readyStateText.text == "Join")
        {
            facade.JoinGame();
        }
        else
        {
            facade.PlayerReady(roomPlayerInfo.readyState);
        }
    }

    private void OnGameStartClick()
    {
        facade.GameStart();
    }

    private void OnBackClick()
    {
        facade.DisconnectServer();
        uiMng.EnterLobby();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        uiMng.InjectRoomReadyPanel(this);
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
}

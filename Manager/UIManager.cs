using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Model;

public class UIManager : BaseManager
{
    private int languageTypeCount = 2;
    private Transform canvasTransform;
    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
    private Dictionary<UIPanelTextType, string> panelTextDict;//存储面板文本
    private UIPanelLanguageType languageType = UIPanelLanguageType.English;
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private UIPanelType panelTypeToPush = UIPanelType.None;
    private Stack<BasePanel> panelStack;

    private MessagePanel msgPanel;

    private PlayerInfoPanel playerInfoPanel;
    private PlayerInfo playerInfo;
    private bool showPlayerInfoFlag = false;

    private List<RoomInfo> roomsInfo;
    private RoomListPanel roomListPanel;
    private bool showRoomsInfoFlag = false;
    private bool clearPanelFlag = false;

    private RoomReadyPanel roomReadyPanel;
    private bool enterRoomReadyPanelFlag = false;

    private bool enterLobbyFlag = false;

    public UIManager(GameFacade facade) : base(facade)
    {
        ParseUIPanelTypeJson();
        SetLanguage(UIPanelLanguageType.Chinese);
    }

    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Login);
        roomsInfo = new List<RoomInfo>();
    }

    public override void Update()
    {
        if (clearPanelFlag)
        {
            ClearPanel();
            clearPanelFlag = false;
        }
        if (panelTypeToPush != UIPanelType.None)
        {
            if (panelTypeToPush == UIPanelType.RoomReady)
            {
                ClearPanel();
            }
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }
        if (showPlayerInfoFlag)
        {
            ShowPlayerInfo();
            showPlayerInfoFlag = false;
        }
        if (showRoomsInfoFlag)
        {
            ShowRoomsInfo();
            showRoomsInfoFlag = false;
        }
        if (enterRoomReadyPanelFlag)
        {
            EnterRoomReadyPanel();
            enterRoomReadyPanelFlag = false;
        }
        if (enterLobbyFlag)
        {
            EnterLobby();
            enterLobbyFlag = false;
        }
    }

    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }

    public void PushPanelSync(UIPanelType panelType)
    {
        panelTypeToPush = panelType;
    }

    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
    }

    public void ClearPanelSync()
    {
        clearPanelFlag = true;
    }

    public void ClearPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭页面的显示
        while (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Pop();
            if (topPanel)
            {
                topPanel.OnExit();
            }
            else
            {
                break;
            }
        }
    }

    public void ClearPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = null;
        panelDict.TryGetValue(panelType, out panel);

        if(panel)
        {
            panel.OnExit();
        }
    }

    public void EnterLobby()
    {
        ClearPanel();
        PushPanel(UIPanelType.Menu);
        PushPanel(UIPanelType.Room);
        PushPanel(UIPanelType.PlayerInfo);
    }

    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);//TODO

        BasePanel panel = null;
        panelDict.TryGetValue(panelType, out panel);

        if (panel == null)
        {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            string path;
            panelPathDict.TryGetValue(panelType, out path);
            //string path = panelPathDict.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(GameObject.Find("Canvas(Clone)").transform, false);
            instPanel.GetComponent<BasePanel>().UIMng = this;
            instPanel.GetComponent<BasePanel>().Facade = facade;
            instPanel.GetComponent<BasePanel>().OnSetLanguage(panelTextDict);
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }

    }

    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList)
        {
            panelPathDict.Add(info.panelType, info.path);
        }
    }

    [Serializable]
    class UIPanelTextTypeJson
    {
        public List<UIPanelText> textList;
    }

    private void ParseUIPanelTextTypeJson(UIPanelLanguageType type)
    {
        panelTextDict = new Dictionary<UIPanelTextType, string>();
        TextAsset ta = null;
        switch (type)
        {
            case UIPanelLanguageType.Chinese:
                ta = Resources.Load<TextAsset>("UIPanelText_Chinese");
                break;
            case UIPanelLanguageType.English:
                ta = Resources.Load<TextAsset>("UIPanelText_English");
                break;
            default:
                break;
        }
        UIPanelTextTypeJson jsonObject = JsonUtility.FromJson<UIPanelTextTypeJson>(ta.text);
        foreach (UIPanelText text in jsonObject.textList)
        {
            panelTextDict.Add(text.textType, text.content);
        }
    }

    public Dictionary<UIPanelTextType, string> GetPanelTextDict()
    {
        return panelTextDict;
    }

    public void SetLanguage(UIPanelLanguageType type)
    {
        ParseUIPanelTextTypeJson(type);

        if (panelDict == null)
        {
            return;
        }

        foreach (BasePanel panel in panelDict.Values)
        {
            panel.OnSetLanguage(panelTextDict);
        }
    }

    public BasePanel TopPanel()
    {
        if (panelStack.Count > 0)
        {
            return panelStack.Peek();
        }
        return null;
    }

    //Message Panel
    public void InjectMessagePanel(MessagePanel messagePanel)
    {
        this.msgPanel = messagePanel;
    }

    public void ShowMessage(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，message panel为空");
            return;
        }
        msgPanel.ShowMessage(msg);
    }

    public void ShowMessageSync(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，message panel为空");
            return;
        }
        msgPanel.ShowMessageSync(msg);
    }

    public void ShowMessage(List<UIPanelTextType> msgs)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，message panel为空");
            return;
        }
        msgPanel.ShowMessage(msgs);
    }

    public void ShowMessageSync(List<UIPanelTextType> msgs)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，message panel为空");
            return;
        }
        msgPanel.ShowMessageSync(msgs);
    }

    //PlayerInfo Panel
    public void InjectPlayerInfoPanel(PlayerInfoPanel playerInfoPanel)
    {
        this.playerInfoPanel = playerInfoPanel;
    }

    public void SetPlayerInfo(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        ShowPlayerInfo();
    }

    public void SetPlayerInfoSync(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        showPlayerInfoFlag = true;
    }

    public PlayerInfo GetPlayerInfo()
    {
        return playerInfo;
    }

    public void ShowPlayerInfo()
    {
        if (playerInfoPanel)
        {
            playerInfoPanel.ShowPlayerInfo(playerInfo);
        }
    }
    //RoomList Panel
    public void InjectRoomListPanel(RoomListPanel roomListPanel)
    {
        this.roomListPanel = roomListPanel;
    }

    public void SetRoomsInfo(List<RoomInfo> roomsInfo)
    {
        this.roomsInfo = roomsInfo;
        ShowRoomsInfoSync();
    }

    public List<RoomInfo> GetRoomsInfo()
    {
        return roomsInfo;
    }

    public void ShowRoomsInfo()
    {
        if (roomListPanel)
        {
            roomListPanel.LoadRoomList();
        }
    }

    public void ShowRoomsInfoSync()
    {
        showRoomsInfoFlag = true;
    }
    //RoomReady Panel
    public void InjectRoomReadyPanel(RoomReadyPanel roomReadyPanel)
    {
        this.roomReadyPanel = roomReadyPanel;
    }

    public void EnterRoomReadyPanelSync()
    {
        enterRoomReadyPanelFlag = true;
    }

    public void EnterRoomReadyPanel()
    {
        ClearPanel();
        PushPanelSync(UIPanelType.RoomReady);
    }

    public void ShowPlayersInfo()
    {
        roomReadyPanel.LoadPlayersInfo();
    }

    public void SetRoomPlayerInfo(RoomPlayerInfo roomPlayerInfo)
    {
        roomReadyPanel.SetRoomPlayerInfo(roomPlayerInfo);
    }
    // enter lobby

    public void EnterLobbySync()
    {
        enterLobbyFlag = true;
    }
}
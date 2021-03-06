﻿using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Model;
using System.Collections.Generic;

public class GameFacade : MonoBehaviour
{

    private static GameFacade _instance;
    public static GameFacade Instance { get { return _instance; } }

    public Canvas canvas;
    private UIManager uiManager;
    //private AudioManager audioManager;
    //private PlayerManager playerManager;
    //private CameraManager cameraManager;
    private RequestManager requestManager;
    private SocketManager socketManager;
    private NetManager netManager;
    private GameObject gM;
    private bool isEnterPlaying;

    private void Awake()
    {
        if (null != _instance)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        InitManager();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateManager();
        if (isEnterPlaying)
        {
            //EnterPlaying();
            isEnterPlaying = false;
        }
    }

    private void InitManager()
    {
        if(uiManager != null)
        {
            return;
        }
        uiManager = new UIManager(this);
        //audioManager = new AudioManager(this);
        //playerManager = new PlayerManager(this);
        //cameraManager = new CameraManager(this);
        requestManager = new RequestManager(this);
        socketManager = new SocketManager(this);
        netManager = new NetManager(this);

        gM = GameObject.Find("GameManager(Clone)") as GameObject;
        gM.SetActive(false);

        uiManager.OnInit();
        //audioManager.OnInit();
        //playerManager.OnInit();
        //cameraManager.OnInit();
        requestManager.OnInit();
        socketManager.OnInit();
        netManager.OnInit();
    }

    private void DestroyManager()
    {
        if(uiManager == null)
        {
            return;
        }

        uiManager.OnDestroy();
        //audioManager.OnDestroy();
        //playerManager.OnDestroy();
        //cameraManager.OnDestroy();
        requestManager.OnDestroy();
        socketManager.OnDestroy();
        netManager.OnDestroy();
    }

    private void UpdateManager()
    {
        uiManager.Update();
        //audioManager.Update();
        //playerManager.Update();
        //cameraManager.Update();
        //requestManager.Update();
        socketManager.Update();
    }

    private void OnDestroy()
    {
        DestroyManager();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        if(requestManager == null)
        {
            InitManager();
        }
        requestManager.AddRequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        socketManager.SendRequest(requestCode, actionCode, data);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestManager.HandleResponse(actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        uiManager.ShowMessage(msg);
    }

    public void PlayBgSound(string soundName)
    {
        //audioManager.PlayBgSound(soundName);
    }

    public void PlayNormalSound(string soundName)
    {
        //audioManager.PlayNormalSound(soundName);
    }
    //UI相关
    public void SetPlayerInfo(PlayerInfo info)
    {
        uiManager.SetPlayerInfoSync(info);
    }

    public PlayerInfo GetPlayerInfo()
    {
        return uiManager.GetPlayerInfo();
    }

    public void SetRoomsInfo(List<RoomInfo> roomsInfo)
    {
        uiManager.SetRoomsInfo(roomsInfo);
    }

    public void ClearPanel()
    {
        uiManager.ClearPanelSync();
    }
    //服务器相关
    public void StartServer(string ip, int port)
    {
        netManager.StartServer(ip, port);
    }

    public void ConnectServer(string ip, int port)
    {
        netManager.ConnectServer(ip, port);
    }

    public void DisconnectServer()
    {
        netManager.DisconnectServer();
    }

    public List<RoomPlayerInfo> GetRoomPlayersInfo()
    {
        return netManager.GetRoomPlayersInfo();
    }

    public void ShowPlayersInfo()
    {
        uiManager.ShowPlayersInfo();
    }

    public void SendPlayerInfo()
    {
        netManager.SendPlayerInfo();
    }

    //设置当前房间信息
    public void SetCurrentRoomInfo(RoomInfo roomInfo)
    {
        netManager.SetCurrentRoomInfo(roomInfo);
    }

    public RoomInfo GetCurrentRoomInfo()
    {
        return netManager.GetCurrentRoomInfo();
    }

    public void SetRoomPlayerInfo(RoomPlayerInfo roomPlayerInfo)
    {
        uiManager.SetRoomPlayerInfo(roomPlayerInfo);
    }

    public void PlayerReady(bool readyState)
    {
        netManager.PlayerReady(readyState);
    }

    public void GameStart()
    {
        netManager.GameStart();
    }

    public void JoinGame()
    {
        netManager.JoinGame();
    }
    //UI
    public void PushPanelSync(UIPanelType uiPanelType)
    {
        uiManager.PushPanelSync(uiPanelType);
    }

    public void PopPanel()
    {
        uiManager.PopPanel();
    }

    public void EnterLobbySync()
    {
        uiManager.EnterLobbySync();
    }

    //Game
    public void StartGameManager()
    {
        gM.SetActive(true);
    }

    public void GameOver()
    {
        gM.SetActive(false);
        netManager.GameOver();
    }
}

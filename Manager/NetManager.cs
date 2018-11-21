using UnityEngine.Networking;
using UnityEngine;
using Assets.Scripts.Model;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

public class NetManager : NetworkManager
{
    private Dictionary<NetworkConnection, RoomPlayerInfo> roomPlayerInfoDict;
    private List<RoomPlayerInfo> roomPlayerInfos;
    private RoomPlayerInfo roomPlayerInfo;
    private RoomInfo preRoomInfo;
    private RoomInfo currentRoomInfo;
    private PlayerInfo playerInfo;
    private NetworkClient myClient = null;
    private GameFacade facade;
    private UpdateRoomRequest updateRoomRequest;
    private RemoveRoomRequest removeRoomRequest;
    private bool isServer = false;

    private bool flag = false;
    GameObject player;

    public NetManager(GameFacade facade)
    {
        this.facade = facade;
    }

    public void OnInit()
    {
        //logLevel = (LogFilter.FilterLevel)LogFilter.Error;

        roomPlayerInfoDict = new Dictionary<NetworkConnection, RoomPlayerInfo>();
        roomPlayerInfos = new List<RoomPlayerInfo>();

        GameObject obj = GameObject.Find("RequestProxy(Clone)");

        updateRoomRequest = obj.GetComponent<UpdateRoomRequest>();
        updateRoomRequest.SetNetManager(this);
        removeRoomRequest = obj.GetComponent<RemoveRoomRequest>();
        removeRoomRequest.SetNetManager(this);

        playerPrefab = Resources.Load("PlayerPrefab/Player") as GameObject;

        onlineScene = "Map1";
        offlineScene = "Lobby";
    }

    public void OnDestroy()
    {

    }
    /*
     ********************************************************
     *                                                      *
     *                                                      *
     *                        SERVER                        *
     *                                                      *
     *                                                      *
     *                                                      *
     ********************************************************
     */
    public void StartServer(string ip, int port)
    {
        autoCreatePlayer = false;
        isServer = true;
        networkAddress = ip;
        networkPort = port;
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
        NetworkServer.RegisterHandler(NetMessageType.PlayerInfo, OnPlayerInfo);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
        NetworkServer.RegisterHandler(NetMessageType.Ready, OnServerPlayerReady);
        NetworkServer.RegisterHandler(NetMessageType.Start, OnServerGameStart);
        NetworkServer.RegisterHandler(NetMessageType.AddPlayer, OnServerAddPlayer);
        NetworkServer.Listen(port);
        ServerChangeScene("Map1");
        ConnectLocalServer(ip, port);
    }

    public void ConnectLocalServer(string ip, int port)
    {
        playerInfo = facade.GetPlayerInfo();
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnClientConnect);
        myClient.RegisterHandler(NetMessageType.PlayersInfo, OnPlayersInfo);
        myClient.RegisterHandler(NetMessageType.RoomInfo, OnCurrentRoomInfo);
        myClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
        myClient.RegisterHandler(NetMessageType.Ready, OnClientPlayerReady);
        myClient.RegisterHandler(NetMessageType.Start, OnClientGameStart);
        PlayerInfoMessage playerInfoMsg = new PlayerInfoMessage();
        roomPlayerInfo = new RoomPlayerInfo(playerInfo.PlayerName, ip, port, 0, playerInfo.PlayerLevel, false, true);
        playerInfoMsg.roomPlayerInfo = roomPlayerInfo;
        myClient.Send(NetMessageType.PlayerInfo, playerInfoMsg);
    }

    public void OnServerConnect(NetworkMessage netMsg)
    {
        //TODO: Client connected
        if(myClient == null || netMsg.conn == myClient.connection)
        {
            return;
        }
        if (currentRoomInfo.RoomCurCount + 1 <= currentRoomInfo.RoomMaxCount)
        {
            preRoomInfo = currentRoomInfo;
            currentRoomInfo.RoomCurCount += 1;
            updateRoomRequest.SendRequest(currentRoomInfo.RoomIndex, currentRoomInfo.RoomName, currentRoomInfo.RoomOwner, currentRoomInfo.RoomPwd, currentRoomInfo.RoomIp, currentRoomInfo.RoomPort, currentRoomInfo.RoomScene,
             currentRoomInfo.RoomState, currentRoomInfo.RoomLevel, currentRoomInfo.RoomCurCount, currentRoomInfo.RoomMaxCount);
        }
        SendCurrentRoomInfo();
    }

    //Send to client
    public void SendCurrentRoomInfo()
    {
        RoomInfoMessage roomInfoMessage = new RoomInfoMessage();
        roomInfoMessage.roomInfo = currentRoomInfo;
        NetworkServer.SendToAll( NetMessageType.RoomInfo, roomInfoMessage);
    }

    //handle message from client
    public void OnPlayerInfo(NetworkMessage netMsg)
    {
        PlayerInfoMessage playerInfoMsg = new PlayerInfoMessage();
        playerInfoMsg.Deserialize(netMsg.reader);
        RoomPlayerInfo playerInfo = playerInfoMsg.roomPlayerInfo;
        roomPlayerInfoDict.Add(netMsg.conn, playerInfo);
        roomPlayerInfos.Add(playerInfo);
        SendPlayersInfoToAll();
    }

    private void SendPlayersInfoToAll()
    {
        PlayersInfoMessage playersInfoMessage = new PlayersInfoMessage();
        playersInfoMessage.count = roomPlayerInfos.Count;
        playersInfoMessage.roomPlayerInfos = roomPlayerInfos;
        NetworkServer.SendToAll(NetMessageType.PlayersInfo, playersInfoMessage);
    }

    public void OnServerDisconnect(NetworkMessage netMsg)
    {
        if (currentRoomInfo.RoomCurCount >= 1)
        {
            preRoomInfo = currentRoomInfo;
            currentRoomInfo.RoomCurCount -= 1;
            updateRoomRequest.SendRequest(currentRoomInfo.RoomIndex, currentRoomInfo.RoomName, currentRoomInfo.RoomOwner, currentRoomInfo.RoomPwd, currentRoomInfo.RoomIp, currentRoomInfo.RoomPort, currentRoomInfo.RoomScene,
         currentRoomInfo.RoomState, currentRoomInfo.RoomLevel, currentRoomInfo.RoomCurCount, currentRoomInfo.RoomMaxCount);
        }

        NetworkConnection conn = netMsg.conn;
        RoomPlayerInfo rpInfo;
        roomPlayerInfoDict.TryGetValue(conn, out rpInfo);
        NetworkServer.SetClientNotReady(conn);
        roomPlayerInfoDict.Remove(conn);

        foreach (RoomPlayerInfo info in roomPlayerInfos)
        {
            if (info.name == rpInfo.name && info.ip == rpInfo.ip && info.port == rpInfo.port)
            {
                roomPlayerInfos.Remove(info);
                break;
            }
        }
        SendPlayersInfoToAll();
        NetworkServer.DestroyPlayersForConnection(conn);
    }

    public void OnServerPlayerReady(NetworkMessage netMsg)
    {
        NetworkConnection conn = netMsg.conn;
        RoomPlayerInfo rpInfo;
        roomPlayerInfoDict.TryGetValue(conn, out rpInfo);
        roomPlayerInfoDict.Remove(conn);
        int index = 0;
        for (; index < roomPlayerInfos.Count; ++index)
        {
            RoomPlayerInfo info = roomPlayerInfos[index];
            if (info.name == rpInfo.name && info.ip == rpInfo.ip && info.port == rpInfo.port)
            {
                roomPlayerInfos.Remove(info);
                break;
            }
        }
        rpInfo.readyState = !rpInfo.readyState;
        PlayerReadyMessage playerReadyMessage = new PlayerReadyMessage();
        playerReadyMessage.readyState = rpInfo.readyState;
        if (rpInfo.readyState)
        {
            NetworkServer.SetClientReady(conn);
        }
        else
        {
            NetworkServer.SetClientNotReady(conn);
        }
        roomPlayerInfoDict.Add(conn, rpInfo);
        roomPlayerInfos.Insert(index, rpInfo);
        NetworkServer.SendToClient(conn.connectionId, NetMessageType.Ready, playerReadyMessage);
        SendPlayersInfoToAll();
    }

    public void OnServerAddPlayer(NetworkMessage netMsg)
    {
        NetworkServer.DestroyPlayersForConnection(netMsg.conn);
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(netMsg.conn, player, 0);
    }

    public void OnServerGameStart(NetworkMessage netMsg)
    {
        GameStartMessage gameStartMessage = new GameStartMessage();
        NetworkServer.SendToAll(NetMessageType.Start, gameStartMessage);

        preRoomInfo = currentRoomInfo;
        currentRoomInfo.RoomState = 1;
        updateRoomRequest.SendRequest(currentRoomInfo.RoomIndex, currentRoomInfo.RoomName, currentRoomInfo.RoomOwner, currentRoomInfo.RoomPwd, currentRoomInfo.RoomIp, currentRoomInfo.RoomPort, currentRoomInfo.RoomScene,
             currentRoomInfo.RoomState, currentRoomInfo.RoomLevel, currentRoomInfo.RoomCurCount, currentRoomInfo.RoomMaxCount);
    }

    /*
     ********************************************************
     *                                                      *
     *                                                      *
     *                        CLIENT                        *
     *                                                      *
     *                                                      *
     *                                                      *
     ********************************************************
     */
    public void ConnectServer(string ip, int port)
    {
        isServer = false;
        if (myClient == null)
        {
            myClient = new NetworkClient();
            myClient.RegisterHandler(MsgType.Connect, OnClientConnect);
            myClient.RegisterHandler(NetMessageType.PlayersInfo, OnPlayersInfo);
            myClient.RegisterHandler(NetMessageType.RoomInfo, OnCurrentRoomInfo);
            myClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
            myClient.RegisterHandler(NetMessageType.Ready, OnClientPlayerReady);
            myClient.RegisterHandler(NetMessageType.Start, OnClientGameStart);
            myClient.Connect(ip, port);
        }
    }

    public void DisconnectServer()
    {
        if (isServer)
        {
            NetworkServer.Shutdown();
            NetworkServer.DisconnectAll();
            roomPlayerInfoDict = new Dictionary<NetworkConnection, RoomPlayerInfo>();
            roomPlayerInfos = new List<RoomPlayerInfo>();
            if (currentRoomInfo.RoomCurCount == 1)
            {
                removeRoomRequest.SendRequest(currentRoomInfo.RoomIndex);
            }
        }
        else
        {
            myClient.Disconnect();
        }
        myClient = null;
        preRoomInfo = null;
        currentRoomInfo = null;
        isServer = false;
        playerInfo = null;
        roomPlayerInfo = null;
        roomPlayerInfos.Clear();
        roomPlayerInfoDict.Clear();
        ServerChangeScene("Lobby");
    }

    public void OnClientConnect(NetworkMessage netMsg)
    {
        ClientScene.RegisterPrefab(playerPrefab);
        if (!isServer)
        {
            facade.ClearPanel();
            ServerChangeScene("Map1");
            SendPlayerInfo();
        }
    }

    public void OnClientDisconnect(NetworkMessage netMsg)
    {
        roomPlayerInfos.RemoveAt(0);
        RoomPlayerInfo [] infos = new RoomPlayerInfo[roomPlayerInfos.Count];
        int count = roomPlayerInfos.Count;
        infos = roomPlayerInfos.ToArray();
        roomPlayerInfos.Clear();
        for(int i = 0; i < count; ++i)
        {
            RoomPlayerInfo info = new RoomPlayerInfo(infos[i]);
            if ((i == 0) && (info.name == roomPlayerInfo.name) && (info.ip == roomPlayerInfo.ip) && (info.port == roomPlayerInfo.port))
            {
                preRoomInfo = currentRoomInfo;
                isServer = true;
                currentRoomInfo.RoomCurCount = 1;
                NetworkServer.BecomeHost(myClient, info.port, null, myClient.connection.connectionId, null);
                //StartServer(info.ip, info.port);
                currentRoomInfo.RoomIp = roomPlayerInfo.ip;
                currentRoomInfo.RoomPort = roomPlayerInfo.port;
                currentRoomInfo.RoomOwner = roomPlayerInfo.name;
                updateRoomRequest.SendRequest(currentRoomInfo.RoomIndex, currentRoomInfo.RoomName, currentRoomInfo.RoomOwner, currentRoomInfo.RoomPwd, currentRoomInfo.RoomIp, currentRoomInfo.RoomPort, currentRoomInfo.RoomScene,
             currentRoomInfo.RoomState, currentRoomInfo.RoomLevel, currentRoomInfo.RoomCurCount, currentRoomInfo.RoomMaxCount);
                
                //JoinGame();
                return;
            }
            else if(i != 0)
            {
                isServer = false;
                myClient.ReconnectToNewHost(infos[0].ip, infos[0].port);
                //JoinGame();
            }
        }
        roomPlayerInfos.Clear();
    }

    //Send to server
    public void SendPlayerInfo()
    {
        playerInfo = facade.GetPlayerInfo();
        string ip = Network.player.ipAddress;
        int port = 20010;
        PlayerInfoMessage playerInfoMsg = new PlayerInfoMessage();
        roomPlayerInfo = new RoomPlayerInfo(playerInfo.PlayerName, ip, port, 0, playerInfo.PlayerLevel, false, false);
        playerInfoMsg.roomPlayerInfo = roomPlayerInfo;
        myClient.Send(NetMessageType.PlayerInfo, playerInfoMsg);
    }

    //handle message form server
    public void OnPlayersInfo(NetworkMessage netMsg)
    {
        PlayersInfoMessage playersInfoMessage = new PlayersInfoMessage();
        playersInfoMessage.Deserialize(netMsg.reader);
        roomPlayerInfos = playersInfoMessage.roomPlayerInfos;
        SetRoomPlayerInfo();
        ShowPlaysInfo();
    }

    public void OnCurrentRoomInfo(NetworkMessage netMsg)
    {
        RoomInfoMessage roomInfoMessage = new RoomInfoMessage();
        roomInfoMessage.Deserialize(netMsg.reader);
        currentRoomInfo = roomInfoMessage.roomInfo;
    }

    public void PlayerReady(bool readyState)
    {
        if ( ClientScene.ready == false)
        {
            ClientScene.Ready(myClient.connection);
        }
        PlayerReadyMessage playerReadyMessage = new PlayerReadyMessage();
        playerReadyMessage.readyState = readyState;
        myClient.Send(NetMessageType.Ready, playerReadyMessage);
    }

    public void GameStart()
    {
        foreach(RoomPlayerInfo rpInfo in roomPlayerInfos)
        {
            if (rpInfo.readyState != true)
            {
                return;
            }
        }
        GameStartMessage gameStartMessage = new GameStartMessage();
        myClient.Send(NetMessageType.Start, gameStartMessage);
    }

    public void JoinGame()
    {
        ClientScene.Ready(myClient.connection);
        
        PlayerReadyMessage playerReadyMessage = new PlayerReadyMessage();
        playerReadyMessage.readyState = true;
        myClient.Send(NetMessageType.Ready, playerReadyMessage);

        facade.PopPanel();

        AddPlayerMessage addPlayerMessage = new AddPlayerMessage();
        myClient.Send(NetMessageType.AddPlayer, addPlayerMessage);
    }

    public void GameOver()
    {
        DisconnectServer();
    }

    public void OnClientPlayerReady(NetworkMessage netMsg)
    {
        PlayerReadyMessage playerReadyMessage = new PlayerReadyMessage();
        playerReadyMessage.Deserialize(netMsg.reader);
        roomPlayerInfo.readyState = playerReadyMessage.readyState;
        SetRoomPlayerInfo();
    }

    public void OnClientGameStart(NetworkMessage netMsg)
    {
        facade.StartGameManager();
        facade.PopPanel();
        AddPlayerMessage addPlayerMessage = new AddPlayerMessage();
        myClient.Send(NetMessageType.AddPlayer, addPlayerMessage);
    }
    /*
     ********************************************************
     *                                                      *
     *                                                      *
     *                        PRIVATE                       *
     *                                                      *
     *                                                      *
     *                                                      *
     ********************************************************
     */

    private void Update()
    {
        if (flag)
        {
            flag = false;
        }
    }

    /*
     ********************************************************
     *                                                      *
     *                                                      *
     *                        PUBLIC                        *
     *                                                      *
     *                                                      *
     *                                                      *
     ********************************************************
     */
    public List<RoomPlayerInfo> GetRoomPlayersInfo()
    {
        return roomPlayerInfos;
    }

    public void ShowPlaysInfo()
    {
        facade.ShowPlayersInfo();
    }

    public void SetCurrentRoomInfo(RoomInfo roomInfo)
    {
        this.currentRoomInfo = roomInfo;
    }

    public RoomInfo GetCurrentRoomInfo()
    {
        return currentRoomInfo;
    }

    public void SetRoomPlayerInfo()
    {
        facade.SetRoomPlayerInfo(roomPlayerInfo);
    }

    public void OnRemoveRoomResponse(ReturnCode retCode)
    {
        if (retCode == ReturnCode.Success)
        {
            SendCurrentRoomInfo();
        }
        else
        {
            currentRoomInfo = preRoomInfo;
        }
    }

    public void OnUpdateRoomResponse(ReturnCode retCode)
    {
        if (retCode == ReturnCode.Success)
        {
            SendCurrentRoomInfo();
        }
        else
        {
            currentRoomInfo = preRoomInfo;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetMigrationManager : NetworkMigrationManager {

    private GameFacade facade;
    public NetMigrationManager(GameFacade facade)
    {
        this.facade = facade;
    }

    public void SetFacade(GameFacade facade)
    {
        this.facade = facade;
    }

    public void OnInit()
    {
        
    }

    public void OnDestroy()
    {
        
    }

    protected override void OnServerHostShutdown()
    {
        Debug.Log("OnServerHostShutdown");
        base.OnServerHostShutdown();
    }

    protected override void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
    {
        Debug.Log("OnServerReconnectPlayer: conn = " + newConnection);
        base.OnServerReconnectPlayer(newConnection, oldPlayer, oldConnectionId, playerControllerId);
    }

    protected override void OnClientDisconnectedFromHost(NetworkConnection conn, out SceneChangeOption sceneChange)
    {
        Debug.Log("OnClientDisconnectedFromHost: conn = " + conn);
        base.OnClientDisconnectedFromHost(conn, out sceneChange);
    }
}

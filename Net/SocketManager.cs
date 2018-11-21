using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Assets.Scripts.Net;

public class SocketManager : BaseManager
{
    private const string SERVER_IP = "118.24.55.184";
    private const string LOCAL_IP = "127.0.0.1";
    private const int PORT = 20000;
    private Socket clientSocket;
    private Message message = new Message();

    public SocketManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(SERVER_IP, PORT);
            Start();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法连接到服务器端，请检查您的网络！！" + e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if ((clientSocket == null) || (!clientSocket.Connected))
            {
                return;
            }
            int count = clientSocket.EndReceive(ar);
            message.ParseMessage(count, OnProcessDataCallBack);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnProcessDataCallBack(ActionCode actionCode, string data)
    {
        facade.HandleResponse(actionCode, data);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PakcData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法关闭跟服务器的连接！！" + e);
        }
    }
}

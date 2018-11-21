using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerInfo
{
    public string name;
    public string ip;
    public int port;
    public int character;
    public int level;
    public bool readyState;
    public bool isRoomOwner;

    public RoomPlayerInfo(string name, string ip, int port, int character, int level, bool readyState, bool isRoomOwner)
    {
        this.name = name;
        this.ip = ip;
        this.port = port;
        this.character = character;
        this.level = level;
        this.readyState = readyState;
        this.isRoomOwner = isRoomOwner;
    }

    public RoomPlayerInfo(RoomPlayerInfo roomPlayerInfo)
    {
        this.name = roomPlayerInfo.name;
        this.ip = roomPlayerInfo.ip;
        this.port = roomPlayerInfo.port;
        this.character = roomPlayerInfo.character;
        this.level = roomPlayerInfo.level;
        this.readyState = roomPlayerInfo.readyState;
        this.isRoomOwner = roomPlayerInfo.isRoomOwner;
    }
}
using UnityEngine.Networking;
using Assets.Scripts.Model;

public class RoomInfoMessage : MessageBase
{
    public RoomInfo roomInfo { get; set; }

    public override void Deserialize(NetworkReader reader)
    {
        int index = reader.ReadInt32();
        string name = reader.ReadString();
        string owner = reader.ReadString();
        string pwd = reader.ReadString();
        string ip = reader.ReadString();
        int port = reader.ReadInt32();
        int scene = reader.ReadInt32();
        int state = reader.ReadInt32();
        int level = reader.ReadInt32();
        int curCount = reader.ReadInt32();
        int maxCount = reader.ReadInt32();
        roomInfo = new RoomInfo(index, name, owner, pwd, ip, port, scene, state, level, curCount, maxCount);
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(roomInfo.RoomIndex);
        writer.Write(roomInfo.RoomName);
        writer.Write(roomInfo.RoomOwner);
        writer.Write(roomInfo.RoomPwd);
        writer.Write(roomInfo.RoomIp);
        writer.Write(roomInfo.RoomPort);
        writer.Write(roomInfo.RoomScene);
        writer.Write(roomInfo.RoomState);
        writer.Write(roomInfo.RoomLevel);
        writer.Write(roomInfo.RoomCurCount);
        writer.Write(roomInfo.RoomMaxCount);
    }
}
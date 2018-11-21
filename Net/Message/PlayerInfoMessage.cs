using UnityEngine.Networking;

public class PlayerInfoMessage : MessageBase
{
    public RoomPlayerInfo roomPlayerInfo;
    public override void Deserialize(NetworkReader reader)
    {
        string name = reader.ReadString();
        string ip = reader.ReadString();
        int port = reader.ReadInt32();
        int character = reader.ReadInt32();
        int level = reader.ReadInt32();
        bool readyState = reader.ReadBoolean();
        bool isRoomOwner = reader.ReadBoolean();
        roomPlayerInfo = new RoomPlayerInfo(name, ip, port, character, level, readyState, isRoomOwner);
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(roomPlayerInfo.name);
        writer.Write(roomPlayerInfo.ip);
        writer.Write(roomPlayerInfo.port);
        writer.Write(roomPlayerInfo.character);
        writer.Write(roomPlayerInfo.level);
        writer.Write(roomPlayerInfo.readyState);
        writer.Write(roomPlayerInfo.isRoomOwner);
    }
}
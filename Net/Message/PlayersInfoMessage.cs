using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayersInfoMessage : MessageBase
{
    public int count;
    public List<RoomPlayerInfo> roomPlayerInfos = new List<RoomPlayerInfo>();

    public override void Deserialize(NetworkReader reader)
    {
        count = reader.ReadInt32();
        for (int i = count; i > 0; --i)
        {
            string name = reader.ReadString();
            string ip = reader.ReadString();
            int port = reader.ReadInt32();
            int character = reader.ReadInt32();
            int level = reader.ReadInt32();
            bool readyState = reader.ReadBoolean();
            bool isRoomOwner = reader.ReadBoolean();
            RoomPlayerInfo info = new RoomPlayerInfo(name, ip, port, character, level, readyState, isRoomOwner);
            roomPlayerInfos.Add(info);
        }
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(count);
        for (int i = 0; i < count; ++i)
        {
            writer.Write(roomPlayerInfos[i].name);
            writer.Write(roomPlayerInfos[i].ip);
            writer.Write(roomPlayerInfos[i].port);
            writer.Write(roomPlayerInfos[i].character);
            writer.Write(roomPlayerInfos[i].level);
            writer.Write(roomPlayerInfos[i].readyState);
            writer.Write(roomPlayerInfos[i].isRoomOwner);
        }
    }
}
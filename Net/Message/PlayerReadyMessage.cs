using UnityEngine.Networking;

public class PlayerReadyMessage : MessageBase
{
    public bool readyState;
    public override void Deserialize(NetworkReader reader)
    {
        readyState = reader.ReadBoolean();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(readyState);
    }
}
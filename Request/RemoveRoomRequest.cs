using Assets.Scripts.Manager;

public class RemoveRoomRequest : BaseRequest
{
    private NetManager netManager;
    private SNetManager sNetManager;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.RemoveRoom;
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest(int index)
    {
        string data = index.ToString();
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode retCode = (ReturnCode)int.Parse(data);
        //netManager.OnRemoveRoomResponse(retCode);
        sNetManager.OnRemoveRoomResponse(retCode);
    }

    public void SetNetManager(NetManager netManager)
    {
        this.netManager = netManager;
    }

    public void SetSNetManager(SNetManager sNetManager)
    {
        this.sNetManager = sNetManager;
    }
}

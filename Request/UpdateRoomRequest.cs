using Assets.Scripts.Manager;

public class UpdateRoomRequest : BaseRequest {

    private NetManager netManager;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest(int index, string name, string owner, string pwd, string ip, int port, int scene, int state, int level, int curCount, int maxCount)
    {
        string data = index + " " + name + " " + owner + " " + pwd + " " + ip + " " + port +
            " " + scene + " " + state + " " + level + " " + curCount + " " + maxCount;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode retCode = (ReturnCode)int.Parse(data);
        netManager.OnUpdateRoomResponse(retCode);
    }

    public void SetNetManager(NetManager netManager)
    {
        this.netManager = netManager;
    }
}

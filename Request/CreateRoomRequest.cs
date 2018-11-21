using UnityEngine;
using Assets.Scripts.Model;

public class CreateRoomRequest : BaseRequest
{
    private CreateRoomPanel createRoomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        createRoomPanel = GetComponent<CreateRoomPanel>();
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest(string name, string owner, string pwd, string ip, int port, int scene, int state, int level, int curCount, int maxCount)
    {
        string data = name + " " + owner + " " + pwd + " " + ip + " " + port + 
            " " + scene + " " + state + " " + level + " " + curCount + " " + maxCount;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        ReturnCode retCode = (ReturnCode)int.Parse(strs[0]);
        createRoomPanel.OnResponse(retCode);
        if (retCode == ReturnCode.Success)
        {
            string [] roomParams = strs[1].Split(' ');
            RoomInfo roomInfo = new RoomInfo(int.Parse(roomParams[0]), roomParams[1], roomParams[2], roomParams[3], roomParams[4], int.Parse(roomParams[5]), int.Parse(roomParams[6]),
            int.Parse(roomParams[7]), int.Parse(roomParams[8]), int.Parse(roomParams[9]), int.Parse(roomParams[10]));
            facade.SetCurrentRoomInfo(roomInfo);
        }
    }
}

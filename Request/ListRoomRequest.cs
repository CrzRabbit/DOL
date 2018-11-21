using System.Collections.Generic;
using Assets.Scripts.Model;

public class ListRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;
    List<RoomInfo> roomList = new List<RoomInfo>();

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest()
    {
        string data = "";
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        ReturnCode retCode = (ReturnCode)int.Parse(strs[0]);
        roomPanel.OnSearchResponse(retCode);
        if (retCode == ReturnCode.Fail)
        {
            roomList.Clear();
            facade.SetRoomsInfo(roomList);
            return;
        }
        if(strs[1] == " ")
        {
            return;
        }
        if (retCode == ReturnCode.Success)
        {
            roomList.Clear();
            for (int i = 1; i < strs.Length; ++i)
            {
                roomList.Add(new RoomInfo(strs[i]));
            }
        }
        else if (retCode == ReturnCode.Cont)
        {
            for (int i = 1; i < strs.Length; ++i)
            {
                roomList.Add(new RoomInfo(strs[i]));
            }
        }
        else if (retCode == ReturnCode.Update)
        {
            RoomInfo roomInfo = new RoomInfo(strs[1]);
            int index = 0;
            foreach (RoomInfo info in roomList)
            {
                if (roomInfo.RoomIndex == info.RoomIndex)
                {
                    roomList.RemoveAt(index);
                    roomList.Insert(index, roomInfo);
                    break;
                }
                ++index;
            }
        }
        else if (retCode == ReturnCode.Remove)
        {
            int index = int.Parse(strs[1]);
            foreach (RoomInfo roomInfo in roomList)
            {
                if (roomInfo.RoomIndex == index)
                {
                    roomList.Remove(roomInfo);
                    break;
                }
            }
        }
        facade.SetRoomsInfo(roomList);
    }
}

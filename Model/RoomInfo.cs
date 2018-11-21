namespace Assets.Scripts.Model
{
    public class RoomInfo
    {
        public int RoomIndex { get; set; }
        public string RoomName { get; set; }
        public string RoomOwner { get; set; }
        public string RoomPwd { get; set; }
        public string RoomIp { get; set; }
        public int RoomPort { get; set; }
        public int RoomScene { get; set; }
        public int RoomState { get; set; }
        public int RoomLevel { get; set; }
        public int RoomCurCount { get; set; }
        public int RoomMaxCount { get; private set; }

        public RoomInfo(string data)
        {
            string[] strs = data.Split(' ');
            this.RoomIndex = int.Parse(strs[0]);
            this.RoomName = strs[1];
            this.RoomOwner = strs[2];
            this.RoomPwd = strs[3];
            this.RoomIp = strs[4];
            this.RoomPort = int.Parse(strs[5]);
            this.RoomScene = int.Parse(strs[6]);
            this.RoomState = int.Parse(strs[7]);
            this.RoomLevel = int.Parse(strs[8]);
            this.RoomCurCount = int.Parse(strs[9]);
            this.RoomMaxCount = int.Parse(strs[10]);
        }

        public RoomInfo(int roomIndex, string roomName, string roomOwner, string roomPwd, string roomIp, int roomPort, int roomScene, 
            int roomState, int roomLevel, int roomCurCount, int roomMaxCount)
        {
            this.RoomIndex = roomIndex;
            this.RoomName = roomName;
            this.RoomOwner = roomOwner;
            this.RoomPwd = roomPwd;
            this.RoomIp = roomIp;
            this.RoomPort = roomPort;
            this.RoomScene = roomScene;
            this.RoomState = roomState;
            this.RoomLevel = roomLevel;
            this.RoomCurCount = roomCurCount;
            this.RoomMaxCount = roomMaxCount;
        }
    }
}
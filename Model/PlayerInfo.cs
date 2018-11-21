using UnityEngine;
namespace Assets.Scripts.Model
{
    public class PlayerInfo
    {
        public int PlayerIndex { get; private set; }
        public string PlayerName { get; private set; }
        public int PlayerLevel { get; private set; }
        public int PlayerCurExp { get; private set; }

        public PlayerInfo(string playerInfo)
        {
            string[] strs = playerInfo.Split(' ');
            this.PlayerIndex = int.Parse(strs[0]);
            this.PlayerName = strs[1];
            this.PlayerLevel = int.Parse(strs[2]);
            this.PlayerCurExp = int.Parse(strs[3]);
        }

        public PlayerInfo(int playerIndex, string playerName, int playerLevel, int playerCurExp)
        {
            this.PlayerIndex = playerIndex;
            this.PlayerName = playerName;
            this.PlayerLevel = playerLevel;
            this.PlayerCurExp = playerCurExp;
        }

        public int GetLevelExp()
        {
            if(PlayerLevel <= 10)
                return this.PlayerLevel * this.PlayerLevel * 40 + 10;
            return this.PlayerLevel * this.PlayerLevel * 100 + 10;
        }
    }
}
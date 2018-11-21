using Assets.Scripts.Model;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Account;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest(string username, string password)
    {
        string data = username + ' ' + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        //Debug.Log(data);
        string[] strs = data.Split(' ');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {
            int index = int.Parse(strs[1]);
            string name = strs[2];
            int level = int.Parse(strs[3]);
            int cur_exp = int.Parse(strs[4]);
            PlayerInfo info = new PlayerInfo(index, name, level, cur_exp);
            facade.SetPlayerInfo(info);
        }
    }
}

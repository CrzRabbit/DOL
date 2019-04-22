public class LogoutRequest : BaseRequest {
    public override void Awake()
    {
        requestCode = RequestCode.Account;
        actionCode = ActionCode.Logout;
        base.Awake();
    }

    public void Update()
    {

    }

    public void SendRequest(int userIndex)
    {
        string data = userIndex.ToString();
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {

    }
}

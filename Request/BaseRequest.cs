using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade facade;

    public virtual void Awake()
    {
        facade = GameFacade.Instance;
        GameFacade.Instance.AddRequest(actionCode, this);
    }

    protected void SendRequest(string data)
    {
        facade.SendRequest(requestCode, actionCode, data);
    }

    public virtual void SendRequest() { }
    public virtual void OnResponse(string data) { }

    public virtual void OnDestroy()
    {
        if (facade != null)
        {
            facade.RemoveRequest(actionCode);
        }
    }
}

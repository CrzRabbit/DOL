using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestCode
{
    None,
    Account,
    Room,
}

public enum ActionCode
{
    None,
    Register,
    Updatepwd,
    Clear,
    Login,
    Logout,
    OfflineAll,

    CreateRoom,
    ListRoom,
    UpdateRoom,
    JoinRoom,
    RemoveRoom,
    ClearEmpty,
    RemoveAll
}

public enum ReturnCode
{
    Success,
    Fail,
    Cont,
    Update,
    Remove
}
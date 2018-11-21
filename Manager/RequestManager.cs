using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class RequestManager : BaseManager
    {
        private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

        public RequestManager(GameFacade facade) : base(facade) { }

        public void AddRequest(ActionCode actionCode, BaseRequest request)
        {
            if (!requestDict.ContainsKey(actionCode))
            {
                requestDict.Add(actionCode, request);
            }
        }

        public void RemoveRequest(ActionCode actionCode)
        {
            requestDict.Remove(actionCode);
        }

        public void HandleResponse(ActionCode actionCode, string data)
        {
            BaseRequest request = null;
            requestDict.TryGetValue(actionCode, out request);
            if (null == request)
            {
                Debug.LogWarning("无法得到ActionCode[" + actionCode + "]对应的Request类");
                return;
            }
            Debug.Log(Enum.GetName(typeof(ActionCode), actionCode) + " " + data);
            request.OnResponse(data);
        }
    }
}

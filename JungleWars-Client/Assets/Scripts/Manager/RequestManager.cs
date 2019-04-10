using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager
{ //這個是BaseRequest的基類
    public RequestManager(GameFacade facade) : base(facade) { }
    
    //創造requestDict字典，讓request可以加入、移除到字典內。要處理request時，到字典內查找
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();
    //字典內增加Request
    public void AddRequest(ActionCode actionCode,BaseRequest request)
    {
        requestDict.Add(actionCode, request);
    }
    //字典內移除Request
    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }
    //字典內處理並查找Request
    public void HandleReponse(ActionCode actionCode, string data)//要處理request時，到字典內查找
    {   //定義request。讓request對應到查找的字典
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if (request == null) //字典內沒找到就報錯
        {
            Debug.LogWarning("无法得到ActionCode[" + actionCode + "]对应的Request类");return;
        }
        request.OnResponse(data);
    }
}

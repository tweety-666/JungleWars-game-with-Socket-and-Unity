using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RegisterRequest : BaseRequest {

    //註冊面板會發出要求，叫RegisterRequest負責SendRequest
    //RegisterRequest收到回應，再叫註冊面板執行函式
    private RegisterPanel registerPanel; //定義對應的UI面板
    public override void Awake()
    {    
    //這三行得優先base.Awake();先執行
    //因為BaseRequest內的Awake()要求facade.AddRequest(actionCode, this);
    //所以要先給定義好的參數
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }
    //被UI面板呼叫函式後，RegisterRequest傳要求
    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }
    //收到回應後，叫UI面板執行對應函式
    //那誰會呼叫OnResponse函式?
    //後端負責阿
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        registerPanel.OnRegisterResponse(returnCode);
    }
}
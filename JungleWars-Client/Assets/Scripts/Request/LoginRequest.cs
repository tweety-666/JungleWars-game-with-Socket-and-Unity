using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LoginRequest : BaseRequest {
    //一樣先定義對應的UI面板
    private LoginPanel loginPanel;
    // Use this for initialization
    public override void Awake()
    {
        requestCode = RequestCode.User; //這三行的初始化，必須放在base.Awake()之前
        actionCode = ActionCode.Login; //把自身先添加到集合裡面，統一管理
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }
    //被UI介面呼叫執行，負責傳送要求
    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }
    //接收後端傳回來的Response
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');//把資料用","分隔開，再放到字串陣列內
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //處理好資料叫面板執行對應的回應函式
        //ReturnCode這個類後端要處理好，前端得跟後端討論回傳的資料格式
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {   
            //登入好後，遊戲內就有這個玩家，玩家有自己的UserData
            //要先處理UserData這個類
            string username = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData ud = new UserData(username, totalCount, winCount);
            facade.SetUserData(ud);//這個facade在父類就定義好了
        }
    }

}

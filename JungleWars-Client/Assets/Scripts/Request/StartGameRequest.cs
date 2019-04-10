using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class StartGameRequest : BaseRequest {
    private RoomPanel roomPanel;//聲明是哪個UI面板作用此Request
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();//定義是哪個UI面板作用此Request
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);//得到data轉成ReturnCode
        roomPanel.OnStartResponse(returnCode);//根據ReturnCode，UI面板要開始遊戲
        //根據ReturnCode，可以判斷是不是房主，房主才可以開始遊戲
    }

}

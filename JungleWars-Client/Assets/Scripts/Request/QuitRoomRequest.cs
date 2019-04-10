using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuitRoomRequest : BaseRequest {
    private RoomPanel roomPanel;//聲明是哪個UI面板作用此Request
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.QuitRoom;
        roomPanel = GetComponent<RoomPanel>();//定義是哪個UI面板作用此Request
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);//將後端傳來的data轉成ReturnCode
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.OnExitResponse();//成功的話，請UI面板執行離開的回應函式
        }
    }
}

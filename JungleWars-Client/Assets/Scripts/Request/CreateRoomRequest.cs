using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class CreateRoomRequest :BaseRequest {
    //聲明
    private RoomPanel roomPanel;//聲明是在哪個UI介面作用

    //定義
    public override void Awake()
    {
        requestCode = RequestCode.Room; //要比base.Awake()先執行
        actionCode = ActionCode.CreateRoom;//因為base.Awake()內的參數需要
        base.Awake();
    }

    public void SetPanel( BasePanel panel)
    {
        roomPanel = panel as RoomPanel;
    }

    //傳送Request
    public override void SendRequest()
    {
        base.SendRequest("r"); 
        //傳隨便一個字串，只是防止數據解析過程中出錯
        //也沒有真的要傳資料，只是做出請求
    }

    //做出Response
    public override void OnResponse(string data) //收到後端資料，做出回應
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //這邊有個RoleType類，RoleType類會另外說明
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        facade.SetCurrentRoleType(roleType);//facade單例執行SetCurrentRoleType函式

        if (returnCode == ReturnCode.Success)
        {   //創建房間成功後，在面板上設置個人戰績
            //房間內左邊欄位是我方的個人戰績
            roomPanel.SetLocalPlayerResSync();
        }
    }
}

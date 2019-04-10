using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class JoinRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;//聲明是在哪個UI介面作用

    public override void Awake()
    {
        requestCode = RequestCode.Room;//要比base.Awake()先執行
        actionCode = ActionCode.JoinRoom;//因為base.Awake()內的參數需要
        roomListPanel = GetComponent<RoomListPanel>();//定義UI介面
        base.Awake();
    }

    public void SendRequest(int id) //傳送加入房間的Request，參數是房間id
    {
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-');
        string[] strs2 = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        //聲明玩家
        UserData ud1 = null;//房間內玩家1(自己)
        UserData ud2 = null;//房間內玩家2(敵人)
        if (returnCode == ReturnCode.Success)
        {   //定義玩家
            //new出來
            string[] udStrArray = strs[1].Split('|');
            ud1 = new UserData(udStrArray[0]);
            ud2 = new UserData(udStrArray[1]);
            
            RoleType roleType = (RoleType)int.Parse(strs2[1]);
            facade.SetCurrentRoleType(roleType);
        }
        //Request回應，叫UI面板執行函式，把兩位玩家資訊呈現在面板上
        roomListPanel.OnJoinResponse(returnCode, ud1, ud2);
    }
}

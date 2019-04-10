using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdateRoomRequest : BaseRequest {
    private RoomPanel roomPanel; //聲明要作用的UI
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();//定義要作用UI
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        UserData ud1 = null;//聲明2個玩家資訊
        UserData ud2 = null;
        string[] udStrArray = data.Split('|');//抓到後端傳來的資訊，用直線隔開並加入字串陣列中
        ud1 = new UserData(udStrArray[0]);//抓取字串陣列中的資料，作為玩家1的資料
        if(udStrArray.Length>1)
            ud2 = new UserData(udStrArray[1]);//抓取字串陣列中的資料，作為玩家2的資料
        roomPanel.SetAllPlayerResSync(ud1, ud2);//面板上設置兩位玩家的資訊
    }
}

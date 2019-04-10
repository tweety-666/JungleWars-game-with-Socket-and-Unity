using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ListRoomRequest : BaseRequest {
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");//傳隨便一個字串，只是防止數據解析過程中出錯
        //也沒有真的要傳資料，只是做出請求
    }
    public override void OnResponse(string data)
    {
        List<UserData> udList = new List<UserData>(); //產生一個空的玩家資料清單，用來把其中資訊放到房間資訊上
        //房間會呈現玩家資訊，像是房主名稱、勝率等等
        if (data != "0")
        {
            string[] udArray = data.Split('|');//抓到的資料，放到陣列內
            foreach (string ud in udArray)//陣列內每項資料轉成字串陣列，並用逗號分隔，把資料加到空的玩家資料清單
            {
                string[] strs = ud.Split(',');
                udList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
            }
        }
        
        roomListPanel.LoadRoomItemSync(udList);//空的玩家資料清單被加入資料了，叫ui面板（房間列表）呈現出資料（房主資訊）
    }
}

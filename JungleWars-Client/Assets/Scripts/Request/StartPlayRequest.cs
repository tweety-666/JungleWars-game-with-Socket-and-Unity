using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartPlayRequest : BaseRequest {
//開始遊戲請求
    private bool isStartPlaying = false;

    public override void Awake()
    {   //注意到這邊沒有requestCode
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }
    //這邊沒有傳送請求的函式
    //因為倒數3秒後，後端發現倒數完，就會開始傳送訊息給前端說可以開始遊戲
    //倒數3秒完，前端收到回傳訊息，執行OnResponse()
    private void Update()
    {
        if (isStartPlaying)
        {
            facade.StartPlaying();
            //布林值轉為真，就請facade開始運行遊戲
            //會執行以下函式
            // public void StartPlaying()
            // {
            //     playerMng.AddControlScript(); //playerMng加入動畫控制器函式，讓玩家動起來
            //     playerMng.CreateSyncRequest();
            // }
            isStartPlaying = false;
        }
    }

    public override void OnResponse(string data)//接收後端訊息，將布林值轉為真
    {
        isStartPlaying = true;
    }
}

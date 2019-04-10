using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ShowTimerRequest : BaseRequest {

    private GamePanel gamePanel;//聲明作用的UI
    public override void Awake()
    {   //這邊並不會傳Request給後端，而是經由後端倒數3秒後，前端接到回應並Response，請遊戲頁面同步倒數3秒後，開始遊戲。
        //所以沒有看到requestCode跟傳送request的函式
        //requestCode = RequestCode.Game;
        actionCode = ActionCode.ShowTimer;
        gamePanel = GetComponent<GamePanel>();//定義作用的UI
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int time = int.Parse(data);
        gamePanel.ShowTimeSync(time);
    }
}

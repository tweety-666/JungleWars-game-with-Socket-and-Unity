using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuitBattleRequest : BaseRequest
{
    private bool isQuitBattle = false;//預設布林值是false
    private GamePanel gamePanel;//聲明哪個UI作用此Request
    public override void Awake()
    {
        requestCode = RequestCode.Game;//定義好RequestCode跟ActionCode
        actionCode = ActionCode.QuitBattle;
        gamePanel = GetComponent<GamePanel>();//定義哪個UI作用此Request
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    private void Update()//一直去偵測後端傳回應了沒，回應的話就叫UI面板執行遊戲結束的回應函式
    {
        if (isQuitBattle)
        {
            gamePanel.OnExitResponse();
            isQuitBattle = false;
        }
    }
    public override void OnResponse(string data)//傳送Request後，後端回應data，前端做出回應
    {
        isQuitBattle = true;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class GameOverRequest:BaseRequest
{
    private GamePanel gamePanel;//聲明作用於哪個面板
    private bool isGameOver = false;//藉由遊戲結束的布林值來判斷是否結束遊戲了，預設為false
    private ReturnCode returnCode;
    public override void Awake()
    {
        requestCode = RequestCode.Game;//定義好RequestCode跟ActionCode
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();//定義作用於哪個面板
        base.Awake();
    }
    //這邊沒有傳送request的函式，那何時觸發遊戲結束函式?又是誰負責呢?
    //當遊戲結束時，會出現失敗跟成功的字(按鈕)
    //按下去會顯示遊戲結果，並結束遊戲
    //在UI面板按下失敗跟成功的字(按鈕)，會叫facade負責執行GameOver的函式
    //也就是說遊戲運行跟結束，都是由facade單例來處理

    //那麼遊戲結束時，facade會做什麼呢? 如下：
    //  public void GameOver()
    // {
    //     cameraMng.WalkthroughScene();　//相機會變成漫遊模式(後續會提到這是啥)
    //     playerMng.GameOver(); //playerMng才是真正處理遊戲結束的角色
    // }
    //為何要由playerMng來處理遊戲結束呢?
    //playerMng負責很多東西(後續會提)，之所以給playerMng負責，因為遊戲結束同時，角色(我方跟敵方)、相關請求都要消除
    private void Update()
    {
        if (isGameOver)
        {
            gamePanel.OnGameOverResponse(returnCode);//遊戲結束的布林值轉為true，就請UI面板執行遊戲結束函式
            isGameOver = false;
        }
    }
    public override void OnResponse(string data)
    {
        returnCode = (ReturnCode)int.Parse(data) ;//收到後端回傳的資訊，遊戲結束的布林值轉為true
        isGameOver = true;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdateResultRequest:BaseRequest
{
    private RoomListPanel roomListPanel;//聲明作用於哪個UI
    private bool isUpdateResult = false;
    private int totalCount;//聲明遊戲物件
    private int winCount;
    public override void Awake()
    {
        actionCode = ActionCode.UpdateResult;
        roomListPanel = GetComponent<RoomListPanel>();//定義作用於哪個UI
        base.Awake();
    }
    //沒有傳送request的函式
    //誰負責傳更新戰績的request呢?
    //是後端主動傳送戰績，只要戰績有變動，就抓給前端顯示
    //上方Awake()函式內，也沒有RequestCode
    //因為前端沒有發請求

    private void Update()
    {
        if (isUpdateResult)
        {
            roomListPanel.OnUpdateResultResponse(totalCount,winCount);//isUpdateResult為真，就叫UI面板做出回應
            isUpdateResult = false;
        }
    }
    public override void OnResponse(string data)//後端回傳data，抓到資訊後轉成int，放入遊戲物件呈現戰績
    {
        string[] strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        isUpdateResult = true;
    }
}

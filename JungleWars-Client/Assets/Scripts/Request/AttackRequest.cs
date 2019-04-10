using System;
using System.Collections.Generic;
using Common;
using UnityEngine;


public class AttackRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Attack ;
        base.Awake();
    }
    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());//傳送扣血量的資訊給後端
    }
    //注意到這邊沒有回應的函式
    //攻擊到一定程度(有一方先掛掉)，後端偵測到有哪方血量歸零，才有後續的反應(遊戲結算並結束等等)
}


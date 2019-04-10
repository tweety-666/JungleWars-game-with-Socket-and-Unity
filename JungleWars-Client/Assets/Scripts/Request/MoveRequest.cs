using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class MoveRequest : BaseRequest {
//本地端角色
    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private int syncRate = 30; //每秒同步率，一秒同步30次

//遠端角色
    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;

    private bool isSyncRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rotation;
    private float forward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }
    private void Start()//遊戲一開始，對遊戲角色掛載此腳本後，就狂傳本地端位置資訊給後端
    {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f / syncRate);//重複調用SyncLocalPlayer函式
        //此函式是傳本地端角色的位置跟旋轉角度給後端
    }
    private void FixedUpdate()
    {
        if (isSyncRemotePlayer) //isSyncRemotePlayer會在後端傳回資訊時，轉為true
        {
            SyncRemotePlayer();//也同步遠端角色
            isSyncRemotePlayer = false;
        }
    }
    public MoveRequest SetLocalPlayer(Transform localPlayerTransform, PlayerMove localPlayerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.localPlayerMove = localPlayerMove;
        return this;
    }
    public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }
    private void SyncLocalPlayer()//此函式是傳本地端角色的位置跟旋轉角度給後端
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
            localPlayerMove.forward);
    }
    private void SyncRemotePlayer()
    {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
        remotePlayerAnim.SetFloat("Forward", forward);
    }
    //傳位置跟旋轉角度給後端
    private void SendRequest(float x,float y,float z,float rotationX,float rotationY,float rotationZ,float forward) 
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forward);
        base.SendRequest(data);
    }
    public override void OnResponse(string data) //從後端接收後回應
    {//27.75,0,1.41-0,0,0-0
        //print(data);
        string[] strs = data.Split('|');
        pos = UnityTools.ParseVector3(strs[0]);//要接收位置，利用UnityTools轉成Vector3
        rotation = UnityTools.ParseVector3(strs[1]);//要接收旋轉角度，轉成Vector3
        forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true; //開始同步
    }
    
}

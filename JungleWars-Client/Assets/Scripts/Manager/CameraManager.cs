using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager {

    private GameObject cameraGo; 
    private Animator cameraAnim;//相機動畫
    private FollowTarget followTarget;//相機要跟隨誰
    private Vector3 originalPosition; //原本相機位置
    private Vector3 originalRotation;//原本相機的旋轉角度

    public CameraManager(GameFacade facade) : base(facade) { }

    public override void OnInit() //初始化的時候去抓遊戲物件
    {
        cameraGo = Camera.main.gameObject;//相機
        cameraAnim = cameraGo.GetComponent<Animator>(); //抓到動化元件
        followTarget = cameraGo.GetComponent<FollowTarget>();//另一個腳本，要讓相機去跟隨目標
    }

    //public override void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        FollowTarget(null);
    //    }
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        WalkthroughScene(); 
    //    }
    //}
    //下方定義兩種方法讓相機切換
    //一種是相機跟隨目標，一種是漫遊狀態
    public void FollowRole() //跟隨目標
    {
        followTarget.target = facade.GetCurrentRoleGameObject().transform;//透過followTarget腳本，去找目標，目標就是玩家腳色，要透過facade抓取到
        cameraAnim.enabled = false;//沒有漫遊狀態，相機不需要動畫
        //定義初始位置跟旋轉角度
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;

        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(delegate()
        {
            followTarget.enabled = true;//沒有漫遊狀態，相機跟隨目標
        });
    }
    public void WalkthroughScene()//漫遊狀態
    {
        followTarget.enabled = false; //場景漫遊，不需要目標
        cameraGo.transform.DOMove(originalPosition, 1f); //相機回歸到初始位置
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete( delegate()//相機回歸到初始旋轉角度，結束後
        {
            cameraAnim.enabled = true;//漫遊狀態，相機有動畫
        });
    }
}

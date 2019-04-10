using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class GamePanel : BasePanel {
    //聲明遊戲物件
    private Text timer;
    private int time = -1;
    private Button successBtn;
    private Button failBtn;
    private Button exitBtn;

    private QuitBattleRequest quitBattleRequest;
    private void Start()
    {   //定義遊戲物件，並監聽
        //某些遊戲物件預設為無顯示
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        successBtn = transform.Find("SuccessButton").GetComponent<Button>();
        successBtn.onClick.AddListener(OnResultClick);
        successBtn.gameObject.SetActive(false);
        failBtn = transform.Find("FailButton").GetComponent<Button>();
        failBtn.onClick.AddListener(OnResultClick);
        failBtn.gameObject.SetActive(false);
        exitBtn = transform.Find("ExitButton").GetComponent<Button>();
        exitBtn.onClick.AddListener(OnExitClick);
        exitBtn.gameObject.SetActive(false);
        //定義此面板涉及的要求
        quitBattleRequest = GetComponent<QuitBattleRequest>();

    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);//進入面板；顯示此面板
    }
    public override void OnExit()//離開面板；不顯示所有遊戲物件
    {
        successBtn.gameObject.SetActive(false);
        failBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }
    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        facade.GameOver(); //GameFacade執行遊戲結束的函式
    }
    private void OnExitClick() //對UI案離開，UI請對應的Request腳本，發出Request
    {
        quitBattleRequest.SendRequest();
    }
    public void OnExitResponse() //Request腳本請UI面板對離開做出回應
    {
        OnResultClick(); //離開後顯示遊戲結果
    }
    public void ShowTimeSync(int time)
    {
        this.time = time;
    }
    //倒數三秒，開始遊戲，搭配音效
    public void ShowTime(int time)
    {
        if (time == 3)
        {
            exitBtn.gameObject.SetActive(true);
        }
        timer.gameObject.SetActive(true);//倒數計時計顯示
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));//動畫結束完後，面板影藏
        facade.PlayNormalSound(AudioManager.Sound_Alert);//請GameFacade叫AudioManager去播音檔
    }
    public void OnGameOverResponse(ReturnCode returnCode) //面板針對遊戲失敗的回應
    {
        Button tempBtn = null; //空按鈕，成功就顯示成功按鈕，失敗則顯示失敗按鈕
        switch (returnCode)
        {
            case ReturnCode.Success:
                tempBtn = successBtn;
                break;
            case ReturnCode.Fail:
                tempBtn = failBtn;
                break;
        }
        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StartPanel : BasePanel {
    //起始界面，只有登入按鈕
    //聲明遊戲物件
    private Button loginButton;//登入按鈕
    private Animator btnAnimator;//按鈕動畫
    public override void OnEnter()
    {
        base.OnEnter();
        //進入畫面，定義遊戲物件
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        btnAnimator = loginButton.GetComponent<Animator>();
        loginButton.onClick.AddListener(OnLoginClick);//監聽按鈕，執行註冊函式
    }

    private void OnLoginClick()
    {
        PlayClickSound();//父類有定義此函式
        uiMng.PushPanel(UIPanelType.Login);
    }
    public override void OnPause()//暫停
    {
        base.OnPause();
        btnAnimator.enabled = false;//動畫不被啟用
        loginButton.transform.DOScale(0, 0.3f).OnComplete(() => loginButton.gameObject.SetActive(false) );//動畫執行完，登入按鈕不顯示
    }
    public override void OnResume()
    {
        base.OnResume();
        loginButton.gameObject.SetActive(true);//按鈕顯示
        loginButton.transform.DOScale(1, 0.3f).OnComplete(() => btnAnimator.enabled = true);//動畫啟用
    }
}

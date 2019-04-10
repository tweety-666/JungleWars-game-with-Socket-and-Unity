using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
public class RegisterPanel : BasePanel {
    //把每個遊戲物件先聲明好
    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private RegisterRequest registerRequest; //定義這個UI面板會發出怎樣的要求，它負責SendRequest

    private void Start()
    { //定義已經聲明好的遊戲物件，之後可以指定他們做什麼事情
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();

    //監聽按鈕，點擊後執行函式
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick); //註冊
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick); //關面板
    }

    //開始覆寫BasePanel內的函式
    //進入面板開始跑動畫
    public override void OnEnter()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
    //點註冊鈕後執行註冊函式
    private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        //判斷InputField內的內容
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空";
        }
        if ( passwordIF.text!=rePasswordIF.text )
        {
            msg += "\n密码不一致";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);return; //訊息由uiMng負責顯示
        }
        
        //进行注册 发送到服务器端
        registerRequest.SendRequest(usernameIF.text, passwordIF.text);
    }
    //這個UI介面執行SendRequest後，會叫RegisterRequest傳要求
    //RegisterRequest接收後端傳回的Response，再叫UI介面執行函式
    //這邊是定義函式，沒有要在此執行，會由RegisterRequest來呼叫執行
    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
            //uiMng會呼叫負責處理訊息的msgPanel去處理訊息
            //因為uiMng就是只負責畫面，不處理訊息
        }
        else
        {
            uiMng.ShowMessageSync("用户名重复");
        }
    }
    //關掉面板，uiMng會移除面板資訊
    private void OnCloseClick()
    {
        PlayClickSound();
        transform.DOScale(0, 0.4f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }
    //離開面板，讓它從畫面上隱藏
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}
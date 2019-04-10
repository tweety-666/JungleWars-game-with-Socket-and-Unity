using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RoomPanel : BasePanel {
//設置各種聲明
//我方資訊
    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;
//敵方資訊
    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;
//面板跟按鈕
    private Transform bluePanel;//我方面板(顯示在左邊)
    private Transform redPanel;//敵方面板(顯示在右邊)
    private Transform startButton;
    private Transform exitButton;
//玩家資訊
    private UserData ud = null;
    private UserData ud1 = null;
    private UserData ud2 = null;
//這個面板負責的要求
    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;

    private bool isPopPanel = false;

    private void Start()
    {
        //定義各種聲明
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");
        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");
        //監聽按鈕
        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitClick);
        //定義此面板涉及的要求
        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

        EnterAnim();
    }
    public override void OnEnter()
    {
        if(bluePanel!=null)
            EnterAnim();
    }
    public override void OnExit()
    {
        ExitAnim();
    }
    public override void OnPause()
    {
        ExitAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }

    private void Update() //時時更新玩家資訊
    {
        if (ud != null)
        {
            SetLocalPlayerRes(ud.Username, ud.TotalCount.ToString(), ud.WinCount.ToString());//設置自己的個人戰績
            ClearEnemyPlayerRes();//創建存放敵人戰績的物件，等有資料就放進來顯示
            ud = null;
        }
        if (ud1 != null)
        {
            SetLocalPlayerRes(ud1.Username, ud1.TotalCount.ToString(), ud1.WinCount.ToString());//設置自己的個人戰績
            if (ud2 != null)
                SetEnemyPlayerRes(ud2.Username, ud2.TotalCount.ToString(), ud2.WinCount.ToString());//設置敵方的個人戰績
            else
                ClearEnemyPlayerRes();
            ud1 = null; ud2 = null;
        }
        if (isPopPanel)
        {
            uiMng.PopPanel();
            isPopPanel = false;
        }
    }

    public void SetLocalPlayerResSync()//同樣函式名稱，不同參數
    {
        ud = facade.GetUserData();//設置玩家資訊前，要先透過facade抓到玩家資料
    }
    public void SetAllPlayerResSync(UserData ud1,UserData ud2)
    {
        this.ud1 = ud1;
        this.ud2 = ud2;
    }
    public void SetLocalPlayerRes(string username, string totalCount, string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数：" + totalCount;
        localPlayerWinCount.text = "胜利：" + winCount;
    }
    private void SetEnemyPlayerRes(string username, string totalCount, string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数：" + totalCount;
        enemyPlayerWinCount.text = "胜利：" + winCount;
    }
    public void ClearEnemyPlayerRes()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "等待玩家加入....";
        enemyPlayerWinCount.text = "";
    }

    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }
    private void OnExitClick()
    {
        quitRoomRequest.SendRequest();
    }
    public void OnExitResponse()
    {
        isPopPanel = true;
    }
    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail) //確定是房主才可以開始遊戲
        //如何確定是不是房主? 透過returnCode來分辨
        {
            uiMng.ShowMessageSync("您不是房主，无法开始游戏！！");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.Game);
            facade.EnterPlayingSync();
        }
    }

    private void EnterAnim()//面板顯示後，執行動畫
    {
        gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 0, 0);
        bluePanel.DOLocalMoveX(-174, 0.4f);
        redPanel.localPosition = new Vector3(1000, 0, 0);
        redPanel.DOLocalMoveX(174, 0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        redPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(() => gameObject.SetActive(false));//執行完動畫把面板隱藏
    }
}

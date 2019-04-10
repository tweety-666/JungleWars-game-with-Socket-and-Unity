using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;
public class RoomListPanel : BasePanel {
    //先聲明遊戲物件
    private RectTransform battleRes;//對戰結果
    private RectTransform roomList;//房間清單
    private VerticalLayoutGroup roomLayout;//房間清單的排版
    private GameObject roomItemPrefab; //RoomItem的類，做成預製資源
    private ListRoomRequest listRoomRequest; //這UI介面上有哪些Request //列出房間Request
    private CreateRoomRequest createRoomRequest; //創建房間Request
    private JoinRoomRequest joinRoomRequest;//加入房間Request
    private List<UserData> udList = null; //玩家清單 //需要先建立好UserData的類

    private UserData ud1 = null; //玩家1 //這遊戲是一打一
    private UserData ud2 = null; //玩家2

    //聲明好遊戲物件後，要定義    
    private void Start()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();//房間列表是垂直排列的列表
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;//RoomItem的類，是prefab
        //監聽按鈕，點擊觸發函式
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("RoomList/CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
        listRoomRequest = GetComponent<ListRoomRequest>();
        createRoomRequest = GetComponent<CreateRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
        //進場動畫
        EnterAnim();
    }
    public override void OnEnter()
    {
        SetBattleRes(); //進入面板就先呈現個人戰積
        if (battleRes != null)
            EnterAnim();
        if(listRoomRequest==null)//進入房間面板就列出房間
            listRoomRequest = GetComponent<ListRoomRequest>();
        listRoomRequest.SendRequest(); //面板叫Request腳本發出要列出房間清單的Request
        
    }

    public override void OnExit()
    {
        HideAnim();
    }
    public override void OnPause()
    {
        HideAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
        listRoomRequest.SendRequest(); //刷新，再列一次房間清單
        //面板叫Request腳本發出要列出房間清單的Request
    }

    private void Update() //時時更新玩家資訊
    {
        if (udList != null)
        {
            LoadRoomItem(udList);//載入玩家清單
            udList = null;
        }
        if (ud1 != null && ud2 != null)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResSync(ud1, ud2);//設置兩位玩家
            ud1 = null;ud2 = null;
        }
    }


    private void OnCloseClick()
    {
        PlayClickSound();//父類的函式，facade會呼叫AudioMng播放音效
        uiMng.PopPanel();
    }
    private void OnCreateRoomClick() //按下創建房間的按鈕，就觸發此函式
    {
        BasePanel panel= uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SetPanel(panel);
        createRoomRequest.SendRequest();//createRoomRequest會發出要求
    }
    private void OnRefreshClick() //按下刷新按鈕，觸發此函式，listRoomRequest會發出要求
    {
        listRoomRequest.SendRequest();
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);

        battleRes.localPosition = new Vector3(-1000, 0);
        battleRes.DOLocalMoveX(-290, 0.5f);

        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(171, 0.5f);
    }
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);

        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void SetBattleRes() //設置戰績，一進入此面板就會執行
    {
        UserData ud = facade.GetUserData();//設置個人戰績前，得先抓到UserData //facade單例負責去抓
        transform.Find("BattleRes/Username").GetComponent<Text>().text = ud.Username;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数:"+ud.TotalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利:"+ud.WinCount.ToString();
    }
    public void OnUpdateResultResponse(int totalCount,int winCount)//Response結尾都是後端發回來後，前端的回應函式
    {
        facade.UpdateResult(totalCount, winCount);//叫此面板更新戰績，函式會叫facade單例更新戰績，再執行設置戰績的函式
        SetBattleRes();
    }
    public void LoadRoomItemSync(List<UserData> udList)
    {
        this.udList = udList;
    }
    private void LoadRoomItem( List<UserData> udList )//列每個房間RoomItem到列表上，同時載入UserData
    {
        RoomItem[] riArray= roomLayout.GetComponentsInChildren<RoomItem>();//抓到垂直的Layout下的子層(RoomItem)
        foreach(RoomItem ri in riArray)
        {
            ri.DestroySelf();//陣列內每個項目都摧毀
        }
        int count = udList.Count;//依據udList.Count數量，重新載入
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);//載入房間的Prefab
            roomItem.transform.SetParent(roomLayout.transform); //設置房間的父層，父層是垂直的Layout
            UserData ud = udList[i];
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id, ud.Username, ud.TotalCount, ud.WinCount,this);//設置房間資訊
        }
        int roomCount = GetComponentsInChildren<RoomItem>().Length;//抓到RoomItem數目
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta; //抓取垂直Layout的長度
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, //重新設置roomLayout長度，因為多增加房間項目，Layout也得變長
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));//RoomItem數目 * (一個房間在列表所需的長度+spacing)
    }
    public void OnJoinClick(int id) //Click結尾都是按扭按下去後，要執行的函式
    {
        joinRoomRequest.SendRequest(id);//按下加入房間按鈕，此面板找joinRoomRequest去發送要求給後端
        //參數是房間id
    }
    public void OnJoinResponse( ReturnCode returnCode,UserData ud1,UserData ud2) //前端收到後端資訊，做出回應
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("房间被销毁无法加入");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("房间已满，无法加入");
                break;
            case ReturnCode.Success: //可以加入就設置雙方玩家的id
                this.ud1 = ud1;
                this.ud2 = ud2;
                break;
        }
    }
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        LoadRoomItem(1);
    //    }
    //}
}

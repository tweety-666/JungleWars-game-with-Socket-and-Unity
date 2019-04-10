using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour { //RoomItem是prefab，可掛載到場景上
    //聲明遊戲物件，房間要顯示那些資訊要先想好
    public Text username;
    public Text totalCount;
    public Text winCount;
    public Button joinButton;

    private int id; //房間ID
    private RoomListPanel panel; //每個房間要顯示在這個UI上

	// Use this for initialization
	void Start () {
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
            //監聽按鈕，點擊後就執行加入房間函式
        }
    }
    //函式名稱一樣但參數不同
    //設置房間資訊
    public void SetRoomInfo(int id,string username, int totalCount, int winCount,RoomListPanel panel)
    {
        SetRoomInfo(id, username, totalCount.ToString(), winCount.ToString(), panel);
    }
    public void SetRoomInfo(int id,string username, string totalCount, string winCount, RoomListPanel panel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = "总场数\n" + totalCount;
        this.winCount.text = "胜利\n" + winCount;
        this.panel = panel;
    }
    //點擊加入房間按鈕後，會呼叫RoomListPanel這個UI介面，它又呼叫對應的Request(joinRoomRequest)去執行要求函式
    private void OnJoinClick()
    {
        panel.OnJoinClick(id);
    }
    //刪除房間，房間這個遊戲物件就會銷毀
    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}

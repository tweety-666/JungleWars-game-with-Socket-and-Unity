using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserData //負責玩家資訊的類
{
    public UserData(string userData)//定義這個類裡面有啥成員
    {   //用','來分隔每筆資料，並存放在字串陣列內
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);//字串內的第1筆資料，轉成int，成為這筆資料的id
        this.Username = strs[1];//字串內的第2筆資料，成為這筆資料的名稱
        this.TotalCount = int.Parse(strs[2]);//字串內的第3筆資料，轉成int，總開戰次數
        this.WinCount = int.Parse(strs[3]);//字串內的第4筆資料，轉成int，成為這筆資料的勝數
    }
    public UserData(string username, int totalCount, int winCount) //不同的參數，但函式名稱一樣
    {
        this.Username = username;
        this.TotalCount = totalCount;
        this.WinCount = winCount;
    }
    public UserData(int id,string username, int totalCount, int winCount)
    {
        this.Id = id;
        this.Username = username;
        this.TotalCount = totalCount;
        this.WinCount = winCount;
    }
    //定義屬性，讓變數的設置(set)只能是讀取，不能外部改變
    public int Id { get;private set; }
    public string Username { get;private set; }
    public int TotalCount { get;  set; }
    public int WinCount { get;  set; }
    
}

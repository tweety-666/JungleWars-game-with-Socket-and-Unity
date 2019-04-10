using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;

/// <summary>
/// 这个是用来管理跟服务器端的Socket连接
/// </summary>
public class ClientManager :BaseManager {
    //建立IP跟port碼
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket; //需要using System.Net.Sockets;
    private Message msg = new Message(); //建立要傳送的Msg

    //ClientManager的構造函式，因為BaseManager有構造函式，找上頭的GameFacade，所以繼承的腳本也得有。
    public ClientManager(GameFacade facade) : base(facade) { }

    public override void OnInit() //初始化就連線。BaseManager的構造函式有虛方法OnInit()，繼承者要複寫OnInit()。
    {
        base.OnInit();

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);//建立並連結到伺服器，並用try、catch來測是否連上。
            Start();
        }
        catch (Exception e) //Exception需要using System;沒連到要報錯。
        {
            Debug.LogWarning("无法连接到服务器端，请检查您的网络！！" + e);
        }
    }
    private void Start() //初始化建立連線了，開始接收訊息
    {   //msg.Data是讀取數據  msg.StartIndex是開始讀取數  msg.RemainSize是讀取最大數
        clientSocket.BeginReceive(msg.Data,msg.StartIndex,msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }
    private void ReceiveCallback(IAsyncResult ar) //接到後成功收到就繼續接，沒接到會中斷，這裡是TCP機制。
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return; //沒連上就return
            int count = clientSocket.EndReceive(ar);//成功收到，就結束這次接收並讀取訊息，讀取後執行OnProcessDataCallback

            msg.ReadMessage(count, OnProcessDataCallback);

            Start();//成功收到就繼續接
        }
        catch(Exception e) //沒收到就報錯
        {
            Debug.Log(e);
        }
    }
    private void OnProcessDataCallback(ActionCode actionCode,string data)
    {
        facade.HandleReponse(actionCode, data);//讀取後執行OnProcessDataCallback，開始處理訊息
    }

    //這邊紀錄傳送訊息的函式，給其他Mng調用。其他Mng有需要傳訊息，就找ClientManager處理傳訊息給伺服器。
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);//打包訊息
        clientSocket.Send(bytes);//傳送訊息
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close(); //關閉伺服器
        }catch(Exception e)
        {
            Debug.LogWarning("无法关闭跟服务器端的连接！！" + e);
        }
    }
}
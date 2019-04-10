using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerManager : BaseManager
{//構造函式
    public PlayerManager(GameFacade facade) : base(facade) { }
//聲明
    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>(); 
    //角色資訊字典，之所以建立這個是要透過字典查找，來給予prefab
    //找RoleType來給予RoleData //RoleType也只有紅藍兩種
    //根據RoleType，也可以得知是否是本地端角色，如果是我方一定是藍色(本地端)
    private Transform rolePositions; //角色誕生的位置

    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;//本地端遊戲物件
    private GameObject playerSyncRequest;
    private GameObject remoteRoleGameObject;//遠端遊戲物件
//聲明這UI涉及的請求
    private ShootRequest shootRequest;
    private AttackRequest attackRequest;
//更新戰績資訊
    public void UpdateResult(int totalCount,int winCount)
    //後端回傳資訊，前端收到後會叫ui面板做回應，於是會透過facade單例執行函式，facade單例又會叫playerMng執行更新戰績函式
    {
        userData.TotalCount = totalCount;
        userData.WinCount = winCount;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        currentRoleType = rt;
    }
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    //初始化
    public override void OnInit()
    {
        rolePositions = GameObject.Find("RolePositions").transform; //找到物件位置
        InitRoleDataDict();//初始化角色資訊字典
    }
    private void InitRoleDataDict()
    {   //角色字典增加紅藍方，並new一個出來
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Arrow_BLUE", "Explosion_BLUE",rolePositions.Find("Position1")));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Arrow_RED", "Explosion_RED", rolePositions.Find("Position2")));
    }

    //利用角色資訊字典誕生玩家的角色
    public void SpawnRoles()
    {   //遍歷字典內的項目(角色資訊)
        foreach(RoleData rd in roleDataDict.Values)
        {
            GameObject go= GameObject.Instantiate(rd.RolePrefab, rd.SpawnPosition, Quaternion.identity);//遊戲物件(角色Prefab)生成
            go.tag = "Player";//給予tag
            //如果RoleType跟現在的RoleType一樣，就確定是Local(本地端)角色
            if (rd.RoleType == currentRoleType)
            {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
            }
            else
            {
                remoteRoleGameObject = go; //如果不同就是遠端的角色
            }
        }
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return currentRoleGameObject;
    }
    private RoleData GetRoleData(RoleType rt)
    {
        RoleData rd = null;
        roleDataDict.TryGetValue(rt, out rd);//去字典內給RoleType找RoleData
        return rd;
    }
    public void AddControlScript() //針對動畫控制器的函式
    {
        currentRoleGameObject.AddComponent<PlayerMove>();//目前的遊戲物件要增加PlayerMove腳本(讓物件動起來)
        PlayerAttack playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleData(rt);
        playerAttack.arrowPrefab = rd.ArrowPrefab;
        playerAttack.SetPlayerMng(this);
    }
    public void CreateSyncRequest() //創造同步 //把這個當成雙方同步的橋樑
    {
        playerSyncRequest=new GameObject("PlayerSyncRequest");//創造一個GameObject
        playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(currentRoleGameObject.transform, currentRoleGameObject.GetComponent<PlayerMove>())
            .SetRemotePlayer(remoteRoleGameObject.transform);
        //給這個GameObje這個添加MoveRequest（同步移動的請求）並設置本地端角色（設置位置跟移動腳本）
        //也要設置遠端的角色（設置位置）
        //再來添加攻擊請求（負責叫後端扣受攻擊玩家的血）跟射擊請求（負責箭）
        shootRequest=playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerMng = this;
        attackRequest = playerSyncRequest.AddComponent<AttackRequest>();
    }
    public void Shoot(GameObject arrowPrefab,Vector3 pos,Quaternion rotation) //射擊函式，跟箭有關
    {
        facade.PlayNormalSound(AudioManager.Sound_Timer);//射擊時，facade會叫AudioMng放音效
        GameObject.Instantiate(arrowPrefab, pos, rotation).GetComponent<Arrow>().isLocal = true;//用prefab產生箭//並告知是本地端的
        shootRequest.SendRequest(arrowPrefab.GetComponent<Arrow>().roleType, pos, rotation.eulerAngles);//執行射擊函式時，要請shootRequest傳送Request
        //參數放:看是哪方的箭（紅、藍方）、位置、旋轉角度
    }
    public void RemoteShoot(RoleType rt, Vector3 pos, Vector3 rotation) //遠端射擊函式
    {
        GameObject arrowPrefab = GetRoleData(rt).ArrowPrefab;//根據RoleData判斷是哪方玩家，給予箭的Prefab
        //生成、設置箭的位置、旋轉角度
        Transform transform = GameObject.Instantiate(arrowPrefab).GetComponent<Transform>();
        transform.position = pos;
        transform.eulerAngles = rotation;
    }
    public void SendAttack(int damage)　//執行攻擊函式，會叫攻擊請求傳送Request
    {
        attackRequest.SendRequest(damage);
    }
    public void GameOver() //遊戲結束，由PlayerMng執行函式，因為遊戲結束後要摧毀某些跟Player有關的物件
    {
        //private GameObject currentRoleGameObject;
        //private GameObject playerSyncRequest;
        //private GameObject remoteRoleGameObject;

        //private ShootRequest shootRequest;
        //private AttackRequest attackRequest;
        GameObject.Destroy(currentRoleGameObject);//本地端現有物件
        GameObject.Destroy(playerSyncRequest);//同步的橋樑
        GameObject.Destroy(remoteRoleGameObject);//遠端物件
        shootRequest = null;
        attackRequest = null;
    }
}

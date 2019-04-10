using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData //把角色建立成一個類，因為有很多資訊
{
    private const string PREFIX_PREFAB = "Prefabs/";

//屬性，外界只能讀取，內部才能修改
    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject ArrowPrefab { get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplostionEffect { get; private set; }

//構造函式
//用來構造角色，參數是roleType、角色prefabs路徑、箭prefabs路徑、粒子prefabs路徑、生成位置
//也用來放在字典內提供查找，RoleType是key，RoleData是value
    public RoleData(RoleType roleType,string rolePath,string arrowPath,string explosionPath, Transform spawnPos)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load(PREFIX_PREFAB+ rolePath) as GameObject;
        this.ArrowPrefab = Resources.Load(PREFIX_PREFAB + arrowPath) as GameObject;
        this.ExplostionEffect = Resources.Load(PREFIX_PREFAB + explosionPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explosionEffect = ExplostionEffect;
        this.SpawnPosition = spawnPos.position;
    }
}

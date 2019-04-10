using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject arrowPrefab;
    private Animator anim;
    private Transform leftHandTrans;
    private Vector3 shootDir;
    private PlayerManager playerMng;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))//角色處於原地站立的動畫狀態時
        {
            if (Input.GetMouseButtonDown(0))//按下指定按鈕就...
            {
                Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);//根據滑鼠點擊位置產生Ray
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    targetPoint.y = transform.position.y; //攻擊高度上保持一致
                    shootDir = targetPoint - transform.position; //取得攻擊的方向
                    transform.rotation = Quaternion.LookRotation(shootDir);
                    anim.SetTrigger("Attack");//設置動畫Trigger，有這個Trigger，會執行Attack動畫
                    Invoke("Shoot", 0.1f);
                }
            }
        }
	}
    public void SetPlayerMng(PlayerManager playerMng) //設定Manager
    {
        this.playerMng = playerMng;
    }
    private void Shoot() //發動攻擊的箭prefabs是由playerMng來管理
    {
        playerMng.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir)); //生成箭Prefab，參數分別是Prefab、位置、旋轉角度
    }
}

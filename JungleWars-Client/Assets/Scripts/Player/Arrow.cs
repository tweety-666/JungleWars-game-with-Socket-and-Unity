using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Arrow : MonoBehaviour {
    public RoleType roleType; //聲明，判斷箭是屬於紅方還是藍方的
    public int speed = 5;
    public GameObject explosionEffect; //箭碰撞後產生的粒子效果
    public bool isLocal = false;//判斷是否本地端，預設為否
    private Rigidbody rgd; //碰撞需要
	// Use this for initialization
	void Start () {
        rgd = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rgd.MovePosition( transform.position+ transform.forward * speed * Time.deltaTime);
	}
    private void OnTriggerEnter(Collider other)//碰撞體碰撞
    {
        if (other.tag == "Player")//如果是撞擊到Player
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ShootPerson);//發出打到人的音效
            //可以把playerIsLocal，想成"打到的player是否為Local"
            if (isLocal)//如果是本地端，playerIsLocal就改為false
            {   
                bool playerIsLocal = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != playerIsLocal)//本地端跟打到的玩家是不同端，就可執行攻擊
                {
                    GameFacade.Instance.SendAttack( Random.Range(10,20) );//攻擊，傷害值是10~20隨機值
                }
            }
        }
        else
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Miss);//沒撞擊到玩家，發出miss音效
        }
        //只要有撞擊，不管撞擊到何物，生成粒子prefab，然後消除被打到的遊戲物件
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}

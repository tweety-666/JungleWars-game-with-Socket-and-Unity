using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float forward = 0;

    private float speed = 3;
    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //角色的動畫控制器 //初始狀態是站立的(Grounded) //如果不是Grounded就return
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false) return;
        float h = Input.GetAxis("Horizontal");//水平移動轉成浮點數字
        float v = Input.GetAxis("Vertical");//垂直移動轉成浮點數字

        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0) //如果有移動
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);//移動控制

            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));//旋轉控制

            float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));//動畫控制 //先抓到最大移動浮點數
            forward = res;
            anim.SetFloat("Forward", res);//設置動畫Forward
        }
	}
}

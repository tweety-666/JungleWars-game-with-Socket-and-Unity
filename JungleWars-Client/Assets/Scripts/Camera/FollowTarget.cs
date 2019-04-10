using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {//這個類是用來定義相機的位置跟目標的距離
    public Transform target; //相機要跟隨的目標
    private Vector3 offset = new Vector3(0, 11.98111f, -14.10971f);//一定的相機距離
    private float smoothing = 2; //滑順的數值
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = target.position + offset; //目標位置+一定的相機距離
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);//現在的位置，要去的位置，花多久時間
        transform.LookAt(target);//位置，朝目標方向看去
	}
}

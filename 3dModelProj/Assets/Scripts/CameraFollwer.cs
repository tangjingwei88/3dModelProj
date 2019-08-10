using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollwer : MonoBehaviour {

    private Transform target;
    private Vector3 offset;
    private float speed = 2;
    private Vector3 pos;

	void Start () {
        target = GameObject.FindGameObjectWithTag("player").transform;
        offset = target.position - transform.position;
	}
	
	
	void Update () {
        pos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);

        Quaternion angel = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, angel, speed * Time.deltaTime);
	}
}

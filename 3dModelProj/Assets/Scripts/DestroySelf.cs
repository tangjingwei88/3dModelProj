using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    public float destroyTime = 5f;
    private float timer = 0;
	
	void Start () {
		
	}
	
	
	void Update () {
        timer = timer + Time.deltaTime;
        if(timer > destroyTime)
        {
            Destroy(gameObject);
        }
	}
}

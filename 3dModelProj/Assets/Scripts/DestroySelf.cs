using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    private float destroyTime = 5f;
    private float timer = 0;
	
	void Start () {
		
	}
	
	
	void Update () {
        timer = timer + Time.deltaTime;
        if(timer > 5)
        {
            Destroy(gameObject);
        }
	}
}

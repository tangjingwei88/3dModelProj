using UnityEngine;
using System.Collections;

public class AnimSpeed : MonoBehaviour {
	
	public float animSpeed=1;
	
	// Use this for initialization
	void Start () {
		GetComponent<Animation>()[GetComponent<Animation>().clip.name].speed = animSpeed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

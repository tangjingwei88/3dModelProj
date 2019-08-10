using UnityEngine;
using System.Collections;

public class Tremble : MonoBehaviour {
	
	public float speed=2;
	public float scope=0.1f;
	private Transform myTransform;
	private Vector3 originPos;
	private Vector3 lastPos;
	private Vector3 targetPos;
	private float t;
	public bool X=true;
	public bool Y=true;
	public bool Z=true;
	private Vector3 randPos;
	
	
	void Awake ()
	{
		myTransform=transform;
		originPos=myTransform.position;
	}
	
	// Use this for initialization
	void Start () 
	{
		randPos=Random.insideUnitSphere*scope;
		if (X==false)
			randPos=new Vector3(0, randPos.y, randPos.z);
		if (Y==false)
			randPos=new Vector3(randPos.x, 0, randPos.z);
		if (Z==false)
			randPos=new Vector3(randPos.x, randPos.y, 0);
		targetPos=originPos+randPos;
		
		lastPos=myTransform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (t<=1)
		{
			t=t+Time.deltaTime*speed/(targetPos-lastPos).magnitude;
			myTransform.position=Vector3.Slerp(lastPos, targetPos, t);
		}
		else
		{
			lastPos=myTransform.position;
			
			randPos=Random.insideUnitSphere*scope;
			if (X==false)
				randPos=new Vector3(0, randPos.y, randPos.z);
			if (Y==false)
				randPos=new Vector3(randPos.x, 0, randPos.z);
			if (Z==false)
				randPos=new Vector3(randPos.x, randPos.y, 0);
			
			targetPos=originPos+randPos;
			t=0;
		}
		
		
	}
	
	
}

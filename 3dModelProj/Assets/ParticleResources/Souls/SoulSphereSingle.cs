using UnityEngine;
using System.Collections;

public class SoulSphereSingle : MonoBehaviour {
	
	public Transform target;
	public Transform emitCore;
	public float minRadius=20;
	public float maxRadius=30;
	public int soulNum=15;     //鐏甸瓊鏁扮洰
	public float initSpeed=20; //鍒濆?鐖嗗紑閫熷害
	public float speed=20;     //鍒濆?椋炶?閫熷害
	public float maxSpeed=80;  //鏈澶ч?琛岄熷害
	public float speedUpFactor=5;  //鍔犻熷害
	public float rand=100;         //闅忔満椋炶?
	private TrailRenderer trailRenderer;
	private Rigidbody myRigidbody;
	private Transform myTransform;
	private Vector3 flyForce;
	private Vector3 MoveDirection;
	private Vector3 targetDirection;
	private static Transform soulParent;
	private GameObject soulClone;
	private float trailTime;
	private int i;
	private bool stopFlag;
	private Vector3 emitCorePos;
	
	Vector3 randUnitSphere;
	Vector3 minSphere;
	Vector3 maxSphere;
	float tRand;
	Vector3 randPos;
	
	void Awake ()
	{
		myTransform=transform;
		myRigidbody=GetComponent<Rigidbody>();
	}
	
	void Start () 
	{
		emitCorePos=emitCore.position;
		trailRenderer=GetComponent<TrailRenderer>();
		trailTime=trailRenderer.time;
		if (soulParent==null)
		{
			soulParent=(new GameObject("soulParent")).transform;
			myTransform.parent=soulParent;
		}
		myTransform.position=emitCorePos+GetRandomPos();
		myRigidbody.velocity = (myTransform.position-emitCorePos).normalized*initSpeed;
	}
	
	public Vector3 GetRandomPos()
	{
		randUnitSphere = Random.onUnitSphere;
		minSphere = randUnitSphere * minRadius;
		maxSphere = randUnitSphere * maxRadius;
		tRand = Random.value;
		randPos = (1-tRand)*minSphere+tRand*maxSphere;
		return randPos;
	}
	
	public IEnumerator Split()
	{
		for (i=0;i<soulNum-1;i++)
		{
			GameObject soulClone=(GameObject)Instantiate(gameObject, emitCorePos+GetRandomPos(), myTransform.rotation);
			soulClone.transform.parent=soulParent;
			yield return new WaitForFixedUpdate();
		}
	}
	
	void FixedUpdate () 
	{
		if (stopFlag || target==null) return;
		
		if (speed<maxSpeed)
			speed=speed+Time.deltaTime*speedUpFactor;
		
		MoveDirection=myRigidbody.velocity;
		targetDirection=(target.position-myTransform.position).normalized*speed;
		flyForce=targetDirection-MoveDirection;
		//~ myTransform.rotation=Quaternion.LookRotation(targetDirection);
		flyForce=flyForce+new Vector3(Random.Range(-rand, rand), Random.Range(-rand, rand), Random.Range(-rand, rand));
		myRigidbody.AddForce(flyForce, ForceMode.Acceleration);
		
		if ((target.position-myTransform.position).magnitude<1f)
		{
			myRigidbody.velocity=Vector3.zero;
			Destroy(myRigidbody);
			myTransform.parent=target;
			myTransform.localPosition=Vector3.zero;
			Destroy(gameObject, trailTime);
			stopFlag=true;
		}
		
	}
	
	
}

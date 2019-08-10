using UnityEngine;
using System.Collections;

public class SoulCircleFly : MonoBehaviour {
	
	public Transform target;
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
	static private int totalEmitNum;
	static private int totalLeftNum;
	static private Transform soulParent;
	private GameObject soulClone;
	private float trailTime;
	private int i;
	public bool stopFlag;
	private static bool firstSoul=true;
	private static Vector3 emitCorePos;
	
	void Start () 
	{
		Vector3 randUnitSphere;
		Vector3 minSphere;
		Vector3 maxSphere;
		float rand;
		Vector3 randPos;
		
		myRigidbody=GetComponent<Rigidbody>();
		myTransform=transform;
		if (firstSoul)
		{
			emitCorePos=target.position;
			randUnitSphere = Random.onUnitSphere;
			minSphere = randUnitSphere * minRadius;
			maxSphere = randUnitSphere * maxRadius;
			rand = Random.value;
			randPos = (1-rand)*minSphere+rand*maxSphere;
			myTransform.position=emitCorePos+randPos;  //move itself to sphere surface random position
			totalLeftNum=soulNum;
			firstSoul=false;
		}
		trailRenderer=GetComponent<TrailRenderer>();
		trailTime=trailRenderer.time;
		if (soulParent==null)
		{
			soulParent=(new GameObject("soulParent")).transform;
			myTransform.parent=soulParent;
		}
		if (totalEmitNum<soulNum-1)
		{
			randUnitSphere = Random.onUnitSphere;
			minSphere = randUnitSphere * minRadius;
			maxSphere = randUnitSphere * maxRadius;
			rand = Random.value;
			randPos = (1-rand)*minSphere+rand*maxSphere;
			GameObject soulClone=(GameObject)Instantiate(gameObject, emitCorePos+randPos, myTransform.rotation);
			soulClone.transform.parent=soulParent;
			totalEmitNum=totalEmitNum+1;
		}
		
		myRigidbody.velocity = (myTransform.position-emitCorePos).normalized*initSpeed;
	}
	
	void FixedUpdate () 
	{
		if (stopFlag) return;
		
		if (speed<maxSpeed)
			speed=speed+Time.deltaTime*speedUpFactor;
		
		MoveDirection=myRigidbody.velocity;
		targetDirection=(target.position-myTransform.position).normalized*speed;
		flyForce=targetDirection-MoveDirection;
		//~ myTransform.rotation=Quaternion.LookRotation(targetDirection);
		flyForce=flyForce+new Vector3(Random.Range(-rand, rand), Random.Range(-rand, rand), Random.Range(-rand, rand));
		myRigidbody.AddForce(flyForce, ForceMode.Acceleration);
		
		if ((target.position-myTransform.position).magnitude<0.5f)
		{
			myRigidbody.velocity=Vector3.zero;
			Destroy(myRigidbody);
			myTransform.parent=target;
			myTransform.localPosition=Vector3.zero;
			Destroy(gameObject, trailTime);
			totalLeftNum=totalLeftNum-1;
			if (totalLeftNum<=0)
			{
				Destroy(target.gameObject);
			}
			stopFlag=true;
		}
		
	}
	
	
}

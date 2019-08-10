using UnityEngine;
using System.Collections;

public class SwordRain : MonoBehaviour {
	
	public float radius = 20;   //特效半径
	public float time=5;        //持续时间
	public float interval=1;    //发生的间隔时间
	public float density=2;    //每次同时发射的数目
	public float scaleMin=1;     //剑大小的最小随机数
	public float scaleMax=1;     //剑大小的最大随机数
	public GameObject swordPrefab;  //剑的prefab
	
	public float explosionRadius = 1.0f;  	//爆炸的球形半径
	public float power = 200.0f;           	//爆炸的力度
	public float powerUp = 0.5f;				//碎片向上弹跳的程度
	public float inDepthBias=1f;              //插入地面的随机深度
	public float lastTime=2f;                  //剑在地面上持续的时间
	public float splitLastTime=3f;            //碎片在地面上持续的时间
	public int splitNum=5;                      //产生碎片的个数
	public Vector3 gravity=new Vector3(0, -9.8f, 0);  //使用的重力
	
	public GameObject splitPrefab;
	public bool physicSplit=true;
	public float splitScaleMin=1;     //碎片大小的最小随机数
	public float splitScaleMax=1;     //碎片大小的最大随机数
	
	private GameObject swordClone;
	private Transform myTransform;
	private Vector2 randPos2D;
	private Vector3 biasPosition;
	private float t;
	private float t1;
	private float i;
	private float deltatime;
	private Vector3 adjustPos;
	private Vector3 groundPos;
	
	// Use this for initialization
	void Start () 
	{
		swordPrefab.SetActive(false);
		splitPrefab.SetActive(false);
		SwordExplosion swordExplosion = swordPrefab.AddComponent<SwordExplosion>();
		swordExplosion.Init(explosionRadius, power, powerUp, inDepthBias, lastTime, splitLastTime, splitNum, gravity, splitPrefab, physicSplit, splitScaleMin, splitScaleMax);
		myTransform=transform;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Sphere"), LayerMask.NameToLayer("Cube"), true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Cube"), LayerMask.NameToLayer("Cube"), true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Sphere"), LayerMask.NameToLayer("Sphere"), true);
		if (scaleMin>scaleMax)
		{
			scaleMin=scaleMax;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		deltatime=Time.deltaTime;
		if (t<time)
		{
			if (t1<interval)
			{
				t1=t1+deltatime;
			}
			else
			{
				for (i=0;i<density;i++)
				{
					randPos2D=Random.insideUnitCircle*radius;
					biasPosition=new Vector3(randPos2D.x, 0, randPos2D.y);
					groundPos = SwordExplosion.GetGroundPos(myTransform.position, gravity);
					Vector3 adjustBias = myTransform.position - groundPos;
					adjustPos = myTransform.position + new Vector3(adjustBias.x, 0, adjustBias.z);
					swordClone=(GameObject) Instantiate(swordPrefab, adjustPos+biasPosition, Quaternion.identity);
					float rand=Random.value;
					swordClone.transform.localScale=swordClone.transform.localScale * ((1-rand)*scaleMin+rand*scaleMax);
					swordClone.transform.parent=myTransform;
				}
				t1=0;
			}
			t=t+deltatime;
		}
		
		
	}
	
	
	
}




using UnityEngine;
using System.Collections;

public class SwordExplosion : MonoBehaviour {
	
	public float explosionRadius = 5.0f;  	//爆炸的球形半径
	public float power = 350.0f;           	//爆炸的力度
	public float powerUp = 0.1f;				//碎片向上弹跳的程度
	public float inDepthBias=0.2f;              //插入地面的随机深度
	public float lastTime=2f;                  //剑在地面上持续的时间
	public float splitLastTime=3f;            //碎片在地面上持续的时间
	public int splitNum=5;                      //产生碎片的个数
	public Vector3 gravity=new Vector3(0, -9.8f, 0);  //使用的重力
	private Vector3 velocity=Vector3.zero;
	public GameObject splitPrefab;
	public bool physicSplit=true;
	public float scaleMin=1;     //碎片大小的最小随机数
	public float scaleMax=1;     //碎片大小的最大随机数
	private GameObject splitClone;
	private Vector3 initPosition;
	private int i;
	private int j;
	private int k;
	private float t;
	private float rand;
	private Transform myTransform;
	private GameObject[] splitObjects;
	private Vector3 targetPos;
	private Vector3 explosionPos;
	private float distance;
	private Transform swordRain;
	private Rigidbody splitRigidbody;
	
	void Awake ()
	{
		myTransform=transform;
	}
	
	public void Init(float explosionRadius0, float power0, float powerUp0, float inDepthBias0, float lastTime0, float splitLastTime0, int splitNum0, Vector3 gravity0, GameObject splitPrefab0, bool physicSplit0, float scaleMin0, float scaleMax0)
	{
		explosionRadius = explosionRadius0;
		power = power0;
		powerUp = powerUp0;
		inDepthBias = inDepthBias0;
		lastTime = lastTime0;
		splitLastTime = splitLastTime0;
		splitNum = splitNum0;
		gravity = gravity0;
		splitPrefab = splitPrefab0;
		physicSplit = physicSplit0;
		scaleMin = scaleMin0;
		scaleMax = scaleMax0;
	}
	
	void Start ()
	{
		splitObjects=new GameObject[splitNum];
		initPosition=myTransform.position;
		targetPos=GetGroundPos(myTransform.position, gravity);
		//特效发生点
		explosionPos=targetPos+new Vector3(0, 0.2f, 0);
		//设置剑插入地面的深度的随机量
		targetPos=targetPos+new Vector3(0, Random.Range(-inDepthBias, inDepthBias), 0);
		distance=(targetPos-initPosition).magnitude;
		if (GameObject.Find("SwordRain"))
			swordRain=GameObject.Find("SwordRain").transform;
		if (scaleMin>scaleMax)
		{
			scaleMin=scaleMax;
		}
	}
	
	void Update ()
	{
		//创建爆开特效的物体
		if (j<splitNum)
		{
			Vector2 newPosition = Random.insideUnitCircle*0.5f;
			splitClone=(GameObject) Instantiate(splitPrefab, explosionPos+new Vector3(newPosition.x, 0, newPosition.y), Quaternion.identity);
			float rand=Random.value;
			splitClone.transform.localScale=splitClone.transform.localScale * ((1-rand)*scaleMin+rand*scaleMax);
			splitObjects[j]=splitClone;
			splitObjects[j].transform.parent=swordRain;
			//关闭模型显示
			if (splitObjects[j].GetComponent<Renderer>()!=null)
			{
				splitObjects[j].GetComponent<Renderer>().enabled=false;
			}
			//关闭粒子发射
			if (splitObjects[j].GetComponent<ParticleEmitter>()!=null)
			{
				splitObjects[j].GetComponent<ParticleEmitter>().enabled=false;
			}
			//处理子模型和粒子
			if (splitObjects[j].transform.childCount>0)
			{
				//关闭所有子模型显示
				MeshRenderer[] renderers=splitObjects[j].GetComponentsInChildren<MeshRenderer>();
				for (k=0;k<renderers.Length;k++)
				{
					renderers[k].enabled=false;
				}
				//关闭所有子粒子的发射
				ParticleEmitter[]  particleEmitters=splitObjects[j].GetComponentsInChildren<ParticleEmitter>();
				for (k=0;k<particleEmitters.Length;k++)
				{
					particleEmitters[k].enabled=false;
				}
			}
			//如果需要物理爆炸
			if (physicSplit)
			{
				//添加物理刚体
				if (splitObjects[j].GetComponent<Rigidbody>()==null)
				{
					splitRigidbody=splitObjects[j].AddComponent<Rigidbody>();
					splitRigidbody.isKinematic=true;
				}
			}
			j=j+1;
		}
		//~ Debug.DrawLine(initPosition, explosionPos, Color.white);
		//~ myTransform.position=myTransform.position+velocity*Time.deltaTime;
		if (t<1)
		{
			myTransform.position=initPosition*(1-t)+targetPos*t;
			myTransform.rotation=Quaternion.LookRotation(gravity);
			velocity=velocity+Time.deltaTime*gravity;
			t=t+Time.deltaTime*velocity.magnitude/distance;
		}
		else if (t>1)
		{
			//~ myTransform.position=targetPos;
			for (i=0;i<splitNum;i++)
			{
				//开启模型显示
				if (splitObjects[i] && splitObjects[i].GetComponent<Renderer>()!=null)
				{
					splitObjects[i].GetComponent<Renderer>().enabled=true;
				}
				//开启粒子
				if (splitObjects[i] && splitObjects[i].GetComponent<ParticleEmitter>()!=null)
				{
					splitObjects[i].GetComponent<ParticleEmitter>().enabled=true;
				}
				//处理子模型和粒子
				if (splitObjects[i] && splitObjects[i].transform.childCount>0)
				{
					//打开所有子模型显示
					MeshRenderer[] renderers=splitObjects[i].GetComponentsInChildren<MeshRenderer>();
					for (k=0;k<renderers.Length;k++)
					{
						renderers[k].enabled=true;
					}
					//打开所有子粒子的发射
					ParticleEmitter[] particleEmitters=splitObjects[i].GetComponentsInChildren<ParticleEmitter>();
					for (k=0;k<particleEmitters.Length;k++)
					{
						particleEmitters[k].enabled=true;
					}
				}
				//如果需要物理爆炸
				if (physicSplit)
				{
					splitRigidbody=splitObjects[i].GetComponent<Rigidbody>();
					splitRigidbody.isKinematic=false;
					splitRigidbody.AddExplosionForce(power, explosionPos, explosionRadius, powerUp);
				}
				//延迟删除碎片
				Destroy(splitObjects[i], splitLastTime);
			}
			//延迟删除剑
			Destroy(gameObject, lastTime);
			t=1;
		}
		
	}
	
	static public Vector3 GetGroundPos (Vector3 pos, Vector3 gravity)
	{
		int mask = 1<<LayerMask.NameToLayer("SwordGround");
		RaycastHit hit;
		if (Physics.Raycast (pos, gravity, out hit, Mathf.Infinity, mask))
		{
			return hit.point;
		}
		else
		{
			return pos+gravity*5;
		}
	}
	
	
}
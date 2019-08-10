using UnityEngine;
using System.Collections;


public class ParticleMaster : MonoBehaviour {
	
	private ArrayList childsList;
	public int count;
	public bool once=true;
	public bool[] animOnces;
	
	public ParticleEmitter[] particleArray;
	public float[] particleDelay;
	public float[] particleLastTime;
	
	public Animation[] animArray;
	public float[] animDelay;
//	public float[] animLastTime;
	
	public TrailRenderer[] trailArray;
	public float[] trailDelay;
	public float[] trailLastTime;
	
	private float t;
	public float maxLastTime;
	public float destroyTime=1.2f;
	private MeshRenderer[] meshRenderers;
	public bool repeat = false;
	public float repeatInterval = 1;
	
	public void OnDrawGizmosSelected()
	{
		if (once)
		{
			childsList = new ArrayList();
			childsList.Clear();
			FindChilds(transform);
			int particlesNum = 0;
			int animsNum = 0;
			int trailsNum = 0;
			int i = 0;
			for (i=0;i<childsList.Count;i++)
			{
				ParticleEmitter particle = ((Transform)childsList[i]).GetComponent<ParticleEmitter>();
				Animation anim = ((Transform)childsList[i]).GetComponent<Animation>();
				TrailRenderer trail = ((Transform)childsList[i]).GetComponent<TrailRenderer>();
				if (particle) particlesNum = particlesNum + 1;
				if (anim) animsNum = animsNum + 1;
				if (trail) trailsNum = trailsNum + 1;
			}
			
			particleArray = new ParticleEmitter[particlesNum];
			particleDelay = new float[particlesNum];
			particleLastTime = new float[particlesNum];
			
			animArray = new Animation[animsNum];
			animDelay = new float[animsNum];
//			animLastTime = new float[animsNum];
			
			trailArray = new TrailRenderer[trailsNum];
			trailDelay = new float[trailsNum];
			trailLastTime = new float[trailsNum];
			
			particlesNum = 0;
			animsNum = 0;
			trailsNum = 0;
			for (i=0;i<childsList.Count;i++)
			{
				ParticleEmitter particle = ((Transform)childsList[i]).GetComponent<ParticleEmitter>();
				Animation anim = ((Transform)childsList[i]).GetComponent<Animation>();
				TrailRenderer trail = ((Transform)childsList[i]).GetComponent<TrailRenderer>();
				if (particle)
				{
					particleArray[particlesNum] = particle;
					particlesNum = particlesNum + 1;
				}
				if (anim)
				{
					anim.playAutomatically = false;
					animArray[animsNum] = anim;
					animsNum = animsNum + 1;
				}
				if (trail)
				{
					trailArray[trailsNum] = trail;
					trailsNum = trailsNum + 1;
				}
			}
			count = childsList.Count;
			meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
			once=false;
		}
	}
	
	void Start ()
	{
		int i=0;
		for (i=0;i<particleArray.Length;i++)
		{
			particleArray[i].emit = false;
			if (maxLastTime<particleDelay[i]+particleLastTime[i])
			{
				maxLastTime = particleDelay[i]+particleLastTime[i];
			}
		}
		animOnces = new bool[animArray.Length];
		for (i=0;i<animArray.Length;i++)
		{
			animArray[i].Stop();
			animArray[i].playAutomatically = false;
//			if (animDelay[i]>0)
//			{
//				animArray[i].Stop();
//				animArray[i].playAutomatically = false;
//			}
			animOnces[i] = true;
			
			Debug.Log("this.gameobject.name = " + this.gameObject.name);
			Debug.Log("i = " + i);
			Debug.Log("animArray[i].clip.name = " + animArray[i].clip.name);
			float len = animArray[i][animArray[i].clip.name].length;
			if (maxLastTime<animDelay[i]+len)
			{
				maxLastTime = animDelay[i]+len;
			}
		}
		for (i=0;i<trailArray.Length;i++)
		{
			trailArray[i].enabled = false;
			if (maxLastTime<trailDelay[i]+trailLastTime[i])
			{
				maxLastTime = trailDelay[i]+trailLastTime[i];
			}
		}
		if (repeat==false)
		{
			Destroy(gameObject, maxLastTime+destroyTime);
		}
	}
	
	void Update () 
	{
		int i=0;
		t = t + Time.deltaTime;
		for (i=0;i<particleArray.Length;i++)
		{
			if (t>particleDelay[i] && t<particleDelay[i]+particleLastTime[i])
			{
				if (particleArray[i]!=null)
				{
					particleArray[i].emit = true;
				}
			}
			else
			{
				if (particleArray[i]!=null)
				{
					particleArray[i].emit = false;
				}
			}
		}
		
		for (i=0;i<animArray.Length;i++)
		{
			if (t>=animDelay[i])
			{
				if (animArray!=null && animArray[i]!=null)
				{
					if (animOnces[i])
					{
						animArray[i].Play();
						animOnces[i]=false;
					}
				}
			}
		}
			
		for (i=0;i<trailArray.Length;i++)
		{
			if (t>trailDelay[i] && t<trailDelay[i]+trailLastTime[i])
			{
				if ( trailArray[i] != null )
					trailArray[i].enabled = true;
			}
			else
			{
				if ( trailArray[i] != null )
					trailArray[i].enabled = false;
			}
		}
		if (meshRenderers!=null)
		{
			for (i=0;i<meshRenderers.Length;i++)
			{
				if (t>maxLastTime && t<maxLastTime+destroyTime)
				{
					Color c = meshRenderers[i].material.color;
					float a = (t-maxLastTime)/destroyTime;
					meshRenderers[i].material.color = new Color(c.r, c.g, c.b, c.a*(1-a));
				}
			}
		}
		if (repeat)
		{
			if (t>maxLastTime+repeatInterval)
			{
				t=0;
			}
		}
	}
	
	public void FindChilds(Transform trans)
	{
		childsList.Add(trans);
		if (trans.childCount>0)
		{
			for (int i=0;i<trans.childCount;i++)
			{
				FindChilds(trans.GetChild(i));
			}
		}
	}
	
}


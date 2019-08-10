using UnityEngine;
using System.Collections;

public class RepeatGenerater : MonoBehaviour {
	
	public int repeatNum=1;
	public float interval=1;
	public GameObject obPrefab;
	private GameObject obClone;
	private float t;
	private Transform myTransform;
	private int num;
	
	void Awake ()
	{
		myTransform=transform;
	}
	
	// Use this for initialization
	void Start () {
		obPrefab.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (t<interval)
		{
			t=t+Time.deltaTime;
		}
		else if (t>interval)
		{
			num=num+1;
			if (num==repeatNum+1)
			{
				t=interval;
			}
			else
			{
				obClone = Instantiate(obPrefab, myTransform.position, myTransform.rotation) as GameObject;
				StartCoroutine(obClone.GetComponent<SoulSphereSingle>().Split());
				t=0;
			}
			
		}
		
	}
	
}


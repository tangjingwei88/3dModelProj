using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Controller : MonoBehaviour {
    private GameObject player;
    private GameObject enemy;
    private Vector3 target;
    private bool isOver = true;
    private float speed = 3;
    private Animator ani;
    RaycastHit hitInfo = new RaycastHit();

    private float m_nextFire;
    private float FireRate = 0.25f;
    private GameObject bullet;
    private float bulletSpeed = 10f;

    void Start () {
        player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Hero_Ares.prefab");
        player = Instantiate(player) as GameObject;
        player.transform.rotation = Quaternion.Euler(Vector3.zero) ;
        enemy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Qzhu.prefab");
        Instantiate(enemy);

        ani = player.GetComponent<Animator>();
	}

    private void FixedUpdate()
    {
        m_nextFire = m_nextFire + Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && m_nextFire > 0.5f)
        {
            //获取鼠标坐标并转换成世界坐标
            Vector3 m_mousePosition = Input.mousePosition;
            m_mousePosition = Camera.main.ScreenToWorldPoint(m_mousePosition);

            //计时器归零
            m_nextFire = 0;
            bullet = LoadPrefab("Assets/Prefabs/Particles/Effect_jianshi_di.prefab");
            bullet.transform.position = player.transform.position;
            bullet.transform.rotation = player.transform.rotation;
            bullet.GetComponent<Rigidbody>().velocity = player.transform.forward * bulletSpeed;

        }
        if(Input.GetKey(KeyCode.Q))
        {
            AttackEffect();
        }
    }

    void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            //Debug.LogError("dd");
            //1. 获取鼠标点击位置
            //创建射线;从摄像机发射一条经过鼠标当前位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //发射射线
            
            if(Physics.Raycast(ray,out hitInfo))
            {
                //Debug.LogError("hitInfo.collider.gameObject.tag " + hitInfo.collider.gameObject.tag);
                //获取碰撞点位置
                if (hitInfo.collider.gameObject.tag == "Plane")
                {
                    target = hitInfo.point;

                    GameObject obj = LoadPrefab("Assets/Prefabs/Particles/Seave_qiu.prefab");
                    obj.transform.position = target + new Vector3(0,1f,0);
                    obj.transform.rotation = Quaternion.Euler(Vector3.zero);
                    target.y = 0.5f;
                    isOver = false;
                    //Debug.LogError("target"+ target);
                }
            }
        }
        if (!isOver)
        {
            //Debug.LogError("tar" + target);
            player.transform.LookAt(target);
            ani.Play("Move");
            player.transform.forward = Vector3.Lerp(player.transform.forward,hitInfo.point- player.transform.forward,Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, hitInfo.point, Time.deltaTime * speed);
            if (Vector3.Distance(target, player.transform.position) < 2f)
            {
                ani.CrossFade("Idle",0.1f);
                isOver = true;

            }
        }

        if(Input.GetKey(KeyCode.Space))
        {
            //Debug.LogError("space");
            
            ani.Play("Attack");
            ani.SetFloat("AttackType", 1);
            ani.SetFloat("AttackBlend", 0);
           
        }

    }

    public void AttackEffect()
    {
        for (int i = 0; i < 3;i++)
        {
            GameObject obj = LoadPrefab("Assets/Prefabs/Particles/Effect_jianshi_di.prefab");
            obj.transform.position = player.transform.position + new Vector3(0,200,0);
            //obj.transform.rotation = player.transform.rotation + Quaternion.Euler(new Vector3(30f, 0, 0)) ;
            obj.GetComponent<Rigidbody>().velocity = player.transform.forward * bulletSpeed;

        }
    }

    public GameObject LoadPrefab(string path)
    {
        Object obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject gobj = Instantiate(obj) as GameObject;
        gobj.transform.rotation = Quaternion.Euler(Vector3.zero);
        return gobj;
    }
}

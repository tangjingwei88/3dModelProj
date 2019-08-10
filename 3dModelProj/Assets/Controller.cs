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
    RaycastHit hitInfo = new RaycastHit();

    void Start () {
        player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Hero_Ares.prefab");
        Instantiate(player);
        enemy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Qzhu.prefab");
        Instantiate(enemy);
	}
	

	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Debug.LogError("dd");
            //1. 获取鼠标点击位置
            //创建射线;从摄像机发射一条经过鼠标当前位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //发射射线
            
            if(Physics.Raycast(ray,out hitInfo))
            {
                Debug.LogError("hitInfo.collider.gameObject.tag " + hitInfo.collider.gameObject.tag);
                //获取碰撞点位置
                if (hitInfo.collider.gameObject.tag == "Plane")
                {
                    target = hitInfo.point;
                    target.y = 0.5f;
                    isOver = false;
                    Debug.LogError("target"+ target);
                }
            }
        }
        if (!isOver)
        {
            Debug.LogError("tar" + target);
            player.transform.LookAt(target);

            player.transform.forward = Vector3.Lerp(player.transform.forward,hitInfo.point- player.transform.forward,Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, hitInfo.point, Time.deltaTime * speed);
            if (Vector3.Distance(target, player.transform.position) < 0.5f)
            {
                isOver = true;
                player.transform.position = target;
            }
        }
    }

}

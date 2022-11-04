using System.Collections;
using UnityEngine;

public class NaviControl : MonoBehaviour 
{

	public float speedMove = 5;
	public float speedRotation = 2;

	void Update()
	{
		if(!MouseNavi.isDone || MouseNavi.path.Count == 0 || MouseNavi.index >= MouseNavi.path.Count) return;
		transform.position = Vector3.MoveTowards(transform.position, MouseNavi.path[MouseNavi.index].position, speedMove * Time.deltaTime);
		transform.forward = Vector3.MoveTowards(transform.forward, MouseNavi.direction, speedRotation * Time.deltaTime);
	}
}

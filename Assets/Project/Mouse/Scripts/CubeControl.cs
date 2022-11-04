using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
	public float speedMove = 5;
	public float speedRotation = 2;

	void Update()
	{
		if (!CubeScript.isDone || CubeScript.path.Count == 0 || CubeScript.index >= CubeScript.path.Count) return;
		transform.position = Vector3.MoveTowards(transform.position, CubeScript.path[CubeScript.index].position, speedMove * Time.deltaTime);
		transform.forward = Vector3.MoveTowards(transform.forward, CubeScript.direction, speedRotation * Time.deltaTime);
	}
}

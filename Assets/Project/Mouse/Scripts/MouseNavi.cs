using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseNavi : MonoBehaviour {

	public string playerTag = "Player"; // тег объекта, который должен перемещаться
	public string groundTag = "GameController"; // тег объекта, на котором можно рисовать маршрут
	public float step = 3; // расстояние от точки до точки
	public int maxPathLength = 100; // макс точек маршрута
	public Transform dotPrefab; // префаб для визуализации точки

	private Vector3 position;
	public static List<Transform> path { get; private set; }
	public static bool isDone { get; private set; }
	private bool hitPlayer;
	private Transform player;
	public static Vector3 direction { get; private set; }
	public static int index { get; private set; }

	void Awake()
	{
		path = new List<Transform>();
	}

	void AddDot(Vector3 curPos, Vector3 normal) // добавление префаба на сцену
	{
		if(maxPathLength == path.Count) return;

		Transform t = Instantiate(dotPrefab) as Transform;
		t.position = curPos;
		t.rotation = Quaternion.FromToRotation(Vector3.up, normal);
		path.Add(t);
	}

	void DoAction() // основная функция, создание маршрута
	{
		if(isDone) return;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit)) 
		{
			if(hit.transform.tag == playerTag)
			{
				player = hit.transform;
				position = hit.transform.position;
				hitPlayer = true;
			}

			if(hitPlayer && hit.transform.tag == groundTag)
			{
				float dist = Vector3.Distance(position, hit.point);

				if(dist >= step)
				{
					position = hit.point;
					AddDot(hit.point + hit.normal * 0.15f, hit.normal);
				}
			}
		}
	}

	void UpdatePath() // обновление вектора направления и массива
	{
		if(index == path.Count)
		{
			index = 0;
			path = new List<Transform>();
			isDone = false;
		}

		if(!isDone) return;

		direction = (path[index].position - player.position).normalized;
		float dist = Vector3.Distance(path[index].position, player.position);
		if(dist <= 0.1f)
		{
			Destroy(path[index].gameObject);
			index++;
		}
	}

	void Update()
	{
		if(Input.GetMouseButton(0))
		{
			DoAction();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			isDone = true;
			hitPlayer = false;
		}

		UpdatePath();
	}
}

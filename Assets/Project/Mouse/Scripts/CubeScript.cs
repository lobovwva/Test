using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{ // ��� �� �� ����� ��� � � MouseNavi
	public string playerTag = "Cube"; // ��� �������, ������� ������ ������������
	public string groundTag = "GameController"; // ��� �������, �� ������� ����� �������� �������
	public float step = 3; // ���������� �� ����� �� �����
	public int maxPathLength = 100; // ���� ����� ��������
	public Transform dotPrefab; // ������ ��� ������������ �����

	private Vector3 position;
	public static List<Transform> path { get; private set; }
	public static bool isDone { get; private set; }
	private bool hitCube;
	private Transform cube;
	public static Vector3 direction { get; private set; }
	public static int index { get; private set; }

	void Awake()
	{
		path = new List<Transform>();
	}

	void AddDot(Vector3 curPos, Vector3 normal) // ���������� ������� �� �����
	{
		if (maxPathLength == path.Count) return;

		Transform t = Instantiate(dotPrefab) as Transform;
		t.position = curPos;
		t.rotation = Quaternion.FromToRotation(Vector3.up, normal);
		path.Add(t);
	}

	void DoAction() // �������� �������, �������� ��������
	{
		if (isDone) return;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.tag == playerTag)
			{
				cube = hit.transform;
				position = hit.transform.position;
				hitCube = true;
			}

			if (hitCube && hit.transform.tag == groundTag)
			{
				float dist = Vector3.Distance(position, hit.point);

				if (dist >= step)
				{
					position = hit.point;
					AddDot(hit.point + hit.normal * 0.15f, hit.normal);
				}
			}
		}
	}

	void UpdatePath() // ���������� ������� ����������� � �������
	{
		if (index == path.Count)
		{
			index = 0;
			path = new List<Transform>();
			isDone = false;
		}

		if (!isDone) return;

		direction = (path[index].position - cube.position).normalized;
		float dist = Vector3.Distance(path[index].position, cube.position);
		if (dist <= 0.1f)
		{
			Destroy(path[index].gameObject);
			index++;
		}
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			DoAction();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			isDone = true;
			hitCube = false;
		}

		UpdatePath();
	}
}

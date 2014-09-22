using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
	private PlayerController controller;
	public string action;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controller == null)
		{
			GameObject robot = GameObject.FindWithTag("Player");
			controller = robot.GetComponent<PlayerController>();
		}
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (collider2D == Physics2D.OverlapPoint(touchPos))
				{
					if (Input.GetTouch(i).phase == TouchPhase.Began)
					{
						if (action == "JUMP")
						{
							controller.Jump = true;
						}
						else if (action == "RIGHT")
						{
							controller.Right = true;
						}
						else if (action == "LEFT")
						{
							controller.Left = true;
						}
					}
					else if (Input.GetTouch(i).phase == TouchPhase.Ended)
					{
						if (action == "JUMP")
						{
							controller.Jump = false;
						}
						else if (action == "RIGHT")
						{
							controller.Right = false;
						}
						else if (action == "LEFT")
						{
							controller.Left = false;
						}
					}
					break;
				}
			}
		}
	}
}

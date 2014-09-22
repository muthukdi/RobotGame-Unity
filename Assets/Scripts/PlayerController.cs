using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	private bool LEFT;
	private bool RIGHT;
	private bool JUMP;

	// Use this for initialization
	void Start ()
	{
		
	}

	public bool Left
	{
		get
		{
			return LEFT;
		}
		set
		{
			LEFT = value;
		}
	}

	public bool Right
	{
		get
		{
			return RIGHT;
		}
		set
		{
			RIGHT = value;
		}
	}

	public bool Jump
	{
		get
		{
			return JUMP;
		}
		set
		{
			JUMP = value;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		LEFT = Input.GetKey("left");
		RIGHT = Input.GetKey("right");
		JUMP = Input.GetKey("space");
	}
}

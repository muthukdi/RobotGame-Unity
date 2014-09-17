using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AIPFramework;

public class RobotController : AIPEventListener
//public class RobotController : MonoBehaviour
{
	private bool LEFT;
	private bool RIGHT;
	private bool JUMP;
	public string clientID;

	// Use this for initialization
	void Start ()
	{

	}

	//Respond to events received by the client
	public override void eventMessage(string name, string data, string clientId)
	{
		Debug.Log ("clientId = " + clientId + ", clientID = " + clientID);
		//Only respond to events from this clientID!
		if (clientId.Equals(clientID))
		{
			Debug.Log ("Event received: " + name);
			if (name == "leftevent") 
			{
				LEFT = true;
			}
			if (name == "rightevent")
			{
				RIGHT = true;
			}
			if (name == "jumpevent")
			{
				JUMP = true;
			}
		}
	}

	public IEnumerator ResetJumpAfter(float time)
	{
		yield return new WaitForSeconds(time);
		JUMP = false;
	}

	public bool Left
	{
		get
		{
			return LEFT;
		}
	}
	
	public bool Right
	{
		get
		{
			return RIGHT;
		}
	}
	
	public bool Jump
	{
		get
		{
			return JUMP;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*LEFT = Input.GetKey("left");
		RIGHT = Input.GetKey("right");
		JUMP = Input.GetKey("space");*/
		StartCoroutine(ResetJumpAfter(0.1f));
	}
}

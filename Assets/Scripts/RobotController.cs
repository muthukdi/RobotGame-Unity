using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AIPFramework;
using SimpleJson;

public class RobotController : AIPEventListener
//public class RobotController : MonoBehaviour
{
	private bool LEFT;
	private bool RIGHT;
	private bool JUMP;
	public string clientID;
	public AIPNetwork myNetwork;

	// Use this for initialization
	void Start ()
	{
		myNetwork = new AIPNetwork(); 
		myNetwork.Connect();
		Debug.Log ("myNetwork.Mobicode: " + myNetwork.Mobicode);
	}

	//Respond to events received by the client
	/*public override void eventMessage(string name, string data, string clientId)
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
	}*/

	//Display the contents of any messages received from clients
	public override void eventMessage (string name, string data, string clientId)
	{
		try
		{
			bool left = false;
			bool right = false;
			bool jump = false;
			if (name == "ControllerEvent") 
			{
				JsonObject json = SimpleJson.SimpleJson.DeserializeObject (data) as JsonObject;
				left = System.Convert.ToBoolean (json ["left"]);
				right = System.Convert.ToBoolean (json ["right"]);
				jump = System.Convert.ToBoolean (json ["jump"]);
				if (left != LEFT || right != RIGHT || jump != JUMP)
				{
					Debug.Log(data);
				}
				lock (this)
				{
					LEFT = left;
					RIGHT = right;
					JUMP = jump;
				}
			} 
	
		}
		catch (System.Exception ex)
		{
			Debug.Log (ex.Source);
			Debug.Log (ex.Message);
			Debug.Log (ex.StackTrace);
		}
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
	}
}

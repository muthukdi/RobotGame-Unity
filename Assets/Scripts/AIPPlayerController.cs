using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AIPFramework;
using SimpleJson;

public class AIPPlayerController : AIPEventListener
{
	private bool LEFT;
	private bool RIGHT;
	private bool JUMP;
	public string clientID;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	//Respond to input events from HTML/JavaScript client controller
	public override void eventMessage (string name, string data, string clientId)
	{
		if (clientId.Equals(clientID)) 
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
						//Debug.Log(data);
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

	}
}
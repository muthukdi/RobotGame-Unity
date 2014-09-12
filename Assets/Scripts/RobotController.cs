using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using AIPFramework;

//public class RobotController : AIPEventListener
public class RobotController : MonoBehaviour
{
	private bool LEFT;
	private bool RIGHT;
	private bool JUMP;
	//public AIPNetwork myNetwork;
	//public string currentMessage = "No message yet";

	// Use this for initialization
	/*void Start ()
	{
		//Create custom network object "myNetwork", connect
		myNetwork = new AIPNetwork(); 
		myNetwork.Connect();
		
		//View the exposed values in AIPNetwork 
		//Note that myNetwork.isConnected may return a false value as the 
		//connection occurs in a separate thread and is time dependant 
		
		Debug.Log ("myNetwork.Mobicode: " + myNetwork.Mobicode); 
		Debug.Log ("myNetwork.isConnected: " + myNetwork.isConnected);
	}*/

	//Display the contents of any messages received from clients
	/*public override void eventMessage(string name, string data, string clientId)
	{
		Debug.Log ("We received a message from a client: " + data); 
		Debug.Log("Its name is: "+ name);
		Debug.Log("It came from: "+ clientId); 
		
		//If a client has sent an event, respond to that event
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
	}*/

	/*public IEnumerator ResetJumpAfter(float time)
	{
		yield return new WaitForSeconds(time);
		JUMP = false;
	}*/
	
	/*void OnGUI()
	{
		//Display the Mobicode and most recent message to the user
		GUI.Box(new Rect(5,5,400,40),"Mobicode: " + myNetwork.Mobicode); 
		GUI.Box(new Rect(5,50,400,40), currentMessage); 		
	}*/

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
		LEFT = Input.GetKey("left");
		RIGHT = Input.GetKey("right");
		JUMP = Input.GetKey("space");
		//StartCoroutine(ResetJumpAfter(0.1f));
	}
}

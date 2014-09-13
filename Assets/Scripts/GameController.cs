using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AIPFramework;

public class GameController : AIPEventListener
{
	public Transform robot;
	private string[] clientIDs;
	private int numOfClients;
	public string currentMessage = "No message yet";
	public AIPNetwork myNetwork;
	private Queue<string> queue;

	// Use this for initialization
	void Start ()
	{
		//Create custom network object "myNetwork", connect
		myNetwork = new AIPNetwork(); 
		myNetwork.Connect();
		
		//View the exposed values in AIPNetwork 
		//Note that myNetwork.isConnected may return a false value as the 
		//connection occurs in a separate thread and is time dependant 
		
		Debug.Log ("myNetwork.Mobicode: " + myNetwork.Mobicode); 
		Debug.Log ("myNetwork.isConnected: " + myNetwork.isConnected);
		clientIDs = new string[20];
		numOfClients = 0;

		queue = new Queue<string>();
	}

	//Display the contents of any messages received from clients
	public override void eventMessage(string name, string data, string clientId)
	{
		// Check if this client already exists
		bool clientExists = false;
		for (int i = 0; i < numOfClients; i++)
		{
			if (clientId == clientIDs[i])
			{
				clientExists = true;
				break;
			}
		}
		if (!clientExists)
		{
			Debug.Log ("We received a message from a client: " + data); 
			Debug.Log("Its name is: "+ name);
			Debug.Log("It came from: "+ clientId);
			lock(this)
			{
				queue.Enqueue(clientId);
			}
		}
	}

	void OnGUI()
	{
		//Display the Mobicode and most recent message to the user
		GUI.Box(new Rect(5,5,400,40),"Mobicode: " + myNetwork.Mobicode); 
		GUI.Box(new Rect(5,50,400,40), currentMessage); 		
	}
	
	// Update is called once per frame
	void Update ()
	{
		lock(this)
		{
			if (queue.Count > 0)
			{
				string clientId = queue.Dequeue();
				float x = -2f + (float)numOfClients/2f;
				float y = 1.64f;
				RobotController clone = Instantiate(robot, new Vector3(x, y, 0), Quaternion.identity) as RobotController;
				clone.ClientID = clientId;
				clientIDs[numOfClients] = clientId;
				numOfClients++;
			}
		}
	}
}

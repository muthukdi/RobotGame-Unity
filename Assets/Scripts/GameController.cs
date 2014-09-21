using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public Transform robot;
	public Transform crawler;
	
	// Use this for initialization
	void Start ()
	{
		Instantiate(robot);
		for (int i = 0; i < 3; i++)
		{
			Instantiate(crawler);
		}

	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public Transform robot;
	
	// Use this for initialization
	void Start ()
	{
		Instantiate(robot);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
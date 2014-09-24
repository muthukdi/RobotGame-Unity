using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	float timeToGo;
	// Use this for initialization
	void Start ()
	{
		timeToGo = Time.fixedTime + 0.075f;
		transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.fixedTime > timeToGo)
		{
			timeToGo = Time.fixedTime + 0.1f;
			//transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
		}
	}
}

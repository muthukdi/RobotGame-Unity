using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	float timeToGo;
	// Use this for initialization
	void Start ()
	{
		timeToGo = Time.fixedTime + 0.075f;
		//transform.position = new Vector3(transform.position.x, transform.position.y + 7.5f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// To slow down the update rate
		if (Time.fixedTime > timeToGo)
		{
			timeToGo = Time.fixedTime + 0.1f;
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
			camera.backgroundColor = new Color(camera.backgroundColor.r - 0.001f, 
			                                   camera.backgroundColor.g - 0.001f, 
			                                   camera.backgroundColor.b - 0.001f,
			                                   camera.backgroundColor.a);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
	public AudioClip coinSound;
	// Use this for initialization
	void Start ()
	{
	
	}

	// This coin has been collected by the robot
	void OnTriggerEnter2D(Collider2D target)
	{
		// If this script is not enabled
		if (!enabled)
		{
			return;
		}
		if (target.gameObject.tag == "Player")
		{
			if (coinSound)
			{
				AudioSource.PlayClipAtPoint(coinSound, transform.position);
				Destroy(gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

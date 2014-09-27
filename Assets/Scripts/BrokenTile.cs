using UnityEngine;
using System.Collections;

public class BrokenTile : MonoBehaviour
{

	private Animator animator;
	private float timeToDeath;
	public AudioClip explodeSound;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		animator.SetInteger("AnimState", 0); //static
		timeToDeath = 0.0f;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			Animator robotAnimator = coll.gameObject.GetComponent<Animator>();
			// If the robot is jumping
			if (robotAnimator.GetInteger("AnimState") == 2)
			{
				rigidbody2D.isKinematic = false;
				rigidbody2D.AddForce(new Vector2(0.0f, 70.0f));
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		switch (animator.GetInteger("AnimState"))
		{
			case 0: //static
			{
				// Tile is on its way down
				if (rigidbody2D.velocity.y < 0.0)
				{
					animator.SetInteger("AnimState", 1); //explode
					rigidbody2D.isKinematic = true;
					collider2D.enabled = false;
					timeToDeath = Time.time + 1.5f;
					if (explodeSound)
					{
						AudioSource.PlayClipAtPoint(explodeSound, transform.position);
					}
				}
				break;
			}
			case 1: //exploding
			{
				if (Time.time > timeToDeath)
				{
					Destroy(gameObject);
				}
				break;
			}
			default:
			{
				//This shouldn't happen
				break;
			}
		}
	}
}

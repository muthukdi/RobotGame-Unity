using UnityEngine;
using System.Collections;

public class Crawler : MonoBehaviour
{
	private Animator animator;
	private float walkingForce;
	private float brakeForce;
	private Vector2 maxVelocity;
	private int direction;
	private float nextThinkTime;
	private float timeToDeath;
	private float velocityX;
	private float[] walkingSpeeds;
	
	// Use this for initialization
	void Start ()
	{
		walkingForce = 10.0f;
		brakeForce = 25.0f;
		animator = GetComponent<Animator>();
		animator.SetInteger("AnimState", 0); //idle
		animator.speed = 1.0f;
		timeToDeath = 0.0f;
		nextThinkTime = Time.time + 3.0f;
		walkingSpeeds = new float[3];
		walkingSpeeds[0] = 0.3f;
		walkingSpeeds[1] = 0.6f;
		walkingSpeeds[2] = 0.9f;
		maxVelocity = new Vector2(walkingSpeeds[Random.Range(0, 3)], 0.0f);
		direction = Random.Range(0, 2) == 1 ? 1 : -1;
		transform.localScale = direction < 0 ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
	}

	// collision callback
	void OnCollisionEnter2D(Collision2D coll)
	{
		// If this script is not enabled
		if (!enabled)
		{
			return;
		}
		if (coll.gameObject.tag == "wall")
		{
			// Face the opposite direction
			transform.localScale = velocityX > 0 ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
			direction = velocityX > 0 ? -1 : 1;
		}
		else if (coll.gameObject.tag == "crawler")
		{
			// Ignore all future collisions with this crawler
			Physics2D.IgnoreCollision(coll.collider, collider2D);
		}
	}

	public float TimeToDeath
	{
		get
		{
			return timeToDeath;
		}
		set
		{
			timeToDeath = value;
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float forceX = 0.0f;
		float forceY = 0.0f;
		float absVelX = Mathf.Abs(rigidbody2D.velocity.x);
		velocityX = rigidbody2D.velocity.x;
		// Change behavior according to the current animation state
		switch (animator.GetInteger("AnimState"))
		{
			case 0: //idle
			{
				animator.speed = 1.0f;
				if (Time.time > nextThinkTime)
				{
					animator.SetInteger("AnimState", 1); //walking
					nextThinkTime = Time.time + Random.Range(2.0f, 4.0f);
					// Change the walking speed randomly
					maxVelocity = new Vector2(walkingSpeeds[Random.Range(0, 3)], 0.0f);
					animator.speed = maxVelocity.x/0.3f;
				}
				break;
			}
			case 1: //walking
			{
				if (Time.time > nextThinkTime)
				{
					animator.SetInteger("AnimState", 0); //idle
					nextThinkTime = Time.time + Random.Range(2.0f, 4.0f);
					//Brake the momentum
					if (velocityX < 0.0f)
					{
						forceX = brakeForce * absVelX;
					}
					else if (velocityX > 0.0f)
					{
						forceX = -brakeForce * absVelX;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
					break;
				}
				// Move the crawler in the current direction
				if (direction < 0)
				{
					if (absVelX < maxVelocity.x)
					{
						forceX = -walkingForce;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				else
				{
					if (absVelX < maxVelocity.x)
					{
						forceX = walkingForce;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				break;
			}
			case 2: //dying
			{
				animator.speed = 1.0f;
				if (Time.time > timeToDeath)
				{
					Destroy(gameObject);
				}
				break;
			}
			default:
			{
				//Shouldn't happen
				break;
			}
		}
	}
}

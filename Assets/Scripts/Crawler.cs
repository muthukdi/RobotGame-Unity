using UnityEngine;
using System.Collections;

public class Crawler : MonoBehaviour
{
	private Animator animator;
	private float walkingSpeed;
	private float brakeSpeed;
	private Vector2 maxVelocity;
	private int direction;
	private float walkingSpeedScale;
	private float nextThinkTime;
	private float timeToDeath;
	private float velocityX;
	
	// Use this for initialization
	void Start ()
	{
		walkingSpeed = 5.0f;
		brakeSpeed = 25.0f;
		maxVelocity = new Vector2(1.0f, 0.0f);
		animator = GetComponent<Animator>();
		animator.SetInteger("AnimState", 0); //idle
		animator.speed = 1.0f;
		walkingSpeedScale = 1.0f;
		direction = -1;
		timeToDeath = 0.0f;
		nextThinkTime = Time.time + 3.0f;
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
			Debug.Log("crawler velocity = " + velocityX);
			transform.localScale = velocityX > 0 ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
			direction = velocityX > 0 ? -1 : 1;
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
				if (Time.time > nextThinkTime)
				{
					animator.SetInteger("AnimState", 1);
					nextThinkTime = Time.time + 3.0f;
				}
				break;
			}
			case 1: //walking
			{
				if (Time.time > nextThinkTime)
				{
					animator.SetInteger("AnimState", 0); //idle
					nextThinkTime = Time.time + 3.0f;
					//Brake the momentum
					if (velocityX < 0.0f)
					{
						forceX = brakeSpeed * absVelX;
					}
					else if (velocityX > 0.0f)
					{
						forceX = -brakeSpeed * absVelX;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
					break;
				}
				// Move the crawler in the current direction
				if (direction < 0)
				{
					if (absVelX < maxVelocity.x)
					{
						forceX = -walkingSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				else
				{
					if (absVelX < maxVelocity.x)
					{
						forceX = walkingSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				break;
			}
			case 2: //dying
			{
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

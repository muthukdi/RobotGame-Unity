using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour
{

	private Animator animator;
	private bool jumpEnabled;
	private float runningSpeed;
	private float jumpingSpeed;
	private float brakeSpeed;
	private Vector2 maxVelocity;
	public AudioClip jumpSound;

	// Use this for initialization
	void Start ()
	{
		runningSpeed = 20.0f;
		jumpingSpeed = 450.0f;
		brakeSpeed = 50.0f;
		maxVelocity = new Vector2(5.0f, 200.0f);
		animator = GetComponent<Animator>();
		jumpEnabled = true;
	}

	// collision callback
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "floor")
		{
			animator.SetInteger("AnimState", 0); //idle
		}
	}

	// Update is called once per frame
	void Update()
	{
		float forceX = 0.0f;
		float forceY = 0.0f;
		float absVelX = Mathf.Abs(rigidbody2D.velocity.x);
		float absVelY = Mathf.Abs(rigidbody2D.velocity.y);
		// Change behavior according to the current animation state
		switch (animator.GetInteger("AnimState"))
		{
			case 0: //idle
			{
				if (Input.GetKey("left") || Input.GetKey("right"))
				{
					animator.SetInteger("AnimState", 1); //running
					// Determine direction of motion and move the robot
					if (Input.GetKey("left"))
					{
						transform.localScale = new Vector3(-1.835181f, 1.835183f, 1.0f);
						if (absVelX < maxVelocity.x)
						{
							forceX = -runningSpeed;
						}
					}
					if (Input.GetKey("right"))
					{
						transform.localScale = new Vector3(1.835181f, 1.835183f, 1.0f);
						if (absVelX < maxVelocity.x)
						{
							forceX = runningSpeed;
						}
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				// Don't allow repeated jumps (disable it until
				// the button is released)
				if (Input.GetKey("space") && jumpEnabled)
				{
					animator.SetInteger("AnimState", 2); //jumping
					if (absVelY < maxVelocity.y)
					{
						forceY = jumpingSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
					if (jumpSound)
					{
						AudioSource.PlayClipAtPoint(jumpSound, transform.position);
					}
					jumpEnabled = false;
				}
				else if (!Input.GetKey("space"))
				{
					jumpEnabled = true;
				}
				break;
			}
			case 1: //running
			{
				if (!(Input.GetKey("left") || Input.GetKey("right")))
				{
					animator.SetInteger("AnimState", 0); //idle
					//Brake the momentum
					if (rigidbody2D.velocity.x < 0.0f)
					{
						forceX = brakeSpeed * absVelX;
					}
					else if (rigidbody2D.velocity.x > 0.0f)
					{
						forceX = -brakeSpeed * absVelX;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
					break;
				}
				// Don't allow repeated jumps (disable it until
				// the button is released)
				if (Input.GetKey("space") && jumpEnabled)
				{
					animator.SetInteger("AnimState", 2); //jumping
					if (absVelY < maxVelocity.y)
					{
						forceY = jumpingSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
					if (jumpSound)
					{
						AudioSource.PlayClipAtPoint(jumpSound, transform.position);
					}
					jumpEnabled = false;
					break;
				}
				else if (!Input.GetKey("space"))
				{
					jumpEnabled = true;
				}
				// Determine direction of motion and move the robot
				if (Input.GetKey("left"))
				{
					transform.localScale = new Vector3(-1.835181f, 1.835183f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = -runningSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				if (Input.GetKey("right"))
				{
					transform.localScale = new Vector3(1.835181f, 1.835183f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = runningSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				break;
			}
			case 2: //jumping
			{
				// Determine direction of motion and move the robot
				if (Input.GetKey("left"))
				{
					transform.localScale = new Vector3(-1.835181f, 1.835183f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = -runningSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				if (Input.GetKey("right"))
				{
					transform.localScale = new Vector3(1.835181f, 1.835183f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = runningSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
			break;
		}
		case 3: //falling
			{
				break;
			}
			default:
				// Shouldn't happen
				break;
		}
	}
	
}

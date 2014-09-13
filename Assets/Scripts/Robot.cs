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
	public AudioClip blockSound;
	public float airDragCoefficient;
	private float velocityY;
	private RobotController controller;

	// Use this for initialization
	void Start ()
	{
		runningSpeed = 10.0f;
		jumpingSpeed = 250.0f;
		brakeSpeed = 25.0f;
		maxVelocity = new Vector2(2.5f, 100.0f);
		animator = GetComponent<Animator>();
		controller = GetComponent<RobotController>();
		jumpEnabled = true;
		animator.SetInteger("AnimState", 0); //idle
		airDragCoefficient = 0.5f;
	}

	// collision callback
	void OnCollisionEnter2D(Collision2D coll)
	{
		// When the robot collides with a floor
		if (coll.gameObject.tag == "floor")
		{
			// Falling
			if (animator.GetInteger("AnimState") == 3)
			{
				animator.SetInteger("AnimState", 0); //idle
			}
			// Jumping
			else if (animator.GetInteger("AnimState") == 2)
			{
				if (blockSound)
				{
					AudioSource.PlayClipAtPoint(blockSound, transform.position);
				}
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float forceX = 0.0f;
		float forceY = 0.0f;
		float absVelX = Mathf.Abs(rigidbody2D.velocity.x);
		float absVelY = Mathf.Abs(rigidbody2D.velocity.y);
		velocityY = rigidbody2D.velocity.y;
		// Change behavior according to the current animation state
		switch (animator.GetInteger("AnimState"))
		{
			case 0: //idle
			{
				//Debug.Log("Idle");
				// Probably falling off the edge of a tile
				if (velocityY < -0.5)
				{
					animator.SetInteger("AnimState", 3); //falling
					break;
				}
				if (controller.Left || controller.Right)
				{
					animator.SetInteger("AnimState", 1); //running
					// Determine direction of motion and move the robot
					if (controller.Left)
					{
						transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
						if (absVelX < maxVelocity.x)
						{
							forceX = -runningSpeed;
						}
					}
					if (controller.Right)
					{
						transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
						if (absVelX < maxVelocity.x)
						{
							forceX = runningSpeed;
						}
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				// Don't allow repeated jumps (disable it until
				// the button is released)
				if (controller.Jump && jumpEnabled)
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
				else if (!controller.Jump)
				{
					jumpEnabled = true;
				}
				break;
			}
			case 1: //running
			{
				//Debug.Log("Running");
				// Probably falling off the edge of a tile
				if (velocityY < -0.5)
				{
					animator.SetInteger("AnimState", 3); //falling
					break;
				}
				if (!(controller.Left || controller.Right))
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
				if (controller.Jump && jumpEnabled)
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
				else if (!controller.Jump)
				{
					jumpEnabled = true;
				}
				// Determine direction of motion and move the robot
				if (controller.Left)
				{
					transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = -runningSpeed;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				if (controller.Right)
				{
					transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
				//Debug.Log("Jumping");
				// Has reached the top of a jump
				if (velocityY < -0.5)
				{
					Debug.Log("Just switched to falling state");
					animator.SetInteger("AnimState", 3); //falling
				}
				// Determine direction of motion and move the robot
				if (controller.Left)
				{
					transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = -runningSpeed * airDragCoefficient;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				if (controller.Right)
				{
					transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = runningSpeed * airDragCoefficient;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
			break;
		}
		case 3: //falling
			{
				//Debug.Log("Falling");
				// Determine direction of motion and move the robot
				if (controller.Left)
				{
					transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = -runningSpeed * airDragCoefficient;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				if (controller.Right)
				{
					transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					if (absVelX < maxVelocity.x)
					{
						forceX = runningSpeed * airDragCoefficient;
					}
					rigidbody2D.AddForce(new Vector2(forceX, forceY));
				}
				break;
			}
			default:
				// Shouldn't happen
				break;
		}
	}
	
}

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

	// Use this for initialization
	void Start ()
	{
		runningSpeed = 10.0f;
		jumpingSpeed = 1000.0f;
		brakeSpeed = 200.0f;
		maxVelocity = new Vector2(5.0f, 200.0f);
		animator = GetComponent<Animator>();
		jumpEnabled = true;
	}
	
	// Update is called once per frame
	/*void Update ()
	{
		float forceX = 0.0f;
		float forceY = 0.0f;
		var absVelX = Mathf.Abs(rigidbody2D.velocity.x);
		var absVelY = Mathf.Abs(rigidbody2D.velocity.y);
		if (Input.GetKey("right"))
		{
			if (absVelX < maxVelocity.x)
			{
				forceX = speed;
			}
			transform.localScale = new Vector3(1.835181f, 1.835183f, 1.0f);
			animator.SetInteger("AnimState", 1);
		} 
		else if (Input.GetKey("left"))
		{
			if (absVelX < maxVelocity.x)
			{
				forceX = -speed;
			}
			transform.localScale = new Vector3(-1.835181f, 1.835183f, 1.0f);
			animator.SetInteger("AnimState", 1);
		}
		else
		{
			animator.SetInteger("AnimState", 0);
		}
		rigidbody2D.AddForce(new Vector2(forceX, forceY));
	}*/

	// Update is called once per frame
	void Update()
	{
		float forceX = 0.0f;
		float forceY = 0.0f;
		float absVelX = Mathf.Abs(rigidbody2D.velocity.x);
		float absVelY = Mathf.Abs(rigidbody2D.velocity.y);
		switch (animator.GetInteger("AnimState"))
		{
			case 0: //idle
			{
				if (Input.GetKey("left") || Input.GetKey("right"))
				{
					animator.SetInteger("AnimState", 1); //running
					if (Input.GetKey("left"))
					{
						//flip direction
						transform.localScale = new Vector3(-1.835181f, 1.835183f, 1.0f);
					}
					if (Input.GetKey("right"))
					{
						//flip direction
						transform.localScale = new Vector3(1.835181f, 1.835183f, 1.0f);
					}
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
					//[[OALSimpleAudio sharedInstance] playEffect:@"jump_sound.wav"];
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
						forceX = brakeSpeed;
					}
					else if (rigidbody2D.velocity.x > 0.0f)
					{
						forceX = -brakeSpeed;
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
					//[[OALSimpleAudio sharedInstance] playEffect:@"jump_sound.wav"];
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

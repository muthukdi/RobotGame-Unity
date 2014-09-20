using UnityEngine;
using System.Collections;

public class AIPRobot : MonoBehaviour
{
	private Animator animator;
	private float runningSpeed;
	private float jumpingSpeed;
	private float brakeSpeed;
	private Vector2 maxVelocity;
	public AudioClip jumpSound;
	public AudioClip blockSound;
	public AudioClip stompSound;
	public AudioClip dieSound;
	private float airDragCoefficient;
	private float velocityY;
	private AIPPlayerController controller;
	private float bouncingSpeed;
	private float fadeOutTime = 0.0f;
	private Color startColor, endColor;
	public Transform robot;
	
	// Use this for initialization
	void Start ()
	{
		runningSpeed = 10.0f;
		jumpingSpeed = 250.0f;
		brakeSpeed = 25.0f;
		bouncingSpeed = 100.0f;
		maxVelocity = new Vector2(2.5f, 100.0f);
		animator = GetComponent<Animator>();
		controller = GetComponent<AIPPlayerController>();
		animator.SetInteger("AnimState", 0); //idle
		airDragCoefficient = 0.5f;
		startColor = renderer.material.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
	}
	
	// collision callback
	void OnCollisionEnter2D(Collision2D coll)
	{
		// If this script is not enabled
		if (!enabled)
		{
			return;
		}
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
		else if (coll.gameObject.tag == "crawler")
		{
			float absVelY = Mathf.Abs(rigidbody2D.velocity.y);
			float forceX = 0.0f;
			float forceY = 0.0f;
			// If the robot is falling onto the crawler, the crawler dies
			if (animator.GetInteger("AnimState") == 3)
			{
				// Give it a little bounce
				animator.SetInteger("AnimState", 2);
				if (absVelY < maxVelocity.y)
				{
					forceY = bouncingSpeed;
				}
				rigidbody2D.AddForce(new Vector2(forceX, forceY));
				// Get the crawler's animator and change its state to dying
				Animator crawlerAnimator = coll.gameObject.GetComponent<Animator>();
				crawlerAnimator.SetInteger("AnimState", 2);
				if (stompSound)
				{
					AudioSource.PlayClipAtPoint(stompSound, transform.position);
				}
				// Set the crawler's time to death
				coll.gameObject.GetComponent<Crawler>().TimeToDeath = Time.time + 0.5f;
				// Disabe the crawler's physics components so that it can no longer
				// interact with the world.
				coll.collider.enabled = false;
				coll.rigidbody.isKinematic = true;
			}
			// If the robot bumps into the crawler while its idle or running, the robot dies
			else if (animator.GetInteger("AnimState") < 2)
			{
				// Give it a little bounce
				animator.SetInteger("AnimState", 4);
				if (absVelY < maxVelocity.y)
				{
					forceY = bouncingSpeed;
				}
				rigidbody2D.AddForce(new Vector2(forceX, forceY));
				// Push the crawler in the opposite direction to null the impact force
				forceX = -50.0f * rigidbody2D.velocity.x;
				forceY = 0.0f;
				coll.rigidbody.AddForce(new Vector2(forceX, forceY));
				if (dieSound)
				{
					AudioSource.PlayClipAtPoint(dieSound, transform.position);
				}
				// Disable the robot's physics components
				collider2D.enabled = false;
				rigidbody2D.isKinematic = true;
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
				// Repeated jumps are allowed here for smoother network response
				if (controller.Jump)
				{
					Debug.Log("Jump pressed");
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
				// Allow repeated jumps for smoother network response
				if (controller.Jump)
				{
					Debug.Log("Jump pressed");
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
					break;
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
			case 4: //dying
			{
				//Debug.Log("Dying");
				fadeOutTime += Time.deltaTime;
				renderer.material.color = Color.Lerp(startColor, endColor, fadeOutTime/2);
				if (renderer.material.color.a <= 0.0f)
				{
					string clientId = controller.clientID;
					float x = -2.0f;
					float y = 1.64f;
					Transform clone = Instantiate(robot, new Vector3(x, y, 0), Quaternion.identity) as Transform;
					AIPPlayerController clone_controller = clone.GetComponent<AIPPlayerController>();
					clone_controller.clientID = clientId;
					Destroy(gameObject);
				}
				break;
			}
			default:
				// Shouldn't happen
				break;
		}
	}
	
}

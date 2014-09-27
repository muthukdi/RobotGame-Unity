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
	public AudioClip stompSound;
	public AudioClip dieSound;
	private float airDragCoefficient;
	private float velocityY;
	private PlayerController controller;
	private float bouncingSpeed;
	private float fadeOutTime = 0.0f;
	private Color startColor, endColor;
	private ArrayList spawnPoints;

	// Use this for initialization
	void Start()
	{
		runningSpeed = 10.0f;
		jumpingSpeed = 260.0f;
		brakeSpeed = 25.0f;
		bouncingSpeed = 100.0f;
		maxVelocity = new Vector2(2.5f, 50.0f);
		animator = GetComponent<Animator>();
		controller = GetComponent<PlayerController>();
		jumpEnabled = true;
		animator.SetInteger("AnimState", 0); //idle
		airDragCoefficient = 0.5f;
		startColor = renderer.material.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
		createSpawnPoints();
	}

	// collision callback
	void OnCollisionEnter2D(Collision2D coll)
	{
		// If this script is not enabled
		if (!enabled)
		{
			return;
		}
		// Get the normal vector of the contact point
		Vector2 normal = coll.contacts[0].normal;
		// When the robot collides with a floor
		if (coll.gameObject.tag == "floor")
		{
			// Falling
			if (animator.GetInteger("AnimState") == 3)
			{
				if (Mathf.Round(normal.y) == 1)
				{
					animator.SetInteger("AnimState", 0); //idle
				}
			}
			// Jumping
			else if (animator.GetInteger("AnimState") == 2)
			{
				if (Mathf.Round(normal.y) == -1)
				{
					if (blockSound)
					{
						AudioSource.PlayClipAtPoint(blockSound, transform.position);
					}
				}
				// Hopefully, this fixes the bug where the robot gets onto
				// a platform while it is still in the jumping state!
				else if (Mathf.Round(normal.y) == 1)
				{
 					animator.SetInteger("AnimState", 0); //idle
				}
			}
		}
		else if (coll.gameObject.tag == "crawler")
		{
			float forceX = 0.0f;
			float forceY = 0.0f;
			// If the robot is falling onto the crawler
			if (animator.GetInteger("AnimState") == 3)
			{
				// The crawler must die in this case
				// Give it a little bounce
				animator.SetInteger("AnimState", 2);
				forceY = bouncingSpeed;
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
			// If the robot bumps into the crawler while its jumping, the robot dies
			else if (animator.GetInteger("AnimState") == 2)
			{
				if (Mathf.Round(normal.y) == -1 || Mathf.Round(normal.x) == -1 || Mathf.Round(normal.x) == 1)
				{
					// Push the crawler in the opposite direction to null the impact force
					forceX = 0.0f;
					forceY = -50.0f * rigidbody2D.velocity.x;
					coll.rigidbody.AddForce(new Vector2(forceX, forceY));
					PrepareToDie();
				}
			}
			// If the robot bumps into the crawler while its running, the robot dies
			else if (animator.GetInteger("AnimState") == 1)
			{
				if (Mathf.Round(normal.x) == -1 || Mathf.Round(normal.x) == 1)
				{
					// Push the crawler in the opposite direction to null the impact force
					forceX = -50.0f * rigidbody2D.velocity.x;
					forceY = 0.0f;
					coll.rigidbody.AddForce(new Vector2(forceX, forceY));
					PrepareToDie();
				}
			}
			// If the robot bumps into the crawler while its idle, the robot dies
			else if (animator.GetInteger("AnimState") == 0)
			{
					PrepareToDie();
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
		// Kill the robot if it's head goes below the camera view
		if (animator.GetInteger ("AnimState") != 4) 
		{
			float bottom = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
			if (transform.position.y < bottom)
			{
				PrepareToDie();
			}
		}
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
					// We need to choose a spawn point within upper half of the camera view
					Vector3 spawnPoint = Vector3.zero;
					float top = Camera.main.ViewportToWorldPoint(Vector3.up).y;
					float middle = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0.0f)).y;
					foreach (Vector3 vector in spawnPoints)
					{
						if (vector.y < top && vector.y > middle)
						{
							spawnPoint = vector;
							break;
						}
					}
					transform.position = spawnPoint;
					// Reset the state back to idle
					animator.SetInteger("AnimState", 0); //idle
					// Re-enable the robot's physics components
					collider2D.enabled = true;
					rigidbody2D.isKinematic = false;
					// Make the robot visible again
					renderer.material.color = new Color(startColor.r, startColor.g, startColor.b, startColor.a);
					// Reset the fade out time
					fadeOutTime = 0.0f;
				}
				break;
			}
			default:
				// Shouldn't happen
				break;
		}
	}

	void PrepareToDie()
	{
		animator.SetInteger("AnimState", 4);
		// Give it a little bounce
		rigidbody2D.AddForce(new Vector2(0.0f, bouncingSpeed));
		if (dieSound)
		{
			AudioSource.PlayClipAtPoint(dieSound, transform.position);
		}
		// Disable the robot's physics components
		collider2D.enabled = false;
		rigidbody2D.isKinematic = true;
	}

	void createSpawnPoints()
	{
		spawnPoints = new ArrayList();
		spawnPoints.Add(new Vector3(-3.599096f, 1.594519f, 0.0f));
		spawnPoints.Add(new Vector3(-3.599096f, 4.041722f, 0.0f));
		spawnPoints.Add(new Vector3(-1.564676f, 5.426082f, 0.0f));
		spawnPoints.Add(new Vector3(-3.599096f, 6.584653f, 0.0f));
		spawnPoints.Add(new Vector3(-1.564676f, 7.866363f, 0.0f));
		spawnPoints.Add(new Vector3(-0.318982f, 9.126759f, 0.0f));
		spawnPoints.Add(new Vector3(-3.599096f, 10.45236f, 0.0f));
		spawnPoints.Add(new Vector3(-3.599096f, 12.56025f, 0.0f));
	}
	
}

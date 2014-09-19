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
	//CCTime _nextThinkTime;
	//CCTime _timeToDeath;
	
	// Use this for initialization
	void Start ()
	{
		walkingSpeed = 5.0f;
		brakeSpeed = 25.0f;
		maxVelocity = new Vector2(1.0f, 0.0f);
		animator = GetComponent<Animator>();
		animator.SetInteger("AnimState", 0); //idle
		direction = -1;
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
			transform.localScale = direction > 0 ? new Vector3(-1.0f, 1.0f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);
			direction = -direction;
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float forceX = 0.0f;
		float absVelX = Mathf.Abs(rigidbody2D.velocity.x);
	}
}

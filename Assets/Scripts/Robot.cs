using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour
{

	public float speed;
	public Vector2 maxVelocity;
	private Animator animator;

	// Use this for initialization
	void Start ()
	{
		float speed = 10.0f;
		Vector2 maxVelocity = new Vector2(5.0f, 20.0f);
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var forceX = 0.0f;
		var forceY = 0.0f;
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
	}

}

using UnityEngine;
using System.Collections;

public class Player2 : Character 
{	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		spawnPos = _transform.position;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		UpdateMovement();
	}

	public void FixedUpdate()
	{
		// these are false unless one of keys is pressed
		currentInputState = inputState.None;
		
		// keyboard input
		if(Input.GetKey(KeyCode.LeftArrow)) 
		{ 
			currentInputState = inputState.WalkLeft;
			facingDir = facing.Left;
		}
		if (Input.GetKey(KeyCode.RightArrow) && currentInputState != inputState.WalkLeft) 
		{ 
			currentInputState = inputState.WalkRight;
			facingDir = facing.Right;
		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow)) 
		{ 
			currentInputState = inputState.Jump;
		}
		
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			currentInputState = inputState.Pass;
		}
		
		UpdatePhysics();
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ball") && hasBall == false)
		{
			PickUpBall();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ball") && hasBall == false)
		{
			PickUpBall();
		}
	}
	
	public void Respawn()
	{
		if(alive == true)
		{
			_transform.position = spawnPos;
			hasBall = false;
		}
	}
}

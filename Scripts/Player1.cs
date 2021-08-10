using UnityEngine;
using System.Collections;

public class Player1 : Character 
{	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		// grab the players position at startup and use it for the spawn position when starting new rounds
		spawnPos = _transform.position;
	}
	
	public void Update () 
	{
		// in case the ball ever gets stuck or is lost
		if(Input.GetKeyDown(KeyCode.R))
		{
			ResetBall();
		}

		// reload the scene to reset scores etc
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
		{
			Application.LoadLevel(0);
		}
		
		UpdateMovement();
	}

	public void FixedUpdate()
	{
		// inputstate is none unless one of the movement keys are pressed
		currentInputState = inputState.None;
		
		// move left
		if(Input.GetKey(KeyCode.A)) 
		{ 
			currentInputState = inputState.WalkLeft;
			facingDir = facing.Left;
		}

		// move right
		if (Input.GetKey(KeyCode.D) && currentInputState != inputState.WalkLeft) 
		{ 
			currentInputState = inputState.WalkRight;
			facingDir = facing.Right;
		}

		// jump
		if (Input.GetKeyDown(KeyCode.W)) 
		{ 
			currentInputState = inputState.Jump;
		}

		// pass the ball
		if(Input.GetKeyDown(KeyCode.S))
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
	
	public void Respawn()
	{
		if(alive == true)
		{
			_transform.position = spawnPos;
			hasBall = false;
		}
	}
}

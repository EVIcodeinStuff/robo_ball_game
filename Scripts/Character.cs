using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;

public class Character : MonoBehaviour 
{
	public MyTeam myTeam = MyTeam.Team1;

	public enum inputState 
	{ 
		None, 
		WalkLeft, 
		WalkRight, 
		Jump, 
		Pass
	}
	[HideInInspector] public inputState currentInputState;
	
	[HideInInspector] public enum facing { Right, Left }
	[HideInInspector] public facing facingDir;

	[HideInInspector] public bool alive = true;
	[HideInInspector] public Vector3 spawnPos;
	
	protected Transform _transform;
	protected Rigidbody2D _rigidbody;

	// edit these to tune character movement	
	private float runVel = 3.7f; 	// run speed when not carrying the ball
	private float walkVel = 3.2f; 	// walk speed while carrying ball
	private float stunVel = 0.3f;  //stunned movement speed
	private float jumpVel = 9.3f; 	// jump velocity
	//private float jump2Vel = 4.9f; 	// double jump velocity
	private float fallVel = 3.2f;		// fall velocity, gravity
	private float passVel = 4.2f;		// horizontal velocity of ball when passed

	private float moveVel;
	private float pVel = 0f;
	
	private int jumps = 0;
    private int maxJumps = 2; 		// set to 2 for double jump
		
	protected bool hasBall = false;
	
	private int stunTime;

	protected string team = "";

	// raycast stuff
	private RaycastHit2D hit;
	private Vector2 physVel = new Vector2();
	[HideInInspector] public bool grounded = false;
	private int groundMask = 1 << 8; // Ground layer mask

	public virtual void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Use this for initialization
	public virtual void Start () 
	{
		moveVel = walkVel;
	}
	
	// Update is called once per frame
	public virtual void UpdateMovement() 
	{
		if(xa.gameOver == true || alive == false) return;

		// if the other team took the ball from me, then remove it from my inventory
		if(myTeam == MyTeam.Team1 && xa.teamWithBall == xa.TeamWithBall.Team2)
		{
			RemoveBall();
		}

		if(myTeam == MyTeam.Team2 && xa.teamWithBall == xa.TeamWithBall.Team1)
		{
			RemoveBall();
		}

		// if I have then ball, then tell it to follow me
		if(hasBall == true)
		{
			xa.ball.UpdateBallFollowPos(_transform);
		}

		// teleport me to the other side of the screen when I reach the edge
		if(_transform.position.x > 9.85f)
		{
			_transform.position = new Vector3(-9.85f,_transform.position.y, 0);
		}
		if(_transform.position.x < -9.85f)
		{
			_transform.position = new Vector3(9.85f,_transform.position.y, 0);
		}
	}
	
	// ============================== FIXEDUPDATE ============================== 

	public virtual void UpdatePhysics()
	{
		if(xa.gameOver == true || alive == false) return;

		physVel = Vector2.zero;

		// move left
		if(currentInputState == inputState.WalkLeft)
		{
			physVel.x = -moveVel;
		}

		// move right
		if(currentInputState == inputState.WalkRight)
		{
			physVel.x = moveVel;
		}

		// jump
		if(currentInputState == inputState.Jump)
		{
			_rigidbody.velocity = new Vector2(physVel.x, jumpVel);
				
		}

		// pass the ball
		if(currentInputState == inputState.Pass && hasBall == true)// && _transform.childCount > 1)
		{
			if(facingDir == facing.Left)
				pVel = -passVel;
			else
				pVel = passVel;

			xa.ball.PassBall(pVel);
			RemoveBall();
		}

		// use raycasts to determine if the player is standing on the ground or not
		if (Physics2D.Raycast(new Vector2(_transform.position.x-0.1f,_transform.position.y), -Vector2.up, .26f, groundMask) 
		    || Physics2D.Raycast(new Vector2(_transform.position.x+0.1f,_transform.position.y), -Vector2.up, .26f, groundMask))
		{
			grounded = true;
			jumps = 0;
		}
		else
		{
			grounded = false;
			_rigidbody.AddForce(-Vector3.up * fallVel);
		}

		// actually move the player
		_rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y);
	}

	// ============================== BALL HANDLING ==============================
	
	public virtual void PickUpBall()
	{
		hasBall = true;
		moveVel = walkVel;
		
		if(myTeam == MyTeam.Team1)
		{
			team = "Team1";
			xa.teamWithBall = xa.TeamWithBall.Team1;
		}
		else if(myTeam == MyTeam.Team2)
		{
			team = "Team2";
			xa.teamWithBall = xa.TeamWithBall.Team2;
		}
		
		xa.ball.PickUp(_transform, team);

	}
	
	public void RemoveBall()
	{
		hasBall = false;
		stun (5); 
		while (stunTime > 0) {
			moveVel = stunVel;
		}
		moveVel = runVel;
	}

	public void stun(int seconds)
	{
		stunTime = seconds;
		InvokeRepeating ("stunCountDown", 0f, 1f);
	}

	void stunCountDown()
	{
		stunTime--;
		moveVel = stunVel;
		if (stunTime == 0) {
			CancelInvoke("stunCountDown");
		}	
	}
	
	// if the ball gets stuck, player1 can reset it by pressing a key
	public virtual void ResetBall()
	{
		StartCoroutine(xa.ball.SpawnBall());
	}

}

public enum MyTeam 
{
	Team1,
	Team2,
	None
}

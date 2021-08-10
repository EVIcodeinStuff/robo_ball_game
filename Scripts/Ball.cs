using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Collider2D _collider;
	
	private string team;

	private bool isBeingCarried = false;
	private bool isScoringPoints = false;
	
	private Color orange = new Color(0.91f,0.57f,0f);
	private Color blue = new Color(0.03f,0.68f,92f);
	private Color green = new Color(0.76f,1f,0f);

	private float offsetY = 0.5f;
	
	void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
	}
	
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(0.1f);
		ResetBall();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// screen wrap
		if(_transform.position.x > 9.85f && isBeingCarried == false)
		{
			_transform.position = new Vector3(-9.85f,_transform.position.y, 0);
		}
		if(_transform.position.x < -9.85f && isBeingCarried == false)
		{
			_transform.position = new Vector3(9.85f,_transform.position.y, 0);
		}
		
		if(isBeingCarried == true && isScoringPoints == false)
		{
			xa.scoreManager.DecreaseScore(team);
		}
	}
	
	public void PickUp(Transform trans, string tm)
	{
		team = tm;

		_transform.position = new Vector3(trans.position.x, trans.position.y + offsetY, trans.position.z);

		// play pickup sound
		xa.audioManager.PlayPickup();

		isBeingCarried = true;
		isScoringPoints = false;

	}

	// update position so that it's above the player's head when being carried
	public void UpdateBallFollowPos(Transform trans)
	{
		_transform.position = new Vector3(trans.position.x, trans.position.y +offsetY, trans.position.z);
	}

	public IEnumerator SpawnBall()
	{
		// move to spawn position
		_transform.position = new Vector3(0,-3.5f,0);
		_rigidbody.isKinematic = true;

		// allow the ball physics to calm down before turning physics on again
		yield return new WaitForSeconds(0.1f);
		_rigidbody.isKinematic = false;

		ResetBall();
	}

	public void PassBall(float velX)
	{
		// send the ball flying away from the player who threw it
		_rigidbody.velocity = new Vector3(velX, 5, 0);

		// play pass sound
		xa.audioManager.PlayPass();

		ResetBall();
	}

	void ResetBall()
	{
		// reset ball info
		xa.teamWithBall = xa.TeamWithBall.None;
		team = "None";
		isBeingCarried = false;
		isScoringPoints = false;

	}

	void IncreaseScore()
	{
		xa.scoreManager.IncreaseScore(team);
		isScoringPoints = true;
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		// ball is being held by team1 in their own goal
		if(other.gameObject.layer == xa.Team1Goal && team == "Team1" && xa.gameOver == false)
			IncreaseScore();

		// ball is being held by team2 in their own goal
		if(other.gameObject.layer == xa.Team2Goal && team == "Team2" && xa.gameOver == false)
			IncreaseScore();
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		// exited a goal so stop scoring points
		isScoringPoints = false;
	}
}

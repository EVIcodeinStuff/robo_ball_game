using UnityEngine;
using System.Collections;

public class xa : MonoBehaviour 
{
	// this script creates a bunch of static public variables that can be seen by all the other scripts in the game

	public static Ball ball;
	public static AudioManager audioManager;
	public static ScoreManager scoreManager;
	
	public static Player1 playerBlue;
	public static Player2 playerRed;
	
	public static bool gameOver = false;
	
	// layers
	public const int Team1Goal = 9;
	public const int Team2Goal = 10;

	public enum TeamWithBall
	{
		None,
		Team1,
		Team2
	}

	public static TeamWithBall teamWithBall = TeamWithBall.None;

	void Start()
	{
		// cache these so they can be accessed by other scripts
		ball = GameObject.FindWithTag("Ball").GetComponent<Ball>();
		scoreManager = gameObject.GetComponent<ScoreManager>();
		audioManager = gameObject.GetComponent<AudioManager>();
		playerBlue = GameObject.Find("PlayerBlue").GetComponent<Player1>();
		playerRed = GameObject.Find("PlayerRed").GetComponent<Player2>();
	}
}

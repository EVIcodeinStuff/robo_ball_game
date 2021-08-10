using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource audioSource; // camera
	
	public AudioClip progress;
	public AudioClip beep1;
	public AudioClip beep2;
	public AudioClip win;
	public AudioClip pickup;
	public AudioClip pass;
	
	public void PlayProgress() 
	{
		audioSource.PlayOneShot(progress, 1f);
	}
	
	public void PlayBeep1() 
	{
		audioSource.PlayOneShot(beep1, 1f);
	}
	
	public void PlayBeep2() 
	{
		audioSource.PlayOneShot(beep2, 1f);
	}
	
	public void PlayWin() 
	{
		audioSource.PlayOneShot(win, 1f);
	}
	
	public void PlayPickup() 
	{
		audioSource.PlayOneShot(pickup, 0.5f);
	}
	
	public void PlayPass() 
	{
		audioSource.PlayOneShot(pass, 1f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolThingToLookAt : MonoBehaviour {

	public static int pointValue = 10;
	private int pointsRemaining;
	private bool depleted;
	private AudioSource sparkle;
	private AudioSource leavesFall;
	public bool isBeingLookedAt;

	// Use this for initialization
	void Start () {
		pointsRemaining = pointValue;
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		sparkle = audioSources [0];
		leavesFall = audioSources [1];
		depleted = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LateUpdate(){
		if (isBeingLookedAt && !sparkle.isPlaying && !depleted) {
			sparkle.Play ();
		}
		if ((!isBeingLookedAt || depleted) && sparkle.isPlaying) {
			sparkle.Stop ();
		}
		isBeingLookedAt = false;
	}

	public bool DrainPoint(int playerNum){
		bool pointDrained = false;
		if (pointsRemaining > 0) {
			pointsRemaining -= 1;
			pointDrained = true;
			if (pointsRemaining == 0) {
				depleted = true;
				Destroy (transform.GetChild (4).gameObject);
				leavesFall.Play ();
				sparkle.Stop ();
			}
		} 
		return pointDrained;
	}
}

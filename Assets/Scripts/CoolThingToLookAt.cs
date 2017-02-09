using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolThingToLookAt : MonoBehaviour {

	public static int pointValue = 10;
	private int pointsRemaining;
	private bool depleted;

	// Use this for initialization
	void Start () {
		pointsRemaining = pointValue;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool DrainPoint(int playerNum){
		bool pointDrained = false;
		if (pointsRemaining > 0) {
			pointsRemaining -= 1;
			pointDrained = true;
		} else if (!depleted) {
			depleted = true;
			Destroy (transform.GetChild (4).gameObject);
		}
		return pointDrained;
	}
}

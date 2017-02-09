using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolThingToLookAt : MonoBehaviour {

	private int pointsLeftForPlayer1;
	private int pointsLeftForPlayer2;

	// Use this for initialization
	void Start () {
		int points = 10;
		pointsLeftForPlayer1 = points;
		pointsLeftForPlayer2 = points;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool DrainPoint(int playerNum){
		bool pointDrained = false;
		if (playerNum == 1) {
			if (pointsLeftForPlayer1 > 0) {
				pointsLeftForPlayer1 -= 1;
				pointDrained = true;
			}
		} else if (playerNum == 2) {
			if (pointsLeftForPlayer2 > 0) {
				pointsLeftForPlayer2 -= 1;
				pointDrained = true;
			}
		}
		return pointDrained;
	}
}

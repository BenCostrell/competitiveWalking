using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject coolThingPrefab;
	public GameObject player1;
	public GameObject player2;
	public Vector3 player1Spawn;
	public Vector3 player2Spawn;
	public GameObject player1Score;
	public GameObject player2Score;
	public GameObject player1Warning;
	public GameObject player2Warning;
	public GameObject winScreen;

	public float minAcceptableDistance;
	public int numCoolThings;
	public float xMin;
	public float xMax;
	public float zMin;
	public float zMax;
	private List<GameObject> coolThingList;

	public bool gameOver;

	// Use this for initialization
	void Start () {
		gameOver = false;
		winScreen.SetActive (false);
		InitializePlayers ();
		SetUpEnvironment ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset")){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	void InitializePlayers(){
		player1 = Instantiate (playerPrefab, player1Spawn, Quaternion.identity) as GameObject;
		PlayerController pc1 = player1.GetComponent<PlayerController> ();
		pc1.playerNum = 1;
		player1.GetComponent<Camera> ().rect = new Rect (0, 0, 0.49f, 1);
		pc1.scoreUI = player1Score;
		pc1.speedWarning = player1Warning;

		player2 = Instantiate (playerPrefab, player2Spawn, Quaternion.identity) as GameObject;
		PlayerController pc2 = player2.GetComponent<PlayerController> ();
		pc2.playerNum = 2;
		player2.GetComponent<Camera> ().rect = new Rect (0.51f, 0, 0.49f, 1);		
		pc2.scoreUI = player2Score;
		pc2.speedWarning = player2Warning;
	}

	void SetUpEnvironment(){
		coolThingList = new List<GameObject> ();
		coolThingList.Add (player1);
		coolThingList.Add (player2);
		for (int i = 0; i < numCoolThings; i++) {
			Vector3 location = GenerateRandomLocation ();
			bool isValidated = ValidateLocation (location);
			while (!isValidated) {
				location = GenerateRandomLocation ();
				isValidated = ValidateLocation (location);
			}
			CreateCoolThing (location);
		}
	}

	Vector3 GenerateRandomLocation(){
		float x = Random.Range (xMin, xMax);
		float z = Random.Range (zMin, zMax);
		return new Vector3 (x, 0, z);
	}

	bool ValidateLocation(Vector3 location){
		bool isValidated = true;
		if (coolThingList.Count > 0) {
			foreach (GameObject ct in coolThingList) {
				if (Vector3.Distance(location, ct.transform.position) < minAcceptableDistance) {
					isValidated = false;
					break;
				}
			}
		}
		return isValidated;
	}

	void CreateCoolThing(Vector3 location){
		GameObject coolThing = Instantiate (coolThingPrefab, location, Quaternion.identity) as GameObject;
		coolThingList.Add (coolThing);
	}

	public void EndGame(int playerNum){
		int otherPlayerNum = 1 + (playerNum % 2);
		gameOver = true;
		player1Warning.SetActive (false);
		player2Warning.SetActive (false);
		winScreen.SetActive (true);
		winScreen.GetComponent<Text> ().text = "PLAYER " + playerNum + " TOTALLY OUTWALKED PLAYER " + otherPlayerNum;
	}
}

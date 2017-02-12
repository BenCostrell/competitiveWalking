using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private CharacterController characterController;
	private GameManager gameManager;
	public int playerNum;
	public float accel;
	public float maxSpeed;
	public float lookDistance;
	public float rotationSpeed;
	private Rigidbody rb;
	private int experienceEnjoyment;
	public GameObject scoreUI;
	public GameObject speedWarning;
	private float timeUntilNextSpeedCheck;
	public LayerMask coolThingLayermask;
	private AudioSource buzzer;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		rb = GetComponent<Rigidbody> ();
		timeUntilNextSpeedCheck = 0;
		experienceEnjoyment = 0;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		buzzer = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			Look ();
			if (timeUntilNextSpeedCheck > 0) {
				timeUntilNextSpeedCheck -= Time.deltaTime;
			} else {
				CheckIfImGoingTooFast ();
				timeUntilNextSpeedCheck = 0.5f;
				if (experienceEnjoyment >= 100) {
					UpdateScoreUI ();
					gameManager.EndGame (playerNum);
				}
			}
		}
	}

	void FixedUpdate(){
		if (!gameManager.gameOver) {
			Move ();
			RotateView ();
		}
	}

	void Move(){
		Vector3 inputDirection = new Vector3 (Input.GetAxis ("Horizontal_P" + playerNum), 0, Input.GetAxis ("Vertical_P" + playerNum));
		Vector3 movementDirection = Quaternion.Euler (transform.rotation.eulerAngles) * inputDirection;
		if (Vector3.Dot(rb.velocity, movementDirection.normalized) < maxSpeed) {
			rb.AddForce (accel * movementDirection);
		} else {
			rb.velocity = maxSpeed * movementDirection;
		}
	}

	void RotateView(){
		float input;
		input = Input.GetAxis("Rotation_P" + playerNum);
		if (input != 0) {
			transform.Rotate (Vector3.up, rotationSpeed * Mathf.Sign (input));
		}
	}

	void Look(){
		float angleLooking = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;
		Vector3 lookDirection = new Vector3 (Mathf.Sin (angleLooking), 0, Mathf.Cos (angleLooking));
		RaycastHit hit;
		if (Physics.Raycast (transform.position, lookDirection, out hit, lookDistance, coolThingLayermask)) {
			CoolThingToLookAt coolThing = hit.collider.gameObject.GetComponent<CoolThingToLookAt> ();
			coolThing.isBeingLookedAt = true;
			if (timeUntilNextSpeedCheck <= 0) {
				if (coolThing.DrainPoint (playerNum)) {
					experienceEnjoyment += 1;
					UpdateScoreUI ();
				}
			}
		}
	}

	void CheckIfImGoingTooFast(){
		if (rb.velocity.magnitude > maxSpeed / 2) {
			experienceEnjoyment -= 5;
			speedWarning.SetActive (true);
			buzzer.Play ();
		} else {
			speedWarning.SetActive (false);
			if (buzzer.isPlaying) {
				buzzer.Stop ();
			}
		}
		UpdateScoreUI ();
	}

	void UpdateScoreUI(){
		scoreUI.GetComponent<Text> ().text = experienceEnjoyment.ToString();
	}
}

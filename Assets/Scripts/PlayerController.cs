using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private CharacterController characterController;
	public int playerNum;
	public float accel;
	public float maxSpeed;
	public float lookDistance;
	public float rotationSpeed;
	private Rigidbody rb;
	private int experienceEnjoyment;
	private GameObject scoreUI;
	private GameObject speedWarning;
	private float timeUntilNextSpeedCheck;
	public LayerMask coolThingLayermask;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		rb = GetComponent<Rigidbody> ();
		scoreUI = transform.GetChild (0).gameObject;
		speedWarning = transform.GetChild (1).gameObject;
		timeUntilNextSpeedCheck = 0;
		experienceEnjoyment = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilNextSpeedCheck > 0) {
			timeUntilNextSpeedCheck -= Time.deltaTime;
		} else {
			CheckIfImGoingTooFast ();
			Look ();
			timeUntilNextSpeedCheck = 0.5f;
		}
	}

	void FixedUpdate(){
		Move ();
		RotateView ();
	}

	void Move(){
		Vector3 inputDirection = new Vector3 (Input.GetAxis ("Horizontal_P" + playerNum), 0, Input.GetAxis ("Vertical_P" + playerNum));
		Vector3 movementDirection = Quaternion.Euler (transform.rotation.eulerAngles) * inputDirection;
		Debug.Log (rb.velocity.magnitude);
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
			if (hit.collider.gameObject.GetComponent<CoolThingToLookAt> ().DrainPoint (playerNum)) {
				experienceEnjoyment += 1;
			}
		}
	}

	void CheckIfImGoingTooFast(){
		if (rb.velocity.magnitude > maxSpeed / 2) {
			experienceEnjoyment -= 1;
			speedWarning.SetActive (true);
		} else {
			speedWarning.SetActive (false);
		}
		UpdateScoreUI ();
	}

	void UpdateScoreUI(){
		scoreUI.GetComponent<TextMesh> ().text = experienceEnjoyment.ToString();
	}
}

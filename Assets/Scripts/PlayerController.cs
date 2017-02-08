using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private CharacterController characterController;
	public int playerNum;
	public float speed;
	public float lookDistance;
	public float rotationSpeed;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Look ();
	}

	void FixedUpdate(){
		Move ();
		RotateView ();
	}

	void Move(){
		Vector3 inputDirection = new Vector3 (Input.GetAxis ("Horizontal_P" + playerNum), 0, Input.GetAxis ("Vertical_P" + playerNum));
		Vector3 movementDirection = Quaternion.Euler (transform.rotation.eulerAngles) * inputDirection;
		rb.velocity = speed * movementDirection;
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
		Vector3 lookDirection = new Vector3 (Mathf.Cos (angleLooking), 0, Mathf.Sin (angleLooking));
		Debug.DrawRay (transform.position, transform.position + lookDirection);
		if (Physics.Raycast (transform.position, transform.position + lookDirection, lookDistance)) {
			Debug.Log ("saw a thing");
		}
	}

}

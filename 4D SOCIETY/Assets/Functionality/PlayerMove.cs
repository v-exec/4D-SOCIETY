using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	//components
	private Rigidbody rb;
	private GameObject cam;
	private Camera camComponent;

	//movement
	private float walkSpeed = 0.05f;
	private float speedChangeWalk = 1.0f;
	private float speedChangeStop = 1.0f;
	private float directionChangeSpeed = 6f;
	public float targetSpeed = 0f;
	private Vector2 targetDirection = new Vector2(0, 0);
	
	void Start() {
		//get components
		cam = GameObject.Find("Raw Camera");
		camComponent = cam.GetComponent<Camera>();
		rb = GetComponent<Rigidbody>();
	}

	//escape
	void Update() {
		handleKeys();
	}

	void FixedUpdate() {
		move();
	}

	void handleKeys() {
		if (Input.GetKeyDown("escape")) {
			Application.Quit();
		}
	}

	void move() {
		//get input
		float horizontal = 0f;
		float vertical = 0f;
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) horizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) vertical = Input.GetAxis("Vertical");
		Vector2 direction = getInput(horizontal, vertical);
		Vector3 newLoc = new Vector3(gameObject.transform.position.x + direction.x, gameObject.transform.position.y, gameObject.transform.position.z + direction.y);

		//disable gravity when grounded to allow for climbing slopes
		if (!isGrounded(0.4f)) rb.useGravity = true;
		else rb.useGravity = false;

		//apply movement
		rb.MovePosition(newLoc);

		//no movement - stop all forces (excluding vertical force for jumping)
		if (horizontal == 0f && vertical == 0f) {
			targetSpeed = FourD.ease(targetSpeed, 0f, speedChangeStop);
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
		} else targetSpeed = FourD.ease(targetSpeed, walkSpeed, speedChangeWalk);
	}

	Vector2 getInput(float horizontal, float vertical) {
		//calculating direction vector
		Vector3 direction = new Vector3(horizontal, 0.0f, vertical);

		//create rotated transform that is locked to avoid up/down camera angle affecting direction magnitude
		Quaternion cameraRotation = cam.transform.rotation;
		cam.transform.Rotate(Vector3.left, cam.transform.localRotation.eulerAngles.x);

		direction = cam.transform.TransformDirection(direction);
		direction.y = 0.0f;

		//revert camera's rotation
		cam.transform.rotation = cameraRotation;

		//limit input magnitude (to avoid high-magnitude input when moving diagonally)
		direction = Vector3.Normalize(direction);

		//ease direction for smoother movement
		float changer = directionChangeSpeed;

		targetDirection.x = FourD.ease(targetDirection.x, direction.x, changer);
		targetDirection.y = FourD.ease(targetDirection.y, direction.z, changer);

		//amplify normalized vector to desired speed
		return new Vector2(targetDirection.x, targetDirection.y) * targetSpeed;
	}

	public float getSpeed() {
		return targetSpeed;
	}

	bool isGrounded(float extraDistance) {
		return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + extraDistance);
	}
}
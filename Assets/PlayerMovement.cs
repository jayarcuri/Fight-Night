using UnityEngine;
using System.Collections;

public enum CharacterState{Standing, Jumping}
public enum MovementDirection{None, Left, Right}

public class PlayerMovement : MonoBehaviour {

	public float speed = 2f;
	public float jumpForce = 100f;

	string horizontalAxis;
	Rigidbody playerBody; 
	public CharacterState state;
	public MovementDirection moveDirection;
	public float jumpMovementModifier = .25f;
	public float remainingJumpTime = 1f;
	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody>();
		state = CharacterState.Standing;
		horizontalAxis = GetComponent<PlayerController> ().horizontalAxis;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Floor") {
			state = CharacterState.Standing;
		}
}

	public void Move(float horizontal){
		Vector3 newPositionDelta = Vector3.zero;

		if (state != CharacterState.Jumping) {
			if (horizontal != 0) {
				if (horizontal > 0)
					moveDirection = MovementDirection.Right;
				else
					moveDirection = MovementDirection.Left;
			
				playerBody.MovePosition(Vector3.right * speed * horizontal);
			} else if (state == CharacterState.Jumping)
				moveDirection = MovementDirection.None;
		}
	}
		
	// Jumpman, jumpman, jumpman them boys up to somethin'...
	public void Jump() {
		if (state == CharacterState.Standing) {
			// Set all variables for jump state
			state = CharacterState.Jumping;
			playerBody.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			StartCoroutine ("AirMovement");
		}
	}

	IEnumerator AirMovement() {
		Vector3 velocityDelta = playerBody.velocity;
		while (state == CharacterState.Jumping) {
			if (moveDirection == MovementDirection.Left) {
				velocityDelta += Vector3.right * -speed * Time.deltaTime;
			} else if (moveDirection == MovementDirection.Right) {
				velocityDelta += Vector3.right * speed * Time.deltaTime;
			}
			if (Input.GetAxis(horizontalAxis) > 0)
				velocityDelta += Vector3.right * speed * jumpMovementModifier * Time.deltaTime;
			else if (Input.GetAxis(horizontalAxis) < 0)
				velocityDelta += Vector3.right * speed * -jumpMovementModifier * Time.deltaTime;

			playerBody.velocity = velocityDelta;
			yield return null;
		}
	}

	IEnumerator AirMovement2() {
		Vector3 velocityDelta = playerBody.velocity;
		while (state == CharacterState.Jumping) {
			if (moveDirection == MovementDirection.Left) {
				velocityDelta += transform.right * -speed * Time.deltaTime;
			} else if (moveDirection == MovementDirection.Right) {
				velocityDelta += transform.right * speed * Time.deltaTime;
			}
			if (Input.GetAxis(horizontalAxis) > 0)
				velocityDelta += transform.right * speed * jumpMovementModifier * Time.deltaTime;
			else if (Input.GetAxis(horizontalAxis) < 0)
				velocityDelta += transform.right * speed * -jumpMovementModifier * Time.deltaTime;

			playerBody.velocity = velocityDelta;
			yield return null;
		}
	}
}

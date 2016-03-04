using UnityEngine;
using System.Collections;

public enum CharacterState{Standing, Jumping}
public enum MovementDirection{None, Left, Right}

public class PlayerMovement : MonoBehaviour {

	public float speed = 2f;
	public float jumpForce = 100f;

	public CharacterState state;
	public MovementDirection moveDirection;
	public float jumpMovementModifier = .25f;
	public float jumpTime = 1f;
	public float jumpHeight = 3f;

	string horizontalAxis;
	Rigidbody playerBody; 
	float initialHeight;
	float remainingJumpTime;
	float westStageConstraint = -7.5f;
	float eastStageConstraint = 7.5f;


	void Start () {
		playerBody = GetComponent<Rigidbody>();
		state = CharacterState.Standing;
		horizontalAxis = GetComponent<PlayerController> ().horizontalAxis;
		initialHeight = playerBody.position.y;
		speed = speed / 60;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Floor") {
			state = CharacterState.Standing;
		}
}

	public void Move(float horizontal) {
		if (horizontal != 0 || state == CharacterState.Jumping) {
			// variable which will be modified by checks for different states which impact movement
			Vector3 moveTo = playerBody.position;

			if (state != CharacterState.Jumping) {
				if (horizontal > 0)
					moveDirection = MovementDirection.Right;
				else
					moveDirection = MovementDirection.Left;
				moveTo += Vector3.right * speed * horizontal;
				} 
			// If jumping...
			else {
				moveTo += JumpVelocity ();
				moveTo += Vector3.right * speed * horizontal * jumpMovementModifier;
			}
			playerBody.MovePosition (moveTo);
		}
	}
		
	// Jumpman, jumpman, jumpman them boys up to somethin'...
	public void Jump() {
		if (state == CharacterState.Standing) {
			// Set all variables for jump state
			state = CharacterState.Jumping;
			remainingJumpTime = jumpTime;
		}
	}

	Vector3 JumpVelocity() {
		remainingJumpTime -= Time.fixedDeltaTime;
		Vector3 moveBy = Vector3.zero;

		if (remainingJumpTime >= jumpTime) {
			moveBy.y += jumpHeight / (jumpTime / 2);

		} else {
			moveBy.y -= jumpHeight / (jumpTime / 2);
		}

		// Move horizontal
		switch (moveDirection) {
		case MovementDirection.Left:
			moveBy += Vector3.right * -speed;
			break;
		case MovementDirection.Right:
			moveBy += Vector3.right * speed;
			break;
		}


		// Verify player is within bounds of level and constrain them
		if (playerBody.position.y + moveBy.y > jumpHeight) {
			moveBy.y = playerBody.position.y - jumpHeight;
		} else if (playerBody.position.y - moveBy.y <= initialHeight) {
			moveBy.y = initialHeight - playerBody.position.y;
			// Test if this works better than a collision check for setting stand state	
			state = CharacterState.Standing;
		} 
		// Verify within horizontal bounds
		if (playerBody.position.x + moveBy.x > eastStageConstraint) {
			moveBy.x = eastStageConstraint - playerBody.position.x;
		} else if (playerBody.position.x + moveBy.x < westStageConstraint) {
			moveBy.x = playerBody.position.x - westStageConstraint;
		}

		return moveBy;
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

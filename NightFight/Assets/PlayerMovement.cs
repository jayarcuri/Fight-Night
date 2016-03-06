using UnityEngine;
using System.Collections;

public enum CharacterState{Standing, Jumping}
public enum MovementDirection{None, Left, Right}

public class PlayerMovement : MonoBehaviour {

	public float speed = 2f;
	public float jumpForce = 100f;


	public float jumpMovementModifier = .25f;
	public float jumpTime = 1f;
	public float jumpHeight = 3f;

	Rigidbody playerBody; 
	CharacterState state;
	MovementDirection moveDirection;
	string horizontalAxis;
	float initialHeight;
	float remainingJumpTime;
	// Make these get pulled by code
	float westStageConstraint = -7.5f;
	float eastStageConstraint = 7.5f;


	void Start () {
		playerBody = GetComponent<Rigidbody>();
		state = CharacterState.Standing;
		horizontalAxis = GetComponent<PlayerController> ().horizontalAxis;
		initialHeight = playerBody.position.y;
		speed = speed / 60;
	}

	/*void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Floor") {
			state = CharacterState.Standing;
		}
}*/

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
			moveTo = ConstrainPlayerPosition (moveTo);
			playerBody.MovePosition (moveTo);
		} else
			moveDirection = MovementDirection.None;
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

		if (remainingJumpTime >= jumpTime / 2) {
			moveBy.y += jumpHeight * Time.fixedDeltaTime / (jumpTime / 2);

		} else if (remainingJumpTime > 0) {
			moveBy.y -= jumpHeight * Time.fixedDeltaTime / (jumpTime / 2);
		} else if (remainingJumpTime <= 0)
			state = CharacterState.Standing;

		// Move horizontal
		switch (moveDirection) {
		case MovementDirection.Left:
			moveBy += Vector3.right * -speed * 1/3;
			break;
		case MovementDirection.Right:
			moveBy += Vector3.right * speed * 1/3;
			break;
		} 

		return moveBy;
	}

	Vector3 ConstrainPlayerPosition(Vector3 newPosition) {
		// Verify player is within bounds of level and constrain them
		if (newPosition.y > jumpHeight) {
			newPosition.y = jumpHeight;
		} else if (newPosition.y < initialHeight) {
			newPosition.y = initialHeight;
		}
		// Verify within horizontal bounds
		if (newPosition.x > eastStageConstraint) {
			newPosition.x = eastStageConstraint;
		} else if (newPosition.x < westStageConstraint) {
			newPosition.x = westStageConstraint;
		}
		return newPosition;
	}
		
}

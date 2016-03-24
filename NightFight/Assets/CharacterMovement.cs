using UnityEngine;
using System.Collections;

public enum CharacterState{Standing, Jumping}
public enum MovementDirection{None, Left, Right}

public class CharacterMovement : MonoBehaviour {

	public float speed = 2f;

	public float jumpMovementModifier;

	public float initialJumpVelocity;
	public float gravityForce;
	public float terminalVelocity;

	Rigidbody playerBody; 
	public CharacterState state;
	MovementDirection moveDirection;

	float initialHeight;
	float remainingJumpTime;
	public float currentJumpVelocity;
	// Make these get pulled by code
	float westStageConstraint = -7.5f;
	float eastStageConstraint = 7.5f;


	void Start () {
		playerBody = GetComponent<Rigidbody>();
		state = CharacterState.Standing;
		initialHeight = playerBody.position.y;
		speed = speed / 60;
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
				moveTo += GetJumpVelocity ();
				if (moveDirection == MovementDirection.None) // Allow "steering" in the air if the player neutral jumps
					moveTo += Vector3.right * speed * horizontal * jumpMovementModifier * 3/5;
			}
			moveTo = ConstrainPlayerPosition (moveTo);
			playerBody.MovePosition (moveTo);
		} else
			moveDirection = MovementDirection.None;
	}
		
	// Jumpman, jumpman, jumpman them boys up to somethin'...
	public void Jump(float horizontalInput) {
		if (state == CharacterState.Standing) {
			// Set all variables for jump state
			currentJumpVelocity = initialJumpVelocity;
			state = CharacterState.Jumping;
			switch ((int)horizontalInput) {
			case 1:
				moveDirection = MovementDirection.Right;
				break;
			case 0:
				moveDirection = MovementDirection.None;
				break;
			case -1:
				moveDirection = MovementDirection.Left;
				break;
			}
		}
	}

	Vector3 GetJumpVelocity() {
		remainingJumpTime -= Time.fixedDeltaTime; 
		Vector3 moveBy = Vector3.zero;

		moveBy.y += currentJumpVelocity;
 
		currentJumpVelocity -= gravityForce;
		if (currentJumpVelocity < terminalVelocity) {
			currentJumpVelocity = terminalVelocity;
		}

		// Move horizontal
		switch (moveDirection) {
		case MovementDirection.Left:
			moveBy += Vector3.right * -speed * jumpMovementModifier;
			break;
		case MovementDirection.Right:
			moveBy += Vector3.right * speed * jumpMovementModifier;
			break;
		} 

		return moveBy;
	}

	Vector3 ConstrainPlayerPosition(Vector3 newPosition) {
		// Verify player is within bounds of level and constrain them
		if (newPosition.y < initialHeight) {
			newPosition.y = initialHeight;
			state = CharacterState.Standing;
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

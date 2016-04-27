using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float speed = 2f;

	public float jumpMovementModifier;

	public float initialJumpVelocity;
	public float gravityForce;
	public float terminalVelocity;

	Rigidbody playerBody; 
	public CharacterAction action;
	MovementDirection moveDirection;
	//Transform 

	float initialHeight;
	float remainingJumpTime;
	public float currentJumpVelocity;
	// Make these get pulled by code
	float westStageConstraint = -9.5f;
	float eastStageConstraint = 9.5f;


	void Start () {
		playerBody = GetComponent<Rigidbody>();
		action = CharacterAction.Standing;
		initialHeight = playerBody.position.y;
		speed = speed / 60;
	}

	public void Move(int horizontal) {
		// Hacky solution to simplifying horizontal inputs
		switch (horizontal) {
		case 2:
			horizontal = 0;
			break;

		case 0:
			horizontal = 1;
			break;

		case 1:
			horizontal = -1;
			break;
		}
		if (horizontal != 0 || action == CharacterAction.Jumping) {
			// variable which will be modified by checks for different states which impact movement
			Vector3 moveTo = playerBody.position;

			if (action != CharacterAction.Jumping) {
				if (horizontal == 1)
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

	public void Jump(int horizontalInput) {
		if (action == CharacterAction.Standing) {
			// Set all variables for jump state
			currentJumpVelocity = initialJumpVelocity;
			action = CharacterAction.Jumping;

			if (horizontalInput > 0)
				moveDirection = MovementDirection.Right;
			if (horizontalInput == 0)
				moveDirection = MovementDirection.None;
			if (horizontalInput < 0)
				moveDirection = MovementDirection.Left;
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
			action = CharacterAction.Standing;
		}
		// Verify within horizontal bounds
		if (newPosition.x + transform.localScale.x/2 > eastStageConstraint) {
			newPosition.x = eastStageConstraint - transform.localScale.x/2;
		} else if (newPosition.x - transform.localScale.x/2 < westStageConstraint) {
			newPosition.x = westStageConstraint + transform.localScale.x/2;
		}
		return newPosition;
	}
		
}
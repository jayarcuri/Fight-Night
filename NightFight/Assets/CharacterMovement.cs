using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

	public float speed = 2f;
	public float jumpMovementModifier;
	public float initialJumpVelocity;
	public float gravityForce;
	public float terminalVelocity;

	Rigidbody playerBody;

	MovementDirection moveDirection;
	Transform opponentTransform;

	public bool isFacingRight;

	float initialHeight;
	float remainingJumpTime;
	public float currentJumpVelocity;
	// Make these get pulled by code
	float westStageConstraint = -9.5f;
	float eastStageConstraint = 9.5f;


	void Start ()
	{
		playerBody = GetComponent<Rigidbody> ();
		string opponentTag = gameObject.tag.Equals ("Player1") ? "Player2" : "Player1";
		opponentTransform = GameObject.FindGameObjectWithTag (opponentTag).GetComponent<Transform> ();

		if (opponentTransform == null) {
			throw new UnityException ("Cannot find other player with tag \'" + opponentTag + "\'.");
		}
			
		initialHeight = playerBody.position.y;
		speed = speed / 60;
		isFacingRight = transform.localPosition.x < opponentTransform.transform.localPosition.x;
	}

	public void SetOpponentTransform (Transform oTransform)
	{
		opponentTransform = oTransform;
	}

	public void FlipRotation ()
	{
		if (!isFacingRight && transform.localPosition.x < opponentTransform.localPosition.x) {
			isFacingRight = true;
		} else if (isFacingRight && transform.localPosition.x > opponentTransform.localPosition.x) {
			isFacingRight = false;
		} else {
			return;
		}
		
		Vector2 newRotation = transform.localEulerAngles; 
		newRotation.y -= 180; 
		transform.localEulerAngles = newRotation; 
	}

	public void MoveByVector (Vector2 difference)
	{
		if (!isFacingRight) {
			difference -= new Vector2 (difference.x * 2, 0);
		}
		Vector2 moveTo = playerBody.position;

		moveTo += difference;
		moveTo = ConstrainPlayerPosition (moveTo);
		playerBody.MovePosition (moveTo);
	}

	public void Move (MoveFrame stepDirection)
	{
		// Hacky solution to simplifying horizontal inputs
		int horizontal = stepDirection.moveType == MoveType.STEP_FORWARD ? 1 : -1;
		if (!isFacingRight)
			horizontal *= -1;

		if (horizontal != 0) {
			// variable which will be modified by checks for different states which impact movement
			Vector2 moveTo = playerBody.position;

			if (horizontal == 1) 
				moveDirection = MovementDirection.Right;
			else
				moveDirection = MovementDirection.Left;
				
			moveTo += Vector2.right * speed * horizontal;
			moveTo = ConstrainPlayerPosition (moveTo);
			playerBody.MovePosition (moveTo);
		} else {
			moveDirection = MovementDirection.None;
		}
	}

	Vector2 GetJumpVelocity ()
	{
		remainingJumpTime -= Time.fixedDeltaTime; 
		Vector2 moveBy = Vector2.zero;

		moveBy.y += currentJumpVelocity;
 
		currentJumpVelocity -= gravityForce;
		if (currentJumpVelocity < terminalVelocity) {
			currentJumpVelocity = terminalVelocity;
		}

		// Move horizontal
		switch (moveDirection) {
		case MovementDirection.Left:
			moveBy += Vector2.right * -speed * jumpMovementModifier;
			break;
		case MovementDirection.Right:
			moveBy += Vector2.right * speed * jumpMovementModifier;
			break;
		} 

		return moveBy;
	}

	Vector2 ConstrainPlayerPosition (Vector2 newPosition)
	{
		// Verify player is within bounds of level and constrain them
		if (newPosition.y < initialHeight) {
			newPosition.y = initialHeight;
			//action = CharacterAction.Standing;
		}
		// Verify within horizontal bounds
		if (newPosition.x + transform.localScale.x / 2 > eastStageConstraint) {
			newPosition.x = eastStageConstraint - transform.localScale.x / 2;
		} else if (newPosition.x - transform.localScale.x / 2 < westStageConstraint) {
			newPosition.x = westStageConstraint + transform.localScale.x / 2;
		}
		return newPosition;
	}
		
}
using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	Rigidbody playerBody;
	MovementDirection moveDirection;
	Transform opponentTransform;

	public bool isFacingRight;

	void Start ()
	{
		playerBody = GetComponent<Rigidbody> ();
		string opponentTag = gameObject.tag.Equals ("Player1") ? "Player2" : "Player1";
		opponentTransform = GameObject.FindGameObjectWithTag (opponentTag).GetComponent<Transform> ();

		if (opponentTransform == null) {
			throw new UnityException ("Cannot find other player with tag \'" + opponentTag + "\'.");
		}

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
		playerBody.MovePosition (moveTo);
	}
		
}
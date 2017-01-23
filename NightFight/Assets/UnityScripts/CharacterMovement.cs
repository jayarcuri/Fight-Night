using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	Rigidbody playerBody;
	GameObject playerLightObject;
	Transform opponentTransform;

	public bool isFacingRight;

	void Start ()
	{
		playerBody = GetComponent<Rigidbody> ();
		playerLightObject = GetComponentInChildren<Light> ().gameObject;
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

		float rotationDegrees = 180;
		
		Vector2 newPlayerRotation = transform.localEulerAngles; 
		newPlayerRotation.y -= rotationDegrees; 
		transform.localEulerAngles = newPlayerRotation;

		Vector2 newLightRotation = playerLightObject.transform.localEulerAngles;
		newLightRotation.y -= rotationDegrees;
		playerLightObject.transform.localEulerAngles = newLightRotation;

		Vector3 lightPosition = playerLightObject.transform.localPosition;
		Vector3 newLightPosition = new Vector3 (lightPosition.x, lightPosition.y, lightPosition.z * -1);
		playerLightObject.transform.localPosition = newLightPosition;
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
using UnityEngine;
using System.Collections;
using Eppy;

public class CollisionUtils
{
	public static readonly Vector2 NaV2 = new Vector2(float.NaN, float.NaN);

	public static Vector2 GetPointOfImpact(Vector2 leftCharacterPosition, Vector2 leftCharacterVelocity, 
		Vector2 rightCharacterPosition, Vector2 rightCharacterVelocity) {
		float distance = rightCharacterPosition.x - leftCharacterPosition.x;
		// if a collision does not occur...
		if ((0 <= leftCharacterVelocity.x && 0 <= rightCharacterVelocity.x) || (0 >= leftCharacterVelocity.x && 0 >= rightCharacterVelocity.x)) {
			// a - d - v_a
			// if the distance traveled by b exceeds the initial distance between a & b plus the distance traveled by a, then a collision will occur
			if (Mathf.Abs(rightCharacterVelocity.x - leftCharacterVelocity.x) >= distance) {
				float velocityDifferential = rightCharacterVelocity.x - leftCharacterVelocity.x;
				float percentDistanceTraveledBeforeCollision = Mathf.Abs (distance / velocityDifferential);
				float percentDistanceTraveledAfter = 1f - percentDistanceTraveledBeforeCollision;

				// velocity after contact (for both players) is equal to the remaining "percentage distance" multiplied by the velocity differential
				float velocityAfterContact = velocityDifferential * percentDistanceTraveledAfter;

				return new Vector2 (rightCharacterPosition.x + (percentDistanceTraveledBeforeCollision * rightCharacterVelocity.x), 0);
			}
		}	else if (distance <= leftCharacterVelocity.x + Mathf.Abs (rightCharacterVelocity.x)) {

			float ratio = Mathf.Abs (leftCharacterVelocity.x) / (Mathf.Abs (leftCharacterVelocity.x) + Mathf.Abs (rightCharacterVelocity.x));
			float offsetFromLeft = distance * ratio;

			return new Vector2 (leftCharacterPosition.x + offsetFromLeft, 0);

		}
		//	else
		return NaV2;
	}

	public static Tuple<Vector2, Vector2> GetUpdatedVelocities (Vector2 leftCharacterPosition, Vector2 leftCharacterVelocity, 
		Vector2 rightCharacterPosition, Vector2 rightCharacterVelocity) {
		float distance = rightCharacterPosition.x - leftCharacterPosition.x;
		// if a collision does not occur...
		float percentageTraveledBefore;
		float percentTraveledAfter;

		if (leftCharacterVelocity.x != 0) {
			Debug.Log ("Hey sailor.");
		}

		if ((0 <= leftCharacterVelocity.x && 0 <= rightCharacterVelocity.x) || (0 >= leftCharacterVelocity.x && 0 >= rightCharacterVelocity.x)) {
			// a - d - v_a
			// if the distance traveled by b exceeds the initial distance between a & b plus the distance traveled by a, then a collision will occur
			if (Mathf.Abs(rightCharacterVelocity.x - leftCharacterVelocity.x) >= Mathf.Abs(distance)) {
				float velocityDifferential = rightCharacterVelocity.x - leftCharacterVelocity.x;
				percentageTraveledBefore = Mathf.Abs (distance / velocityDifferential);
				percentTraveledAfter = 1f - percentageTraveledBefore;

				// velocity after contact (for both players) is equal to the remaining "percentage distance" multiplied by the velocity differential
				float velocityAfterContact = velocityDifferential * percentageTraveledBefore;

				Vector2 newLeftVelocity = new Vector2 ((leftCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);
				Vector2 newRightVelocity = new Vector2 ((rightCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);
					
				return new Tuple<Vector2, Vector2> (newLeftVelocity, newRightVelocity);
				//return new Vector2 (rightCharacterPosition.x + (percentDistanceTraveledBeforeCollision * rightCharacterVelocity.x), 0);
			}
		}	else if (distance <= leftCharacterVelocity.x + Mathf.Abs (rightCharacterVelocity.x)) {

			float ratio = Mathf.Abs (leftCharacterVelocity.x) / (Mathf.Abs (leftCharacterVelocity.x) + Mathf.Abs (rightCharacterVelocity.x));
			float offsetFromLeft = distance * ratio;

			percentageTraveledBefore = leftCharacterVelocity.x / offsetFromLeft;
			percentTraveledAfter = 1f - percentageTraveledBefore;

			float velocityAfterContact = (leftCharacterVelocity.x - rightCharacterVelocity.x) * percentTraveledAfter;

			Vector2 newLeftVelocity = new Vector2 ((leftCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);
			Vector2 newRightVelocity = new Vector2 ((rightCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);

			return new Tuple<Vector2, Vector2> (newLeftVelocity, newRightVelocity);
			//return new Vector2 (leftCharacterPosition.x + offsetFromLeft, 0);

		}
		//	else
		return new Tuple<Vector2, Vector2>(NaV2, NaV2);

	}

	public static Tuple<Vector2, Vector2> TakeThree(Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {
		Transform leftCharacterTransform;
		Vector2 leftCharacterVelocity;
		Transform rightCharacterTransform;
		Vector2 rightCharacterVelocity;

		if (p1Transform.position.Equals(p2Transform.position)) {
			throw new UnityException ("There should NEVER be a case of 1 to 1 overlap before velocity is calculated.");
		}

		bool p1IsFacingRight = p1Transform.position.x < p2Transform.position.x;

		if (p1IsFacingRight) {
			leftCharacterTransform = p1Transform;
			leftCharacterVelocity = p1Velocity;
			rightCharacterTransform = p2Transform;
			rightCharacterVelocity = p2Velocity;
		} else {
			leftCharacterTransform = p2Transform;
			leftCharacterVelocity = p2Velocity;
			rightCharacterTransform = p1Transform;
			rightCharacterVelocity = p1Velocity;
		}

		Vector2 a = new Vector2 (leftCharacterTransform.position.x + leftCharacterTransform.localScale.x / 2, leftCharacterTransform.position.y); 
		Vector2 b = new Vector2 (rightCharacterTransform.position.x - rightCharacterTransform.localScale.x / 2, rightCharacterTransform.position.y); 

		Vector2 a_LocationAfterMovement = a + leftCharacterVelocity;
		Vector2 b_LocationAfterMovement = b + rightCharacterVelocity;

		//	If the right-side character's left-side bound has passed the left-side character's right-side bound, then a collision has occurred 
		//	& we need to correct the velocities to prevent this.
		if (b_LocationAfterMovement.x <= a_LocationAfterMovement.x) {
			// get distance between the two overlapped bodies
			float distanceBetween = a_LocationAfterMovement.x - b_LocationAfterMovement.x;
			float velocitiesRatio = Mathf.Abs(leftCharacterVelocity.x) / 
				(Mathf.Abs(leftCharacterVelocity.x) + Mathf.Abs(rightCharacterVelocity.x));
			float newMidPoint = b_LocationAfterMovement.x + (distanceBetween * velocitiesRatio);

			Vector2 newLeftCharacterVelocity = new Vector2 (newMidPoint - leftCharacterTransform.position.x - leftCharacterTransform.localScale.x/2 - 0.05f, 
				leftCharacterTransform.position.y);
			Vector2 newRightCharacterVelocity = new Vector2 (newMidPoint - rightCharacterTransform.position.x + rightCharacterTransform.localScale.x/2 + 0.05f, 
				rightCharacterTransform.position.y);

			if (p1IsFacingRight) {
				return new Tuple<Vector2, Vector2> (newLeftCharacterVelocity, newRightCharacterVelocity);
			} else {
				return new Tuple<Vector2, Vector2> (newRightCharacterVelocity, newLeftCharacterVelocity);
			}
		}


		return new Tuple<Vector2, Vector2>(NaV2, NaV2);
	}




}


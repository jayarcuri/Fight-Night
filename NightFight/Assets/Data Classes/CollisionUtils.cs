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
		if ((0 <= leftCharacterVelocity.x && 0 <= rightCharacterVelocity.x) || (0 >= leftCharacterVelocity.x && 0 >= rightCharacterVelocity.x)) {
			// a - d - v_a
			// if the distance traveled by b exceeds the initial distance between a & b plus the distance traveled by a, then a collision will occur
			if (Mathf.Abs(rightCharacterVelocity.x - leftCharacterVelocity.x) >= distance) {
				float velocityDifferential = rightCharacterVelocity.x - leftCharacterVelocity.x;
				float percentDistanceTraveledBeforeCollision = Mathf.Abs (distance / velocityDifferential);
				float percentDistanceTraveledAfter = 1f - percentDistanceTraveledBeforeCollision;

				// velocity after contact (for both players) is equal to the remaining "percentage distance" multiplied by the velocity differential
				float velocityAfterContact = velocityDifferential * percentDistanceTraveledAfter;

				Vector2 newLeftVelocity = new Vector2 ((leftCharacterVelocity.x * percentDistanceTraveledBeforeCollision) + velocityAfterContact, 0);
				Vector2 newRightVelocity = new Vector2 ((rightCharacterVelocity.x * percentDistanceTraveledBeforeCollision) + velocityAfterContact, 0);
					
				return new Tuple<Vector2, Vector2> (newLeftVelocity, newRightVelocity);
				//return new Vector2 (rightCharacterPosition.x + (percentDistanceTraveledBeforeCollision * rightCharacterVelocity.x), 0);
			}
		}	else if (distance <= leftCharacterVelocity.x + Mathf.Abs (rightCharacterVelocity.x)) {

			float ratio = Mathf.Abs (leftCharacterVelocity.x) / (Mathf.Abs (leftCharacterVelocity.x) + Mathf.Abs (rightCharacterVelocity.x));
			float offsetFromLeft = distance * ratio;

			float percentageTraveledBefore = leftCharacterVelocity.x / offsetFromLeft;
			float percentAfter = 1f - percentageTraveledBefore;

			float velocityAfterContact = (leftCharacterVelocity.x - rightCharacterVelocity.x) * percentAfter;

			Vector2 newLeftVelocity = new Vector2 ((leftCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);
			Vector2 newRightVelocity = new Vector2 ((rightCharacterVelocity.x * percentageTraveledBefore) + velocityAfterContact, 0);

			return new Tuple<Vector2, Vector2> (newLeftVelocity, newRightVelocity);
			//return new Vector2 (leftCharacterPosition.x + offsetFromLeft, 0);

		}
		//	else
		return new Tuple<Vector2, Vector2>(NaV2, NaV2);

	}

}


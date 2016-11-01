using UnityEngine;
using System.Collections;
using System;
using Eppy;

public class CollisionUtils
{
	public static readonly Vector2 NaV2 = new Vector2(float.NaN, float.NaN);
	public static readonly float bufferValue = 0.05f;
	public static readonly float leftSideLevelBounds = -9.5f;
	public static readonly float rightSideLevelBounds = 9.5f;
	public static readonly float floor = -0.77f;

	public static Tuple<Vector2, Vector2> GetUpdatedVelocities(Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {
		Transform leftCharacterTransform;
		Vector2 leftCharacterVelocity;
		Transform rightCharacterTransform;
		Vector2 rightCharacterVelocity;

		if (p1Transform.position.Equals(p2Transform.position)) {
			//	Occurs when a character jumps in on another in the corner.
			throw new UnityException ("There should NEVER be a case of 1 to 1 overlap before velocity is calculated.");
		}

		//	if the vertical distance between p1 & p2 after movement is not less than p1.height/2f + p2.height/2f than no collision can have occurred.
		float min_distance = p1Transform.localScale.y / 2f + p2Transform.localScale.y / 2f;
		float verticalDistanceAfterMovement = Mathf.Abs(p1Transform.position.y + p1Velocity.y - (p2Transform.position.y + p2Velocity.y));

		if (verticalDistanceAfterMovement > min_distance) {
			return new Tuple<Vector2, Vector2>(NaV2, NaV2);
		}

		if (p1Velocity != Vector2.zero && p2Velocity != Vector2.zero) {
			Debug.Log ("Both characters are moving.");
		}

		bool p1IsFacingRight = p1Transform.position.x < p2Transform.position.x;

		if (p1IsFacingRight) {
			leftCharacterTransform = p1Transform;
			leftCharacterVelocity = p1Velocity;
			rightCharacterTransform = p2Transform;
			rightCharacterVelocity = new Vector2(p2Velocity.x * -1f, p2Velocity.y);
		} else {
			leftCharacterTransform = p2Transform;
			leftCharacterVelocity = p2Velocity;
			rightCharacterTransform = p1Transform;
			rightCharacterVelocity = new Vector2(p1Velocity.x * -1f, p1Velocity.y);
		}

		Vector2 a = new Vector2 (leftCharacterTransform.position.x + leftCharacterTransform.localScale.x / 2f, leftCharacterTransform.position.y); 
		Vector2 b = new Vector2 (rightCharacterTransform.position.x - rightCharacterTransform.localScale.x / 2f, rightCharacterTransform.position.y); 

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

			Vector2 newLeftCharacterVelocity = new Vector2 (newMidPoint - leftCharacterTransform.position.x - leftCharacterTransform.localScale.x/2 - bufferValue, 
				leftCharacterVelocity.y);
			Vector2 newRightCharacterVelocity = new Vector2 ((newMidPoint - rightCharacterTransform.position.x + rightCharacterTransform.localScale.x/2 + bufferValue) * -1f, 
				rightCharacterVelocity.y);

			if (p1IsFacingRight) {
				return new Tuple<Vector2, Vector2> (newLeftCharacterVelocity, newRightCharacterVelocity);
			} else {
				return new Tuple<Vector2, Vector2> (newRightCharacterVelocity, newLeftCharacterVelocity);
			}
		}
			
		return new Tuple<Vector2, Vector2>(NaV2, NaV2);
	}

	public static Vector2 GetConstraintedVelocity (Transform characterTransform, Vector2 characterVelocity) {
		Vector2 newCharacterVelocity;
		float characterVelocityModifier = characterTransform.rotation.y != 1f ? 1f : -1f;
		float constraintedYVelocity = GetConstrainedYVelocity (characterTransform, characterVelocity.y);
		float characterWidth = characterTransform.localScale.x / 2f;

		if (characterTransform.position.x + characterVelocity.x * characterVelocityModifier < leftSideLevelBounds + characterWidth) {
			newCharacterVelocity = new Vector2 ((leftSideLevelBounds - characterTransform.position.x + characterWidth) * characterVelocityModifier, constraintedYVelocity);
		} else if (characterTransform.position.x + characterVelocity.x * characterVelocityModifier > rightSideLevelBounds - characterWidth) {
			newCharacterVelocity = new Vector2 ((rightSideLevelBounds - characterTransform.position.x - characterWidth) * characterVelocityModifier, constraintedYVelocity);
		} else if (constraintedYVelocity != characterVelocity.y) {
			newCharacterVelocity = new Vector2(characterVelocity.x, constraintedYVelocity);
		} else {
			newCharacterVelocity = NaV2;
		}

		return newCharacterVelocity;
	}

//	public static Tuple<Vector2, Vector2> GetLegalVelocities (Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {
//		Vector2 newP1Velocity;
//		Vector2 newP2Velocity;
//		float p1VelocityModifier = p1Transform.rotation.y != 1f ? 1f : -1f;
//		float p2VelocityModifier = p2Transform.rotation.y != 1f ? 1f : -1f;
//		float p1YVelocity = GetYVelocity (p1Transform, p1Velocity.y);
//		float p2YVelocity = GetYVelocity (p2Transform, p2Velocity.y);
//
//		if (p1Transform.position.x + p1Velocity.x * p1VelocityModifier < leftSideLevelBounds) {
//			newP1Velocity = new Vector2 ((leftSideLevelBounds - p1Transform.position.x) * p1VelocityModifier, p1YVelocity);
//		} else if (p1Transform.position.x + p1Velocity.x * p1VelocityModifier > rightSideLevelBounds) {
//			newP1Velocity = new Vector2 ((rightSideLevelBounds - p1Transform.position.x) * p1VelocityModifier, p1YVelocity);
//		} else {
//			newP1Velocity = NaV2;
//		}
//
//		if (p2Transform.position.x + p2Velocity.x * p2VelocityModifier < leftSideLevelBounds) {
//			newP2Velocity = new Vector2 ((leftSideLevelBounds - p2Transform.position.x) * p2VelocityModifier, p2YVelocity);
//		} else if (p2Transform.position.x + p2Velocity.x * p2VelocityModifier > rightSideLevelBounds) {
//			newP2Velocity = new Vector2 ((rightSideLevelBounds - p2Transform.position.x) * p2VelocityModifier, p2YVelocity);
//		} else {
//			newP2Velocity = NaV2;
//		}
//
//		return new Tuple<Vector2, Vector2>(newP1Velocity, newP2Velocity);
//	}

	public static Tuple<Vector2, Vector2> GetNonOverlappingVelocities (Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {


		return new Tuple<Vector2, Vector2>(NaV2, NaV2);
	}



	static float GetConstrainedYVelocity(Transform transform, float yVelocity) {
		float height = transform.localScale.y;

		if (transform.position.y - height / 2f + yVelocity < floor) {
			return floor - (transform.position.y - height / 2f);
		}

		return yVelocity;
	}




}


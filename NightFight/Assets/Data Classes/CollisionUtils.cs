using UnityEngine;
using System.Collections;
using System;
using Eppy;

public class CollisionUtils
{
	public static readonly Vector2 NaV2 = new Vector2(float.NaN, float.NaN);
	public static readonly float bufferValue = 0.05f;
	public static float leftSideLevelBounds { get; private set; }
	public static float rightSideLevelBounds { get; private set; }
	public static float floorBounds { get; private set; }

	public static void SetUp () {
		Transform leftWall = GameObject.FindGameObjectWithTag ("LeftWall").GetComponent<Transform> ();
		leftSideLevelBounds = leftWall.position.x + leftWall.localScale.x / 2;
		Transform rightWall = GameObject.FindGameObjectWithTag ("RightWall").GetComponent<Transform> ();
		rightSideLevelBounds = rightWall.position.x - rightWall.localScale.x / 2;
		Transform floor = GameObject.FindGameObjectWithTag ("Floor").GetComponent<Transform> ();
		floorBounds = floor.position.y + floor.localScale.y / 2;
	}

	public static Tuple<Vector2, Vector2> GetUpdatedVelocities(Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {
		Transform leftCharacterTransform;
		Vector2 leftCharacterVelocity;
		Transform rightCharacterTransform;
		Vector2 rightCharacterVelocity;

		if (!VerticalCollisionWillOccur(p1Transform, p1Velocity, p2Transform, p2Velocity)) {
			return new Tuple<Vector2, Vector2>(NaV2, NaV2);
		}

		bool p1IsFacingRight = p1Transform.position.x < p2Transform.position.x;

		leftCharacterTransform = p1IsFacingRight ? p1Transform : p2Transform;
		leftCharacterVelocity = p1IsFacingRight ? p1Velocity : p2Velocity;
		rightCharacterTransform = p1IsFacingRight ? p2Transform : p1Transform;
		rightCharacterVelocity = p1IsFacingRight ? new Vector2(p2Velocity.x * -1f, p2Velocity.y) : new Vector2(p1Velocity.x * -1f, p1Velocity.y);

		float leftCharacterRightSideBounds = leftCharacterTransform.position.x + leftCharacterTransform.localScale.x / 2f;
		float rightCharacterLeftSideBounds = rightCharacterTransform.position.x - rightCharacterTransform.localScale.x / 2f;
		float leftCharacterBoundsFinalX = leftCharacterRightSideBounds + leftCharacterVelocity.x;
		float rightCharacterBoundsFinalX = rightCharacterLeftSideBounds + rightCharacterVelocity.x;

		if (rightCharacterBoundsFinalX <= leftCharacterBoundsFinalX) {
			float newMidPoint;
			float distanceBetween = leftCharacterBoundsFinalX - rightCharacterBoundsFinalX;
			float velocitiesRatio = Mathf.Abs(leftCharacterVelocity.x) / 
				(Mathf.Abs(leftCharacterVelocity.x) + Mathf.Abs(rightCharacterVelocity.x));
			if (float.IsNaN (velocitiesRatio)) {
				velocitiesRatio = 0;
				newMidPoint = rightCharacterVelocity.magnitude > leftCharacterVelocity.magnitude ? rightCharacterLeftSideBounds : leftCharacterRightSideBounds;
			} else {
				newMidPoint = rightCharacterBoundsFinalX + (distanceBetween * velocitiesRatio);
			}

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

	public static Vector2 GetLevelConstraintedVelocity (Transform characterTransform, Vector2 characterVelocity) {
		Vector2 newCharacterVelocity;
		float characterVelocityModifier = Mathf.Abs(characterTransform.rotation.y) != 1f ? 1f : -1f;
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

	public static float GetNonOverlappingXVelocity (Transform correctedPlayerTransform, Vector2 correctedPlayerVelocity, Transform otherPlayerTransform, Vector2 otherPlayerVelocity) {
		float correctedPlayerXVelocity = correctedPlayerVelocity.x;
		float otherPlayerXVelocity = otherPlayerVelocity.x;
		if (!VerticalCollisionWillOccur(correctedPlayerTransform, correctedPlayerVelocity, otherPlayerTransform, otherPlayerVelocity)) {
			return float.NaN;
		}
		float correctedPlayerWidthFromOrigin = correctedPlayerTransform.localScale.x / 2f;
		float otherPlayerWidthFromOrigin = otherPlayerTransform.localScale.x / 2f;	
		float minXDistance = correctedPlayerWidthFromOrigin + otherPlayerWidthFromOrigin;
		correctedPlayerXVelocity = GetAbsoluteXVelocity (correctedPlayerTransform, correctedPlayerXVelocity);
		otherPlayerXVelocity = GetAbsoluteXVelocity (otherPlayerTransform, otherPlayerXVelocity);
		float xDistanceAfterMovement = Mathf.Abs(correctedPlayerTransform.position.x + correctedPlayerXVelocity 
			- (otherPlayerTransform.position.x + otherPlayerXVelocity));

		if (minXDistance >= xDistanceAfterMovement) {
			if (correctedPlayerTransform.position.x < 0) {
				float newX = correctedPlayerTransform.position.x + correctedPlayerXVelocity + correctedPlayerWidthFromOrigin + (otherPlayerWidthFromOrigin + bufferValue * 2f);
				float newXVelocity = newX - otherPlayerTransform.position.x;
				return GetAbsoluteXVelocity(otherPlayerTransform, newXVelocity);
			} else {
				float newX = correctedPlayerTransform.position.x + correctedPlayerXVelocity - correctedPlayerWidthFromOrigin - (otherPlayerWidthFromOrigin + bufferValue * 2f);
				float newXVelocity = newX - otherPlayerTransform.position.x;
				return GetAbsoluteXVelocity(otherPlayerTransform, newXVelocity);
			}
		}

		return float.NaN;
	}

	static float GetConstrainedYVelocity(Transform transform, float yVelocity) {
		float height = transform.localScale.y;

		if (transform.position.y - height / 2f + yVelocity < floorBounds) {
			return floorBounds - (transform.position.y - height / 2f);
		}

		return yVelocity;
	}

	static bool VerticalCollisionWillOccur(Transform p1Transform, Vector2 p1Velocity, Transform p2Transform, Vector2 p2Velocity) {
		float minYDistance = p1Transform.localScale.y / 2f + p2Transform.localScale.y / 2f;
		float yDistanceAfterMovement = Mathf.Abs(p1Transform.position.y + p1Velocity.y - (p2Transform.position.y + p2Velocity.y));

		return (yDistanceAfterMovement <= minYDistance);
	}

	static float GetAbsoluteXVelocity(Transform characterTransform, float xVelocity) {
		float modifier = Mathf.Abs(characterTransform.rotation.y) != 1f ? 1f : -1f;

		return xVelocity * modifier;
	}


}


using UnityEngine;
using System.Collections;

public class JumpSequence : IFrameSequence {
	MoveSequence[] supplementaryMove;
	Vector2 velocity;
	double maxJumpHeight;

	public double currentHeight { get; private set; }
	public bool isFalling { get; private set; }

	public JumpSequence(int jumpLengthInFrames, double jumpHeight, double horizontalDistanceCovered) {
		SetUp ();
		this.maxJumpHeight = jumpHeight;
		this.velocity = new Vector2 ((float) horizontalDistanceCovered / jumpLengthInFrames, (float) jumpHeight * 2 / jumpLengthInFrames);
	}

	public MoveFrame GetNext() {
		if (HasNext ()) {
			currentHeight += velocity.y;

			if (currentHeight >= maxJumpHeight) {
				isFalling = true;
				velocity = new Vector2 (velocity.x, -velocity.y);
			}

			return new MoveFrame (velocity, MoveType.AIRBORNE);
		} else {
			throw new System.IndexOutOfRangeException("Current sequence does not have a next move!");
		}

	}

	public bool HasNext() {
		return (!isFalling || currentHeight > 0);
	}

	public int MoveLength () {
		return -1;
	}

	public void Reset () {
		if (isFalling) {
			velocity = new Vector2 (velocity.x, -velocity.y);
		}
		SetUp ();
	}

	public void AddSupplimentaryFrameSequence (IFrameSequence newSequence) {
		return;
	}

	void SetUp () {
		supplementaryMove = null;
		isFalling = false;
		currentHeight = 0;
	}
		
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSequence : IFrameSequence {
	MoveSequence[] supplementaryMove;
	Vector2 velocity;
	double maxJumpHeight;
	Dictionary<string, IFrameSequence> cancellableToDict;

	public double currentHeight { get; private set; }
	public bool isFalling { get; private set; }

	public JumpSequence(int jumpLengthInFrames, double jumpHeight, double horizontalDistanceCovered, Dictionary<string, IFrameSequence> cancellableToDict) {
		SetUp ();
		this.maxJumpHeight = jumpHeight;
		this.velocity = new Vector2 ((float) horizontalDistanceCovered / jumpLengthInFrames, (float) jumpHeight * 2 / jumpLengthInFrames);
		this.cancellableToDict = cancellableToDict;
	}

	public MoveFrame GetNext() {
		if (HasNext ()) {
			currentHeight += velocity.y;

			if (currentHeight >= maxJumpHeight && !isFalling) {
				isFalling = true;
				velocity = new Vector2 (velocity.x, -velocity.y);
			}
			MoveFrame returnFrame = new MoveFrame (velocity, MoveType.AIRBORNE);
			returnFrame.cancellableTo = cancellableToDict;
			return returnFrame;
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

	public MoveFrame Peek () {
		Vector2 nextFrame;

		if (isFalling || currentHeight + velocity.y >= maxJumpHeight) {
			nextFrame = new Vector2 (velocity.x, -velocity.y);
		} else {
			nextFrame = new Vector2 (velocity.x, velocity.y);
		}

		return new MoveFrame (nextFrame, MoveType.AIRBORNE);
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

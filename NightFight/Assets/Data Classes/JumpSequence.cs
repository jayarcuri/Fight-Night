using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSequence : IFrameSequence
{
	MoveSequence[] supplementaryMove;
	Vector2 velocity;
	double maxJumpHeight;
	Dictionary<string, IFrameSequence> cancelsTo;

	public double currentHeight { get; private set; }

	public bool isFalling { get; private set; }

	public JumpSequence (int jumpLengthInFrames, double jumpHeight, double horizontalDistanceCovered, Dictionary<string, IFrameSequence> cancelsTo)
	{
		SetUp ();
		this.maxJumpHeight = jumpHeight;
		this.velocity = new Vector2 ((float)horizontalDistanceCovered / jumpLengthInFrames, (float)jumpHeight * 2 / jumpLengthInFrames);
		this.cancelsTo = cancelsTo;
	}

	public JumpSequence (double maxHeight, double currentHeight, Vector2 velocity, Dictionary<string, IFrameSequence> cancelsTo)
	{
		SetUp ();
		this.maxJumpHeight = maxHeight;
		this.currentHeight = currentHeight;
		this.velocity = velocity;
		this.cancelsTo = cancelsTo;
	}

	public bool HasNext ()
	{
		return (!isFalling || currentHeight > 0);
	}

	public int MoveLength ()
	{
		return -1;
	}

	public void Reset ()
	{
		if (isFalling) {
			velocity = new Vector2 (velocity.x, -velocity.y);
		}
		SetUp ();
	}
		
	public MoveFrame GetNext ()
	{
		if (HasNext ()) {
			currentHeight += velocity.y;

			if (currentHeight >= maxJumpHeight && !isFalling) {
				isFalling = true;
				velocity = new Vector2 (velocity.x, -velocity.y);
			}
			MoveFrame returnFrame = GetNextMoveFrame (velocity);
			return returnFrame;
		} else  {
			throw new System.IndexOutOfRangeException ("Current sequence does not have a next move!");
		}

	}

	public MoveFrame Peek ()
	{
		MoveFrame nextFrame;

		if (isFalling || currentHeight + velocity.y >= maxJumpHeight) {
			nextFrame = GetNextMoveFrame(new Vector2 (velocity.x, -velocity.y));
		} else  {
			nextFrame = GetNextMoveFrame(new Vector2 (velocity.x, velocity.y));
		}
		return nextFrame;
	}

	public void AddSupplimentaryFrameSequence (IFrameSequence newSequence)
	{
	}

	public JumpSequence GetAirRecoverySequence() {
		float verticalVelocity = isFalling ? -velocity.y : velocity.y;
		float horizontalVelocity = velocity.x != 0 ? -Mathf.Abs(velocity.x/2) : -0.1f;
		Vector2 recoveryVelocity = new Vector2 (horizontalVelocity, verticalVelocity);
		return new JumpSequence (currentHeight + (verticalVelocity * 10), currentHeight, recoveryVelocity, new Dictionary<string, IFrameSequence> ());
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {
		return cancelsTo;
	}



	MoveFrame GetNextMoveFrame (Vector2 nextVelocity)
	{
		MoveFrame returnFrame = new MoveFrame (nextVelocity, MoveType.AIRBORNE);
		if (supplementaryMove == null) {
			returnFrame.cancellableTo = cancelsTo;
		}
		return returnFrame;
	}

	void SetUp ()
	{
		supplementaryMove = null;
		isFalling = false;
		currentHeight = 0;
	}
		
}

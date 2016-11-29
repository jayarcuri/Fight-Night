using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSequence : IFrameSequence
{
	IFrameSequence supplementaryMove;
	float horizontalVelocity;
	float maxJumpHeight;
	int jumpLengthInFrames;
	protected int currentFrameCount;
	Dictionary<string, IFrameSequence> cancelsTo;

	public JumpSequence (int jumpLengthInFrames, float jumpHeight, float horizontalDistanceCovered, Dictionary<string, IFrameSequence> cancelsTo)
	{
		Reset ();
		this.maxJumpHeight = jumpHeight;
		this.jumpLengthInFrames = jumpLengthInFrames;
		horizontalVelocity = horizontalDistanceCovered / jumpLengthInFrames;
		this.cancelsTo = cancelsTo;
		currentFrameCount = 0;
	}

	public bool HasNext ()
	{
		return (currentFrameCount < jumpLengthInFrames);
	}

	public int MoveLength ()
	{
		return -1;
	}

	public void Reset ()
	{
		currentFrameCount = 0;
		supplementaryMove = null;
	}
		
	public MoveFrame GetNext ()
	{
		if (HasNext ()) {
			MoveFrame nextFrame = GetNextFrame ();
			IncrementIndex ();
			return nextFrame;
		} else  {
			throw new System.IndexOutOfRangeException ("Current sequence does not have a next move!");
		}

	}

	public MoveFrame Peek ()
	{
		return GetNextFrame ();
	}

	public void AddSupplementaryFrameSequence (IFrameSequence newSequence)
	{
		supplementaryMove = newSequence;
	}

	public JumpSequence GetAirRecoverySequence() {
		int recoverySequenceLength = 15;
		int framesBeforeDescent = 0;
		int spoofLength = (recoverySequenceLength - framesBeforeDescent) * 2;

		float maxHeightForRS = GetCurrentHeight () /*/ Mathf.Sin (11f / 13f * Mathf.PI * 0.5f)*/;		
		float newHorizontalVelocity = /*horizontalVelocity != 0 ? -Mathf.Abs(horizontalVelocity / 2f) :*/ -2.5f;

		JumpSequence recoverySequence = new JumpSequence (recoverySequenceLength * 2, maxHeightForRS, newHorizontalVelocity, new Dictionary<string, IFrameSequence> ());
		recoverySequence.currentFrameCount = spoofLength / 2 - framesBeforeDescent;
		return recoverySequence;
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {
		return cancelsTo;
	}

	public void IncrementIndex () {
		currentFrameCount++;
		if (supplementaryMove != null && supplementaryMove.HasNext ()) {
			supplementaryMove.IncrementIndex ();
		}
	}

	public bool SequenceStartedWithButton(ButtonInput[] engagedButtons) {
		return false;
	}

	MoveFrame GetNextFrame () {
		Vector2 velocity = GetMovementForNextFrame ();

		MoveFrame supplementalFrame = null;
		if (supplementaryMove != null && supplementaryMove.HasNext ()) {
			supplementalFrame = supplementaryMove.Peek ();
		}

		MoveFrame returnFrame = GetCombinedMoveFrame (velocity, supplementalFrame);
		return returnFrame;
	}

	MoveFrame GetCombinedMoveFrame (Vector2 nextVelocity, MoveFrame supplementalFrame)
	{
		MoveFrame returnFrame;
		if (supplementalFrame != null) {
			if (supplementalFrame.attackData != null) {
				AttackFrameData hit = supplementalFrame.attackData;
				// Remove HitFrame construction
				returnFrame = new MoveFrame(nextVelocity + supplementalFrame.movementDuringFrame, 
					MoveType.AIRBORNE, hit);
				returnFrame.cancellableTo = supplementalFrame.cancellableTo;
			} else {
				returnFrame = new MoveFrame (nextVelocity + supplementalFrame.movementDuringFrame, MoveType.AIRBORNE, supplementalFrame.isLit);
			}
		} else {
			returnFrame = new MoveFrame (nextVelocity, MoveType.AIRBORNE);
			returnFrame.cancellableTo = cancelsTo;
		}
		return returnFrame;
	}
		
	JumpSequence (double maxHeight, double currentHeight, Dictionary<string, IFrameSequence> cancelsTo)
	{
		Reset ();
		this.maxJumpHeight = (float)maxHeight;
		this.cancelsTo = cancelsTo;
		currentFrameCount = 0;
	}

	Vector2 GetMovementForNextFrame () {
		float yPercentageNextFrame = Mathf.Sin ((currentFrameCount + 1) / (jumpLengthInFrames / 2f) * Mathf.PI * 0.5f);
		float yPercentageThisFrame = Mathf.Sin ((currentFrameCount) / (jumpLengthInFrames / 2f) * Mathf.PI * 0.5f);

		float percentageDelta = yPercentageNextFrame - yPercentageThisFrame;

		return new Vector2 (horizontalVelocity, maxJumpHeight * percentageDelta); 
	}

	float GetCurrentHeight () {
		return Mathf.Sin ((currentFrameCount) / (jumpLengthInFrames / 2f) * Mathf.PI * 0.5f) * maxJumpHeight;
	}
		
}

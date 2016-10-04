using UnityEngine;
using System.Collections;
using System;

public class CharacterManager
{
	CharacterState characterState;
	CharacterData characterData;

	public IFrameSequence currentMove { get; private set; }

	public CharacterManager ()
	{
		characterData = new CharacterData ();
		characterState = new CharacterState (characterData.maxHealth);

		currentMove = null;
	}

	public void QueueMove (IFrameSequence newMove)
	{
		currentMove = newMove;
		currentMove.Reset ();
	}

	public bool HasQueuedFrames ()
	{
		return currentMove != null && currentMove.HasNext ();
	}

	public int GetStartingHealth ()
	{
		return characterData.maxHealth;
	}

	public MoveFrame GetCurrentFrame (DirectionalInput directionalInput, AttackType attackType)
	{
		IFrameSequence newMove = null;
		IFrameSequence currentMoveSequence = this.currentMove;
		MoveFrame nextFrameToExecute = currentMoveSequence != null && currentMoveSequence.HasNext () ? currentMoveSequence.Peek () : null;
		// ---Attempt to enqueue a new move using input---
		//
		// If there is no current Sequence or it would resolve, give input to neutral state to get new Sequence
		if (currentMoveSequence == null || nextFrameToExecute == null) {
			newMove = characterData.GetNewMove (directionalInput, attackType);
		} 
		// Otherwise, if we currently have a move that will not resolve next frame & exists, try to cancel that 
		// frame to a new Sequence.currentMoveSequence
		else if (currentMoveSequence != null && nextFrameToExecute.cancellableTo != null) {
			newMove = characterData.TryToCancelCurrentMove (nextFrameToExecute, directionalInput, attackType);
		}

		if (newMove != null) {
			if (nextFrameToExecute != null && MoveType.AIRBORNE.Equals (nextFrameToExecute.moveType)) {
				JumpSequence currentJump = (JumpSequence)currentMoveSequence;
				currentJump.AddSupplimentaryFrameSequence (newMove);
			} else {
				QueueMove (newMove);
			}
		}
			
		// Finally, get the next MoveFrame if it exists
		MoveFrame currentMoveFrame = currentMove != null && currentMove.HasNext () ? currentMove.GetNext () : null;

		return currentMoveFrame;
	}
}
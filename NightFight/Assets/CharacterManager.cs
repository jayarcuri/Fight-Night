using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	public bool HasQueuedFrames ()
	{
		return currentMove != null && currentMove.HasNext ();
	}

	public int GetStartingHealth ()
	{
		return characterData.maxHealth;
	}

	public int GetCurrentHealth ()
	{
		return characterState.health;
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
			newMove = GetNewMove (directionalInput, attackType);
		} 
		// Otherwise, if we currently have a move that will not resolve next frame & exists, try to cancel that 
		// frame to a new Sequence.currentMoveSequence
		else if (currentMoveSequence != null && nextFrameToExecute.cancellableTo != null) {
			newMove = TryToCancelCurrentMove (nextFrameToExecute, directionalInput, attackType);
		}

		if (newMove != null) {
			if (nextFrameToExecute != null && MoveType.AIRBORNE.Equals (nextFrameToExecute.moveType)) {
				JumpSequence currentJump = (JumpSequence)currentMoveSequence;
				currentJump.AddSupplimentaryFrameSequence (newMove);
			} else  {
				QueueMove (newMove);
			}
		}
			
		// Finally, get the next MoveFrame if it exists
		MoveFrame currentMoveFrame = currentMove != null && currentMove.HasNext () ? currentMove.GetNext () : null;

		return currentMoveFrame;
	}

	public bool ProcessHitFrame (HitFrame hit, MoveFrame previousFrame)
	{
		IFrameSequence currentMoveSequence = this.currentMove;
		MoveFrame nextFrameToExecute = previousFrame;
		Debug.Log ("next frame is null: " + previousFrame == null);
		MoveType currentMoveType = nextFrameToExecute != null ? nextFrameToExecute.moveType : MoveType.NONE;
		Debug.Log ("next frame is of type: " + currentMoveType);
		Dictionary<string, IFrameSequence> optionDictionary = nextFrameToExecute != null ? 
			nextFrameToExecute.cancellableTo : 
			characterData.neutralMoveOptions;

		if (MoveType.ACTIVE.Equals(hit.moveType) && optionDictionary.ContainsKey ("HIT")) {
			if (currentMoveType == MoveType.AIRBORNE) {
				JumpSequence currentJumpSequence = (JumpSequence)currentMoveSequence;
				JumpSequence recoverySequence = currentJumpSequence.GetAirRecoverySequence ();
				QueueMoveWithoutReset (recoverySequence);
				characterState.TakeDamage (hit.damage);
			} else if (currentMoveType == MoveType.BLOCKING) {
				QueueMove (hit.blockStunFrames);
			} else  {
				QueueMove (hit.hitStunFrames);
				characterState.TakeDamage (hit.damage);
			}
			return true;
		} else if (MoveType.THROW.Equals(hit.moveType) && optionDictionary.ContainsKey ("THROW")) {
			Debug.Log ("Throw triggered");
			QueueMove (hit.hitStunFrames);
			characterState.TakeDamage (hit.damage);
			return true;
		} else {
			return false;
		}
	}

	public IFrameSequence TryToCancelCurrentMove (MoveFrame currentFrame, DirectionalInput directionalInput, AttackType attack)
	{
		IFrameSequence newMove = GetSequenceFromDictionary (currentFrame.cancellableTo, directionalInput, attack);
		return newMove;
	}

	public IFrameSequence GetNewMove (DirectionalInput directionalInput, AttackType attack)
	{
		IFrameSequence newMove = GetSequenceFromDictionary (characterData.neutralMoveOptions, directionalInput, attack);
		return newMove;
	}

	IFrameSequence GetSequenceFromDictionary (Dictionary<string, IFrameSequence> optionDictionary,
	                                          DirectionalInput directionalInput, AttackType attack)
	{
		int intInput = directionalInput.numpadValue;
		IFrameSequence newMove = null;
		bool hasValue = false;
		if (intInput != 5 || AttackType.None != attack)
			//Debug.Log ("Directional input: " + intInput + "\nAttack input: " + (char)attack);
		// Test input in order of what we've defined to be the "priority" of input
		// 1. Can I jump?
		if (intInput >= 7) {
			//Debug.Log ("Should jump.");
			hasValue = optionDictionary.TryGetValue (intInput.ToString (), out newMove);
			if (hasValue) {
				newMove.Reset ();
				return newMove;
			}
		}
		// 2. Can I attack?
		if (!AttackType.None.Equals (attack)) {
			string attackEnumString = ((char)attack).ToString ();
			//Debug.Log ("Should attack with " + attackEnumString);
			hasValue = optionDictionary.TryGetValue (attackEnumString, out newMove);
			if (hasValue) {
				//Debug.Log ("has value");
				newMove.Reset ();
				return newMove;
			}
		}
		// 3. Lowest priority) Can I move?
		hasValue = optionDictionary.TryGetValue (intInput.ToString (), out newMove);
		if (hasValue) {
			newMove.Reset ();
			return newMove;
		}

		return newMove;
	}

	void QueueMove (IFrameSequence newMove)
	{
		currentMove = newMove;
		currentMove.Reset ();
	}

	void QueueMoveWithoutReset(IFrameSequence newMove) {
		currentMove = newMove;
	}
}
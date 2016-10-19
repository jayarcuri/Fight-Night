using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterManager
{
	CharacterData characterData;
	int currentHealth;
	bool isSelfIlluminated;
	public IFrameSequence currentMove { get; private set; }

	public CharacterManager ()
	{
		characterData = new CharacterData ();
		this.currentHealth = characterData.maxHealth;
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
		return currentHealth;
	}

	public MoveFrame GetCurrentFrame (DirectionalInput directionalInput, AttackType attackType)
	{
		IFrameSequence newMove = null;
		IFrameSequence currentMoveSequence = this.currentMove;
		MoveFrame nextFrameToExecute = currentMoveSequence != null && currentMoveSequence.HasNext () ? currentMoveSequence.Peek () : null;
		MoveFrame returnFrame;
		// ---Attempt to enqueue a new move using input---
		//
		// If there is no current Sequence or it would resolve, give input to neutral state to get new Sequence
		if (currentMoveSequence == null || nextFrameToExecute == null) {
			newMove = GetNewMove (directionalInput, attackType);
		} 
		// Otherwise, if we currently have a move that will not resolve next frame & exists, try to cancel that 
		// frame to a new Sequence.currentMoveSequence
		else if (currentMoveSequence != null && nextFrameToExecute.cancellableTo != null) {
			newMove = GetSequenceFromCancelDictionary (nextFrameToExecute.cancellableTo, directionalInput, attackType);
		}

		if (newMove != null) {
			if (nextFrameToExecute != null && MoveType.AIRBORNE.Equals (nextFrameToExecute.moveType)) {
				JumpSequence currentJump = (JumpSequence)currentMoveSequence;
				currentJump.AddSupplementaryFrameSequence (newMove);
			} else {
				QueueMove (newMove);
			}
			returnFrame = currentMove.GetNext ();
		} else if (nextFrameToExecute != null) {
			this.currentMove.IncrementIndex ();
			returnFrame = nextFrameToExecute;
		} else {
			returnFrame = characterData.GetEmptyMoveFrame ();
		}

		if (isSelfIlluminated) {
			returnFrame.isLit = true;
		}

		return returnFrame;
	}

	public bool ProcessHitFrame (AttackFrameData hit, MoveFrame previousFrame)
	{
		IFrameSequence currentMoveSequence = this.currentMove;
		MoveType previousMoveType = previousFrame != null ? previousFrame.moveType : MoveType.NONE;
		Dictionary<string, IFrameSequence> optionDictionary = previousFrame != null ? 
			previousFrame.cancellableTo : 
			characterData.neutralMoveOptions;

		if (HitType.HIT == hit.hitType && optionDictionary.ContainsKey ("HIT")) {
			if (previousMoveType == MoveType.AIRBORNE) {
				JumpSequence currentJumpSequence = (JumpSequence)currentMoveSequence;
				JumpSequence recoverySequence = currentJumpSequence.GetAirRecoverySequence ();
				QueueMoveWithoutReset (recoverySequence);
				TakeDamage (hit.damage);
			} else if (previousMoveType == MoveType.BLOCKING) {
				QueueMove (hit.blockStunFrames);
			} else  {
				QueueMove (hit.hitStunFrames);
				TakeDamage (hit.damage);
			}
			return true;
		} else if (HitType.THROW == hit.hitType && optionDictionary.ContainsKey ("HIT")) {
			Debug.Log ("Throw triggered");
			QueueMove (hit.blockStunFrames);
			TakeDamage (hit.damage);
			return true;
		} else if (HitType.HIT == hit.hitType && previousMoveType == MoveType.BLOCKING) {
			QueueMove (hit.blockStunFrames);
			return true;
		}
			return false;
	}

	public IFrameSequence GetNewMove (DirectionalInput directionalInput, AttackType attack)
	{
		IFrameSequence newMove = GetSequenceFromCancelDictionary (characterData.neutralMoveOptions, directionalInput, attack);
		return newMove;
	}

	public IFrameSequence GetSequenceFromCancelDictionary (Dictionary<string, IFrameSequence> optionDictionary,
	                                          DirectionalInput directionalInput, AttackType attack)
	{
		int intInput = directionalInput.numpadValue;
		IFrameSequence newMove = null;
		bool hasValue = false;
		if (intInput != 5 || AttackType.None != attack)
		// Test input in order of what we've defined to be the "priority" of input
		// 1. Can I jump?
		if (intInput >= 7) {
			hasValue = optionDictionary.TryGetValue (intInput.ToString (), out newMove);
			if (hasValue) {
				newMove.Reset ();
				return newMove;
			}
		}
		// 2. Can I attack?
		if (!AttackType.None.Equals (attack)) {
			string attackEnumString = ((char)attack).ToString ();
			hasValue = optionDictionary.TryGetValue (attackEnumString, out newMove);
			if (hasValue) {
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

	public void ToggleCharacterIllumination () {
		isSelfIlluminated = !isSelfIlluminated;
	}

	void QueueMove (IFrameSequence newMove)
	{
		currentMove = newMove;
		currentMove.Reset ();
	}

	void QueueMoveWithoutReset(IFrameSequence newMove) {
		currentMove = newMove;
	}

	void TakeDamage(int damage) {
		currentHealth -= damage;
	}
}
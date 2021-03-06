﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterDataManager
{
	CharacterData characterData;
	MoveBufferManager moveBufferManager;
	int currentHealth;
	public bool isSelfIlluminated { get; private set; }
	public bool isCarryingOrb = false;
	public IFrameSequence currentMove { get; private set; }

	public CharacterDataManager ()
	{
		this.characterData = new CharacterData ();
		this.moveBufferManager = characterData.GetMoveBufferManager ();
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

	public MoveFrame GetCurrentFrame (DirectionalInput directionalInput, ButtonInputCommand ButtonInputCommand, out bool isLit)
	{
		IFrameSequence newMove = null;
		IFrameSequence currentMoveSequence = this.currentMove;
		MoveFrame nextFrameToExecute = currentMoveSequence != null && currentMoveSequence.HasNext () ? currentMoveSequence.Peek () : null;
		MoveFrame returnFrame;
		// ---Attempt to enqueue a new move using input---
		//
		// If there is no current Sequence or it would resolve, give input to neutral state to get new Sequence
		if (nextFrameToExecute == null) {
			newMove = GetNewMove (directionalInput, ButtonInputCommand);
		} 
		// Otherwise, if we currently have a move that will not resolve next frame & exists, try to cancel that 
		// frame to a new Sequence.currentMoveSequence
		else if (currentMoveSequence != null && nextFrameToExecute.cancellableTo != null) {
			newMove = GetSequenceFromCancelDictionary (nextFrameToExecute.cancellableTo, directionalInput, ButtonInputCommand);
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

		isLit = isSelfIlluminated || returnFrame.isLit || isCarryingOrb;

		return returnFrame;
	}

	public bool ProcessHitFrame (AttackFrameData hit, MoveFrame previousFrame)
	{
		MoveType previousMoveType = previousFrame != null ? previousFrame.moveType : MoveType.NONE;
		Dictionary<string, IFrameSequence> optionDictionary;

		if (previousFrame != null) {
			optionDictionary = previousFrame.cancellableTo;
		} else {
			optionDictionary = !isCarryingOrb ? characterData.defaultNeutralMovesDict : characterData.orbCarryMovesDict;
		}

		if (HitType.HIT == hit.hitType && optionDictionary.ContainsKey ("HIT")) {
			if (previousMoveType == MoveType.AIRBORNE) {
				JumpSequence currentJumpSequence = (JumpSequence)this.currentMove;
				JumpSequence recoverySequence = currentJumpSequence.GetAirRecoverySequence ();
				QueueMoveWithoutReset (recoverySequence);
				TakeDamage (hit.damage);
			} else if (previousMoveType == MoveType.BLOCKING) {
				QueueMove (hit.blockStunSequence);
			} else  {
				QueueMove (hit.hitStunSequence);
				TakeDamage (hit.damage);
			}
			return true;
		} else if (HitType.THROW == hit.hitType && optionDictionary.ContainsKey ("HIT")) {
			Debug.Log ("Throw triggered");
			QueueMove (hit.blockStunSequence);
			TakeDamage (hit.damage);
			return true;
		} else if (HitType.HIT == hit.hitType && previousMoveType == MoveType.BLOCKING) {
			QueueMove (hit.blockStunSequence);
			return true;
		}
			return false;
	}

	public IFrameSequence GetNewMove (DirectionalInput directionalInput, ButtonInputCommand attack)
	{
		Dictionary<string, IFrameSequence> optionDictionary = !isCarryingOrb ? characterData.defaultNeutralMovesDict : characterData.orbCarryMovesDict;
		IFrameSequence newMove = GetSequenceFromCancelDictionary (optionDictionary, directionalInput, attack);
		return newMove;
	}

	public IFrameSequence GetSequenceFromCancelDictionary (Dictionary<string, IFrameSequence> optionDictionary,
	                                          DirectionalInput directionalInput, ButtonInputCommand attack)
	{
		int intInput = directionalInput.numpadValue;
		IFrameSequence newMove = null;
		bool hasValue = false;

		//	Check if any special moves are currently executable, & if those special moves are currently in the cancel dictionary
		List<string> executableSpecialMoves = GetMoveCodesForReadyBufferMoves(directionalInput, attack);
		if (executableSpecialMoves.Count > 0) {
			foreach (string moveCode in executableSpecialMoves) {
				if (optionDictionary.TryGetValue (moveCode, out newMove)) {
					newMove.Reset ();
					ResetMoveBuffer (moveCode);
					return newMove;
				}
			}
		}

		if (intInput != 5 || ButtonInputCommand.NONE != attack)
		// Test input in order of what we've defined to be the "priority" of input
		// 1. Can I jump?
		if (intInput >= 7) {
			if (optionDictionary.TryGetValue (intInput.ToString (), out newMove)) {
				newMove.Reset ();
				return newMove;
			}
		}
		// 2. Can I attack?
		if (!ButtonInputCommand.NONE.Equals (attack)) {
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

	public List<string> GetMoveCodesForReadyBufferMoves(DirectionalInput currentDirectionalInput, ButtonInputCommand currentButton) {
		return moveBufferManager.GetReadiedBufferMove (currentDirectionalInput, currentButton);
	}

	public void ResetMoveBuffer(string forMove) {
		moveBufferManager.ResetMoveBuffer (forMove);
	}

	public void ToggleCharacterIllumination () {
		isSelfIlluminated = !isSelfIlluminated;
	}

	public bool IsInIlluminatedState () {
		return isCarryingOrb || isSelfIlluminated;
	}

	public void SetWalkSpeed(float newWalkSpeed) {
		characterData.SetWalkSpeed (newWalkSpeed);
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
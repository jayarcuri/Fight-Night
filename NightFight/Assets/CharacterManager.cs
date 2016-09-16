using UnityEngine;
using System.Collections;
using System;

public class CharacterManager {
	CharacterState characterState;
	CharacterData characterData;
	public MoveSequence currentMove {get; private set;}

	public CharacterManager() {
		characterData = new CharacterData ();
		characterState = new CharacterState (characterData.maxHealth);

		currentMove = null;
	}

	public MoveFrame GetCurrentFrame(DirectionalInput directionalInput, AttackType attackType, bool isFacingRight) {
		MoveFrame currentMoveFrame;

		if (!isFacingRight) {
			directionalInput.FlipHorizontalInput ();
		}

		ResolveInput (directionalInput, attackType);

		currentMoveFrame = currentMove != null && currentMove.HasNext () ? currentMove.GetNext () : null;

		return currentMoveFrame;
	}

	public bool HasQueuedFrames() {
		return currentMove != null && currentMove.HasNext ();
	}

	public void QueueMove(MoveSequence newMove) {
		currentMove = newMove;
		currentMove.Reset ();
	}

	public int GetStartingHealth() {
		return characterData.maxHealth;
	}

	void ResolveInput(DirectionalInput directionalInput, AttackType attackType) {
		MoveFrame currentFrame;
		if (currentMove != null && currentMove.HasNext ()) {
				currentFrame = currentMove.Peek ();
		} else {
			currentFrame = null;
		}
		MoveSequence newMove = characterData.GetNewMove (characterState.GetCurrentAction(), currentFrame, directionalInput, attackType);

		if (newMove != null) {
			QueueMove (newMove);
		}
	}



}

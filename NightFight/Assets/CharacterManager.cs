using UnityEngine;
using System.Collections;
using System;

public class CharacterManager {
	CharacterState characterState;
	CharacterData characterData;
	public IFrameSequence currentMove {get; private set;}

	public CharacterManager() {
		characterData = new CharacterData ();
		characterState = new CharacterState (characterData.maxHealth);

		currentMove = null;
	}

	public MoveFrame GetCurrentFrame(DirectionalInput directionalInput, AttackType attackType) {
		MoveFrame currentMoveFrame;

		ResolveInput (directionalInput, attackType);

		currentMoveFrame = currentMove != null && currentMove.HasNext () ? currentMove.GetNext () : null;

		if (currentMoveFrame != null) {
		//	Debug.Log(currentMoveFrame.moveType);
		}

		return currentMoveFrame;
	}

	public bool HasQueuedFrames() {
		return currentMove != null && currentMove.HasNext ();
	}

	public void QueueMove(IFrameSequence newMove) {
		currentMove = newMove;
		currentMove.Reset ();
	}

	public int GetStartingHealth() {
		return characterData.maxHealth;
	}

	void ResolveInput(DirectionalInput directionalInput, AttackType attackType) {
		MoveFrame currentFrame;
		IFrameSequence newMove = null;
		if (currentMove == null || !currentMove.HasNext ()) {
			newMove = characterData.GetNewMove (characterState.GetCurrentAction (), directionalInput, attackType);
		}
		if (newMove != null) {
			QueueMove (newMove);
		}
	}



}

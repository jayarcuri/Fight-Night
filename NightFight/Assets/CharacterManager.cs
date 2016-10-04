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

	public void QueueMove(IFrameSequence newMove) {
		currentMove = newMove;
		currentMove.Reset ();
	}

	public bool HasQueuedFrames() {
		return currentMove != null && currentMove.HasNext ();
	}

	public int GetStartingHealth() {
		return characterData.maxHealth;
	}

	public MoveFrame GetCurrentFrame(DirectionalInput directionalInput, AttackType attackType) {
		MoveFrame currentMoveFrame;

		ResolveInput (directionalInput, attackType);

		currentMoveFrame = currentMove != null && currentMove.HasNext () ? currentMove.GetNext () : null;

		return currentMoveFrame;
	}

	void ResolveInput(DirectionalInput directionalInput, AttackType attackType) {
		IFrameSequence newMove = null;
		if (this.currentMove == null || !this.currentMove.HasNext ()) {
			newMove = characterData.GetNewMove (characterState.GetCurrentAction (), directionalInput, attackType);
		}
		if (this.currentMove != null && this.currentMove.HasNext ()) {
			MoveFrame nextFrameToExecute = this.currentMove.Peek ();
			if (nextFrameToExecute.cancellableTo != null) {
				newMove = characterData.GetNewMove (nextFrameToExecute, directionalInput, attackType);
			}
		}
		
		if (newMove != null) {
			QueueMove (newMove);
		}
	}

}
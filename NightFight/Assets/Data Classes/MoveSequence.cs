using UnityEngine;
using System.Collections;

public class MoveSequence : IFrameSequence {
	int index;
	MoveFrame[] moveSequence;
	ButtonInputCommand startedByButton;

	public MoveSequence(MoveFrame[] moveSequence, ButtonInputCommand startedByButton) {
		index = -1;
		this.moveSequence = moveSequence;
		this.startedByButton = startedByButton;
	}

	public static MoveSequence GetAttackSequenceWithFrameData (int startUp, int activeFrames, int recoveryFrames, AttackFrameData hitData, ButtonInputCommand mappedToButton, bool canBeExtended) {
		MoveSequence newMoveSequence = new MoveSequence (new MoveFrame[0], mappedToButton);
		newMoveSequence.PopulateMoveSequenceUsingFrameData (startUp, activeFrames, recoveryFrames, hitData, canBeExtended);

		return newMoveSequence;
	}

	public virtual MoveFrame GetNext () {
		if(HasNext()) {
			index++;
			return moveSequence [index];
			}
		else
			throw new UnityException("Current move sequence does not have a next move!");
		}

	public virtual MoveFrame Peek () {
		if (HasNext()) {
			return moveSequence [index + 1];
		} else {
			throw new System.IndexOutOfRangeException ("Current move sequence does not have a next move!");
		}
	}

	public virtual bool HasNext() {
		return (index < moveSequence.Length - 1);
	}

	public virtual MoveFrame GetPrevious() {
		MoveFrame returnMove;

		if (index == -1)
			returnMove = null;
		else
			returnMove = moveSequence [index - 1];
		return returnMove;
	}
	// appears to exist only to test; consider deleting
	public virtual int MoveLength () {
		return moveSequence.Length;
	}

	public virtual void Reset() {
		index = -1;
	}

	public void IncrementIndex () {
		index++;
	}

	public bool SequenceStartedWithButton(ButtonInput[] engagedButtons) {
		foreach (ButtonInput input in engagedButtons) {
			if (input.buttonType == startedByButton) {
				return true;
			}
		}
		return false;
	}

	protected void PopulateMoveSequenceUsingFrameData (int startUp, int activeFrames, int recoveryFrames, AttackFrameData hitData, bool hitboxMayBeExtended) {
		MoveFrame neutralFrame = MoveFrame.GetEmptyLitFrame ();
		MoveFrame attackFrame = new MoveFrame (Vector2.zero, MoveType.NONE, hitData);
		moveSequence = new MoveFrame[startUp + activeFrames + recoveryFrames - 1];

		for (int i = 0; i < startUp - 1; i++) {
			moveSequence [i] = neutralFrame;
		}
		for (int j = startUp - 1; j < startUp + activeFrames - 1; j++) {
			moveSequence [j] = attackFrame;
			if (j == startUp + activeFrames - 2 && hitboxMayBeExtended) {
				MoveFrame extendoFrame = attackFrame.CloneMoveFrame ();
				extendoFrame.canBeExtended = true;
				moveSequence [j] = extendoFrame;
			}
		}
		for (int k = startUp + activeFrames - 1; k < moveSequence.Length; k++) {
			moveSequence [k] = neutralFrame;
		}
	}

}

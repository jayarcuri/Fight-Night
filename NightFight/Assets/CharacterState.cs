using UnityEngine;

public class CharacterState {
	protected float health = 100f;
	protected float charge = 0f;
	public bool defaultOrientation;
	protected CharacterAction action = CharacterAction.Standing;
	protected MovementDirection moveDirection = MovementDirection.None;

	public MoveSequence currentMoveSequence;
	public MoveFrame currentFrame;
	protected CharacterData characterData;
	protected HitboxController hitBox;

	public CharacterState () {
		characterData = new CharacterData (this);
	}

	public virtual bool CanAct() {
		return (action != CharacterAction.BlockStunned || action != CharacterAction.HitStunned);
	}

	public virtual void SetCurrentMove(MoveSequence newMoves) {
		hitBox.Reset ();
		currentMoveSequence = newMoves;
	}

	public virtual CharacterAction GetCurrentAction () {
		return action;
	}

	public virtual MoveFrame GetCurrentFrame () {
		return currentFrame;
	}

	public virtual void ExecuteInput(DirectionalInput directionalInput, AttackType attackType) {
		int intInput = (int)directionalInput;
		// Correct DirectionalInput based on if character is oriented correctly
		if (!defaultOrientation) {
			if (intInput % 3 == 1) {
				intInput += 2;
			} else if (intInput % 3 == 0) {
				intInput -= 2;
			}
		}
		// Calls CharacterData to return a move if it is possible 
		directionalInput = (DirectionalInput)intInput;
		if (currentMoveSequence != null) {
			MoveFrame nextMove = currentMoveSequence.peek ();
			characterData.ReadInput (action, nextMove, directionalInput, attackType);
		}
	}

	void ExecuteNextMoveFrame() {
		MoveFrame lastFrame = currentFrame;
		currentFrame = currentMoveSequence.getNext ();
		if (currentFrame.moveType == MoveType.ACTIVE) {
			HitFrame attackFrame = (HitFrame)currentFrame;
			hitBox.ExecuteAttack (attackFrame.offset, attackFrame.size, attackFrame);
		}
		else if (lastFrame != null) {
			if (lastFrame.moveType == MoveType.ACTIVE && currentFrame.moveType != MoveType.ACTIVE)
				hitBox.Reset ();
		}
		if (!currentMoveSequence.hasNext())
			currentMoveSequence = null;
	}
}

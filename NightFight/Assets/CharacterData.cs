using UnityEngine;

public class CharacterData {
	protected CharacterState characterState;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
	protected SpecialMove fireBall;
	protected MoveSequence jab;
	protected MoveSequence AA;
	protected MoveSequence forwardStep;
	protected MoveSequence backwardStep;

	public CharacterData () {
		jabHitbox = new HitFrame (new Vector3 (0.8f, 0.2f, 0f), 
			new Vector3 (.7f, .25f, 1f), Vector3.zero, 1f, 7, 6, MoveType.ACTIVE);
		jab = new MoveSequence (new MoveFrame[]{
			new MoveFrame (), 
			new MoveFrame (),
			jabHitbox,
			jabHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		});
		AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4f, 11, 7, MoveType.ACTIVE);
		AA = new MoveSequence (new MoveFrame[] {
			new MoveFrame (), 
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			AAHitbox,
			AAHitbox,
			AAHitbox,
			AAHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		});
		fireBall = new SpecialMove (
			new DirectionalInput[] { DirectionalInput.Down, DirectionalInput.DownRight, DirectionalInput.Right },
			AA);
	}
	public virtual MoveSequence GetMidAttack () {
		return null;
	}
	public virtual MoveSequence GetHeavyAttack () {
		return null;
	}
	// Fix arguments
	public virtual MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		return null;
	}

	public virtual void ReadInput (CharacterAction action, MoveFrame moveFrame, DirectionalInput dInput, AttackType attack) {
		int intInput = (int)dInput;
		switch (action) {
		case CharacterAction.Standing:
			if (attack == AttackType.Light) {
				characterState.SetCurrentMove (jab);
			} else if (intInput == 4)
				characterState.SetCurrentMove (backwardStep);
			else if (intInput == 6)
				characterState.SetCurrentMove (forwardStep);
			// Add to appropriate move buffers
			bool ready = fireBall.ReadyMove (dInput);
			if (ready)
				characterState.SetCurrentMove (fireBall.GetSpecialMove (attack));
			break;
		case CharacterAction.Jumping:
			// Get jumping frames
			// Add hitboxes and hitframes to the frames for which it matters.
			break;
		}
	}
}
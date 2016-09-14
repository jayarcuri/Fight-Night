using UnityEngine;

public class CharacterData {
	public readonly int maxHealth = 100;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
	protected SpecialMove fireBall;
	protected MoveSequence jab;
	protected MoveSequence AA;
	protected MoveSequence forwardStep;
	protected MoveSequence backwardStep;

	public CharacterData () {
		
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
		forwardStep = new MoveSequence(new MoveFrame[] {new MoveFrame(MoveType.STEP_FORWARD)});
		backwardStep = new MoveSequence(new MoveFrame[] {new MoveFrame(MoveType.STEP_BACK)});
	}
	public virtual MoveSequence GetLightAttack() {
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

		return jab;
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

	public virtual MoveSequence GetNewMove (CharacterAction action, MoveFrame moveFrame, DirectionalInput dInput, AttackType attack) {
		int intInput = (int)dInput;
		MoveSequence newMove = null;
		 // TODO: set up assignment which makes sense in context of current architecture
//		switch (action) {
//		case CharacterAction.Standing:
		if (CharacterAction.Standing.Equals(action)) {
			if (attack == AttackType.Light) {
				newMove = GetLightAttack();
				Debug.Log ("Light attack added");
			} else if (intInput == 4)
				newMove = GetBackwardStep();
			else if (intInput == 6)
				newMove = GetForwardStep();
			// Add to appropriate move buffers
			bool ready = fireBall.ReadyMove (dInput);
			if (ready)
				newMove = fireBall.GetSpecialMove (attack);
	}
		//			break;
			/*
		case CharacterAction.Jumping:
			// Get jumping frames
			// Add hitboxes and hitframes to the frames for which it matters.
			break;*/
//		}

		return newMove;
	}

	MoveSequence GetForwardStep() {
		return new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_FORWARD) });
	}

	MoveSequence GetBackwardStep() {
		return new MoveSequence(new MoveFrame[] {new MoveFrame(MoveType.STEP_BACK)});
	}
}
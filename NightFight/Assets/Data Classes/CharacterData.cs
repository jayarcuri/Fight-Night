using UnityEngine;

public class CharacterData {
	public readonly int maxHealth = 25;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
//	protected SpecialMove fireBall;
	protected MoveSequence jab;
	protected MoveSequence AA;
	protected MoveSequence forwardStep;
	protected MoveSequence backwardStep;

	public CharacterData () {
//		fireBall = null;
//			new SpecialMove (
//			new DirectionalInput[] { DirectionalInput.Down, DirectionalInput.DownRight, DirectionalInput.Right },
//			AA);
		jabHitbox = new HitFrame (new Vector3 (0.8f, 0.2f, 0f), 
			new Vector3 (.7f, .25f, 1f), Vector3.zero, 1, 7, 6, MoveType.ACTIVE);
		jab = new MoveSequence (new MoveFrame[]{
			new MoveFrame (), 
			new MoveFrame (),
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		});

		AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4, 11, 7, MoveType.ACTIVE);
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
	
		forwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_FORWARD) });
		backwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_BACK) });
	}
	// Fix arguments
	public virtual MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		return null;
	}

	public virtual MoveSequence GetNewMove (CharacterAction action, MoveFrame moveFrame, DirectionalInput dInput, AttackType attack) {
		int intInput = dInput.GetNumpadNotation();
		MoveSequence newMove = null;
		 // TODO: set up assignment which makes sense in context of current architecture
		if (CharacterAction.Standing.Equals(action) && moveFrame == null) {
			if (attack == AttackType.Light) {
				newMove = GetLightAttack();
			} else if (intInput == 4)
				newMove = GetBackwardStep();
			else if (intInput == 6)
				newMove = GetForwardStep();
			
//			// TODO: implement special moves
//			bool ready = fireBall.ReadyMove (dInput);
//			if (ready)
//				newMove = fireBall.GetSpecialMove (attack);
	}
		if (newMove != null) {
			// DO NOT DELETE THIS. This line ensures that a MoveSequence can be used more than once.
			newMove.Reset();
		}
		return newMove;
	}

	protected MoveSequence GetForwardStep() {
		Debug.Log ("Forward step added.");
		return forwardStep;
	}

	protected MoveSequence GetBackwardStep() {
		Debug.Log ("Back step added.");
		return backwardStep;
	}

	protected virtual MoveSequence GetLightAttack() {
		return jab;
	}

	protected virtual MoveSequence GetMidAttack () {
		return null;
	}
	protected virtual MoveSequence GetHeavyAttack () {
		return AA;
	}
}
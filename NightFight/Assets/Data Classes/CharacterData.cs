using UnityEngine;

public class CharacterData {
	public readonly int maxHealth = 3;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
//	protected SpecialMove fireBall;
	protected IFrameSequence jab;
	protected IFrameSequence AA;
	protected IFrameSequence forwardStep;
	protected IFrameSequence backwardStep;
	protected IFrameSequence block;
	IFrameSequence verticalJump;
	IFrameSequence forwardJump;
	IFrameSequence backwardJump;

	public CharacterData () {
//		fireBall = null;
//			new SpecialMove (
//			new DirectionalInput[] { DirectionalInput.Down, DirectionalInput.DownRight, DirectionalInput.Right },
//			AA);
		verticalJump = new JumpSequence (40, 3.5, 0.0);
		forwardJump = new JumpSequence (40, 3.5, 2.5);
		backwardJump = new JumpSequence (40, 3.5, -2.5);

		jabHitbox = new HitFrame (new Vector2 (0.8f, 0.2f), 
			new Vector3 (.7f, .25f, 1f), Vector2.zero, 1, 7, 6, MoveType.ACTIVE);
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

		AAHitbox = new HitFrame (new Vector2 (0.6f, 0.4f), new Vector3 (.7f, .5f, 1f), Vector2.zero, 4, 11, 7, MoveType.ACTIVE);
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

		block = new MoveSequence (new MoveFrame[] {
			new MoveFrame(MoveType.BLOCKING)
			}
		);
	
		forwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_FORWARD) });
		backwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_BACK) });
	}
	// Fix arguments
	public virtual MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		return null;
	}

	// TODO: replace with check against hashmap implementation of a FSM
	public virtual IFrameSequence GetNewMove (CharacterAction action, DirectionalInput dInput, AttackType attack) {
		int intInput = dInput.GetNumpadNotation();
		IFrameSequence newMove = null;
		 // TODO: set up assignment which makes sense in context of current architecture
		if (CharacterAction.Standing.Equals(action)) {
			if (attack == AttackType.Light) {
				newMove = GetLightAttack ();
			} else if (attack == AttackType.Block) {
				newMove = block;
			} else if (intInput == 4) {
				newMove = GetBackwardStep ();
			} else if (intInput == 6)
				newMove = GetForwardStep ();
			else if (intInput == 7) {
				newMove = backwardJump;
			} else if (intInput == 8) {
				newMove = verticalJump;
			} else if (intInput == 9) {
				newMove = forwardJump;
			}
				
			
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

	protected IFrameSequence GetForwardStep() {
		return forwardStep;
	}

	protected IFrameSequence GetBackwardStep() {
		return backwardStep;
	}

	protected virtual IFrameSequence GetLightAttack() {
		return jab;
	}

	protected virtual IFrameSequence GetMidAttack () {
		return null;
	}
	protected virtual IFrameSequence GetHeavyAttack () {
		return AA;
	}
}
using UnityEngine;
using System.Collections.Generic;

public class CharacterData {
	public readonly int maxHealth = 25;
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
		MoveFrame neutralFrame = MoveFrame.GetLitMoveFrame ();
		Dictionary<string, IFrameSequence> cancelsForJump = new Dictionary<string, IFrameSequence> ();
		MoveSequence jumpAttack = new MoveSequence (new MoveFrame[] {
			neutralFrame
		});
		cancelsForJump.Add ("L", jumpAttack);
		verticalJump = new JumpSequence (40, 3.5, 0.0, cancelsForJump);
		forwardJump = new JumpSequence (40, 3.5, 2.5, cancelsForJump);
		backwardJump = new JumpSequence (40, 3.5, -2.5, cancelsForJump);

		jabHitbox = new HitFrame (new Vector2 (0.8f, 0.2f), 
			new Vector3 (.7f, .25f, 1f), Vector2.zero, 1, 7, 6, MoveType.ACTIVE);
		jab = new MoveSequence (new MoveFrame[]{
			neutralFrame, 
			neutralFrame,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
		});

		AAHitbox = new HitFrame (new Vector2 (0.6f, 0.6f), new Vector3 (.7f, .8f, 1f), Vector2.zero, 4, 11, 7, MoveType.ACTIVE);
		AA = new MoveSequence (new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			neutralFrame,
			neutralFrame,
			AAHitbox,
			AAHitbox,
			AAHitbox,
			AAHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
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
		int intInput = dInput.numpadValue;
		IFrameSequence newMove = null;
		// Get current frame

		 
		// 1. feed input into special move camp; see if anything sticks
		//		i. if a special move is able to be executed & a button is being pressed, get that special move 

		//			TODO: implement special moves
		//			bool ready = fireBall.ReadyMove (dInput);
		//			if (ready) {
		//				newMove = fireBall.GetSpecialMove (attack);
		// 			}

		// 2. proceed thru remaining order and attempt to grab a value
		if (intInput >= 7) {
			// newMove = hashmap.Get(intInput)
			// if newMove != null
			// 		goto end
		}
		if (!AttackType.None.Equals (attack)) {
			// newMove = hashmap.Get(attack);
			// if newMove != null
			// 		goto end
		}

//		if (newMove
		// newMove = hashmap.Get(intInput);
		// if newMove != null
		// 		goto end

		if (CharacterAction.Standing.Equals (action)) {
			if (attack == AttackType.Light) {
				newMove = GetLightAttack ();
			} else if (attack == AttackType.Block) {
				newMove = GetHeavyAttack ();
			} else if (attack == AttackType.Block) {
				newMove = block;
			} else if (intInput == 4) {
				newMove = GetBackwardStep ();
			} else if (intInput == 6) {
				newMove = GetForwardStep ();
			}
			else if (intInput == 7) {
				newMove = backwardJump;
			} else if (intInput == 8) {
				newMove = verticalJump;
			} else if (intInput == 9) {
				newMove = forwardJump;
			}
		}
				
		if (newMove != null) {
			// DO NOT DELETE THIS. This line ensures that a MoveSequence can be used more than once.
			newMove.Reset();
		}
		return newMove;
	}

	public virtual IFrameSequence GetNewMove (MoveFrame nextFrame, DirectionalInput dInput, AttackType attack) {
		int intInput = dInput.numpadValue;
		IFrameSequence newMove = null;
		// Get current frame


		// 1. feed input into special move camp; see if anything sticks
		//		i. if a special move is able to be executed & a button is being pressed, get that special move 

		//			TODO: implement special moves
		//			bool ready = fireBall.ReadyMove (dInput);
		//			if (ready) {
		//				newMove = fireBall.GetSpecialMove (attack);
		// 			}

		// 2. proceed thru remaining order and attempt to grab a value
		if (intInput >= 7) {
			// newMove = hashmap.Get(intInput)
			// if newMove != null
			// 		goto end
		}
		if (!AttackType.None.Equals (attack)) {
			// newMove = hashmap.Get(attack);
			// if newMove != null
			// 		goto end
		}

		//		if (newMove
		// newMove = hashmap.Get(intInput);
		// if newMove != null
		// 		goto end

		if (newMove != null) {
			// DO NOT DELETE THIS. 
			// This line ensures that a MoveSequence can be used more than once.
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
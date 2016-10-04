using UnityEngine;
using System.Collections.Generic;

public class CharacterData {
	public readonly int maxHealth = 25;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
//	protected SpecialMove fireBall;
	protected Dictionary<string, IFrameSequence> neutralMoveOptions;
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

	public virtual MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		return null;
	}
		
	public virtual IFrameSequence TryToCancelCurrentMove (MoveFrame currentFrame, DirectionalInput directionalInput, AttackType attack) {
		IFrameSequence newMove = GetSequenceFromDictionary (currentFrame.cancellableTo, directionalInput, attack);
		return newMove;
	}

	public virtual IFrameSequence GetNewMove (DirectionalInput directionalInput, AttackType attack) 
	{
		IFrameSequence newMove = GetSequenceFromDictionary (neutralMoveOptions, directionalInput, attack);
		return newMove;
	}

	IFrameSequence GetSequenceFromDictionary(Dictionary<string, IFrameSequence> optionDictionary,
		DirectionalInput directionalInput, AttackType attack) 
	{
		int intInput = directionalInput.numpadValue;
		IFrameSequence newMove = null;
		bool hasValue = false;
		// Test input in order of what we've defined to be the "priority" of input
		// 1. Can I jump?
		if (intInput >= 7) {
			hasValue = optionDictionary.TryGetValue (intInput.ToString (), out newMove);
			if (hasValue) {
				newMove.Reset ();
				return newMove;
			}
		}
		// 2. Can I attack?
		if (!AttackType.None.Equals (attack)) {
			hasValue = optionDictionary.TryGetValue (attack.ToString (), out newMove);
			if (hasValue) {
				newMove.Reset ();
				return newMove;
			}
		}
		// 3. Lowest priority) Can I move?
		hasValue = optionDictionary.TryGetValue (attack.ToString (), out newMove);
		if (hasValue) {
			newMove.Reset ();
			return newMove;
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
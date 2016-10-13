using UnityEngine;
using System.Collections.Generic;

public class CharacterData
{
	public readonly int maxHealth = 15;
	//	protected SpecialMove fireBall;
	public Dictionary<string, IFrameSequence> neutralMoveOptions { get; private set; }

	protected IFrameSequence jab;
	protected IFrameSequence AA;
	protected IFrameSequence forwardStep;
	protected IFrameSequence backwardStep;
	protected IFrameSequence block;
	IFrameSequence verticalJump;
	IFrameSequence forwardJump;
	IFrameSequence backwardJump;

	public CharacterData ()
	{
		MoveFrame neutralFrame = new MoveFrame ();
		neutralFrame.isLit = true;
		Dictionary<string, IFrameSequence> cancelsForJump = new Dictionary<string, IFrameSequence> ();
		cancelsForJump.Add ("HIT", null);
		AttackFrameData jumpAttackData = new AttackFrameData (new Vector2 (0.25f, -0.625f), 
			new Vector3 (1.2f, .25f, 1f), 5, 6, 6, HitType.HIT);
		MoveFrame jumpAttackHitbox = new MoveFrame (Vector2.zero, MoveType.AIRBORNE, jumpAttackData);
		
		MoveSequence jumpAttack = new MoveSequence (new MoveFrame[] {
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
		});
		cancelsForJump.Add ("A", jumpAttack);
		verticalJump = new JumpSequence (40, 3.5, 0.0, cancelsForJump);
		forwardJump = new JumpSequence (40, 3.5, 2.5, cancelsForJump);
		backwardJump = new JumpSequence (40, 3.5, -2.5, cancelsForJump);

		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (0.8f, 0.2f), 
			new Vector3 (.7f, .25f, 1f), 1, 7, 6, HitType.HIT);
		MoveFrame jabHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, jabAttackData);
		jab = new MoveSequence (new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
		});
			
		AttackFrameData throwAttackData = new AttackFrameData (new Vector2 (0.9f, -0.25f), 
			new Vector3 (.8f, .5f, 1f), 3, 14, 0, HitType.THROW);
		MoveFrame throwHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, throwAttackData);
		MoveSequence _throw = new MoveSequence (new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			neutralFrame,
			throwHitbox,
			throwHitbox,
			throwHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
		});

		AttackFrameData AAAttackData = new AttackFrameData (new Vector2 (0.6f, 0.6f), new Vector3 (.7f, .8f, 1f), 4, 10, 8, HitType.HIT);
		MoveFrame AAHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, AAAttackData);
		AA = new MoveSequence (new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			neutralFrame,
			neutralFrame,
			AAHitbox,
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
		Dictionary<string, IFrameSequence> blockDict = new Dictionary<string, IFrameSequence> ();
		//blockDict.Add ("HIT", null);
		block = new MoveSequence (new MoveFrame[] {
			new MoveFrame (MoveType.BLOCKING)
		}, blockDict
		);

		forwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_FORWARD) });
		backwardStep = new MoveSequence (new MoveFrame[] { new MoveFrame (MoveType.STEP_BACK) });


		neutralMoveOptions = new Dictionary<string, IFrameSequence> ();
		// Adding moves to default FSM
		neutralMoveOptions.Add ("4", backwardStep);
		neutralMoveOptions.Add ("6", forwardStep);
		neutralMoveOptions.Add ("7", backwardJump);
		neutralMoveOptions.Add ("8", verticalJump);
		neutralMoveOptions.Add ("9", forwardJump);
		neutralMoveOptions.Add ("A", jab);
		neutralMoveOptions.Add ("C", AA);
		neutralMoveOptions.Add ("X", block);
		neutralMoveOptions.Add ("T", _throw);
		neutralMoveOptions.Add ("THROW", null);
		neutralMoveOptions.Add ("HIT", null);
	}

}
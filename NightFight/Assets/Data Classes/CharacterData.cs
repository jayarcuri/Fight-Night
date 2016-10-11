using UnityEngine;
using System.Collections.Generic;

public class CharacterData
{
	public readonly int maxHealth = 25;
	HitFrame jabHitbox;
	HitFrame AAHitbox;
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
		//		fireBall = null;
		//			new SpecialMove (
		//			new DirectionalInput[] { DirectionalInput.Down, DirectionalInput.DownRight, DirectionalInput.Right },
		//			AA);
		MoveFrame neutralFrame = MoveFrame.GetLitMoveFrame ();
		Dictionary<string, IFrameSequence> cancelsForJump = new Dictionary<string, IFrameSequence> ();
		cancelsForJump.Add ("HIT", null);
		HitFrame jumpAttackHitbox = new HitFrame (new Vector2 (0.25f, -0.625f), 
			new Vector3 (1.2f, .25f, 1f), Vector2.zero, 1, 7, 6, MoveType.ACTIVE);
		MoveSequence jumpAttack = new MoveSequence (new MoveFrame[] {
			neutralFrame,
			neutralFrame,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			neutralFrame
		});
		cancelsForJump.Add ("A", jumpAttack);
		verticalJump = new JumpSequence (40, 3.5, 0.0, cancelsForJump);
		forwardJump = new JumpSequence (40, 3.5, 2.5, cancelsForJump);
		backwardJump = new JumpSequence (40, 3.5, -2.5, cancelsForJump);

		jabHitbox = new HitFrame (new Vector2 (0.8f, 0.2f), 
			new Vector3 (.7f, .25f, 1f), Vector2.zero, 1, 7, 6, MoveType.ACTIVE);
		jab = new MoveSequence (new MoveFrame[] {
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
			
		HitFrame throwHitbox = new HitFrame (new Vector2 (0.9f, -0.25f), 
			new Vector3 (.8f, .5f, 1f), Vector2.zero, 1, 7, 6, MoveType.THROW);
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

		AAHitbox = new HitFrame (new Vector2 (0.6f, 0.6f), new Vector3 (.7f, .8f, 1f), Vector2.zero, 4, 11, 2, MoveType.ACTIVE);
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
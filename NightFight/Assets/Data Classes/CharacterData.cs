using UnityEngine;
using System.Collections.Generic;

public class CharacterData
{
	public readonly int maxHealth = 15;
	float walkSpeed = 0.15f;
	MoveBufferManager moveBufferManager;


	public Dictionary<string, IFrameSequence> neutralMoveOptions { get; private set; }
	public MoveFrame forwardStepFrame;
	public MoveFrame backStepFrame;

	public CharacterData ()
	{
		moveBufferManager = new MoveBufferManager ();
		DirectionalInput[] dashInput = new DirectionalInput[] {new DirectionalInput(1,0), new DirectionalInput(0,0), new DirectionalInput(1,0)};
		string dashCode = "FrD";
		MoveBuffer forwardDash = new MoveBuffer (8, dashInput, false, dashCode);
		moveBufferManager.AddMoveBuffer (forwardDash);
		DashSequence dash = new DashSequence (10, 5);
		

		MoveFrame neutralFrame = MoveFrame.GetEmptyLitFrame ();
		Dictionary<string, IFrameSequence> cancelsForJump = new Dictionary<string, IFrameSequence> ();
		cancelsForJump.Add ("HIT", null);
		AttackFrameData jumpAttackData = new AttackFrameData (new Vector2 (0.25f, -0.625f), 
			new Vector3 (1.2f, .25f, 1f), 5, 6, 6, HitType.HIT);
		MoveFrame jumpAttackHitbox = new MoveFrame (Vector2.zero, MoveType.AIRBORNE, jumpAttackData);
		
		MoveSequence jumpAttack = new SupplementaryJumpMove (new MoveFrame[] {
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox,
			jumpAttackHitbox
		},
			new Dictionary<string, IFrameSequence>());
		cancelsForJump.Add ("A", jumpAttack);
		JumpSequence verticalJump = new JumpSequence (40, 3.5, 0.0, cancelsForJump);
		JumpSequence forwardJump = new JumpSequence (40, 3.5, 2.5, cancelsForJump);
		JumpSequence backwardJump = new JumpSequence (40, 3.5, -2.5, cancelsForJump);

		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (1f, 0.2f), 
			new Vector3 (1f, .25f, 1f), 1, 7, 6, HitType.HIT);
		MoveFrame jabHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, jabAttackData);
		MoveSequence jab = new MoveSequence (new MoveFrame[] {
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
			
		AttackFrameData throwAttackData = new AttackFrameData (new Vector2 (0.9f, -0.35f), 
			new Vector3 (.8f, .3f, 1f), 3, 0, 14, HitType.THROW);
		MoveFrame throwHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, throwAttackData);
		MoveSequence _throw = new MoveSequence (new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			neutralFrame,
			throwHitbox,
			throwHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
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

		AttackFrameData AAAttackData = new AttackFrameData (new Vector2 (0.8f, 0.6f), new Vector3 (.6f, .8f, 1f), 4, 10, 8, HitType.HIT);
		MoveFrame AAHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, AAAttackData);
		MoveSequence AA = new MoveSequence (new MoveFrame[] {
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
		blockDict.Add ("HIT", null);
		blockDict.Add ("stop us now", null);
		MoveSequence block = new MoveSequence (new MoveFrame[] {
			new MoveFrame (MoveType.BLOCKING)
		}, blockDict
		);
		this.forwardStepFrame = new MoveFrame (new Vector2 (walkSpeed, 0), MoveType.NONE);
		this.backStepFrame = new MoveFrame (new Vector2 (-walkSpeed, 0), MoveType.NONE);
		MoveSequence forwardStep = new MoveSequence (new MoveFrame[] { forwardStepFrame });
		MoveSequence backwardStep = new MoveSequence (new MoveFrame[] { backStepFrame });

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
		neutralMoveOptions.Add (dashCode, dash);

		neutralMoveOptions.Add ("THROW", null);
		neutralMoveOptions.Add ("HIT", null);
	}

	public List<string> GetMoveCodesForReadyBufferMoves(DirectionalInput currentDirectionalInput, AttackType currentButton) {
		return moveBufferManager.GetReadiedBufferMove (currentDirectionalInput, currentButton);
	}

	public void ResetMoveBuffer(string forMove) {
		moveBufferManager.ResetMoveBuffer (forMove);
	}

	public MoveFrame GetEmptyMoveFrame() {
		return new MoveFrame (MoveType.NONE);
	}

	public void SetWalkSpeed(float newWalkSpeed) {
		if (newWalkSpeed <= 0) {
			throw new UnityException ("Invalid speed; must be positive");
		}
		this.forwardStepFrame.movementDuringFrame = new Vector2 (newWalkSpeed, 0);
		this.backStepFrame.movementDuringFrame = new Vector2 (-newWalkSpeed, 0);
	}

}
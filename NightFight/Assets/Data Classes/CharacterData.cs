using UnityEngine;
using System.Collections.Generic;

public class CharacterData
{
	public static int maxCharge = 1200;

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

		RecoilSequence jumpAttackHitStun = new RecoilSequence (10, .7f, MoveType.IN_HITSTUN);
		RecoilSequence jumpAttackBlockStun = new RecoilSequence (8, .7f, MoveType.BLOCKING);
		AttackFrameData jumpAttackData = new AttackFrameData (new Vector2 (0.25f, -0.625f), 
			new Vector3 (1.2f, .25f, 1f), 5, jumpAttackHitStun, jumpAttackBlockStun, HitType.HIT);
		MoveSequence jumpAttack = SupplementaryJumpMove.GetSupplementaryJumpMoveWithFrameData (6, 5, jumpAttackData);
		cancelsForJump.Add ("A", jumpAttack);

		JumpSequence verticalJump = new JumpSequence (40, 3.5f, 0.0f, cancelsForJump);
		JumpSequence forwardJump = new JumpSequence (40, 3.5f, 2.5f, cancelsForJump);
		JumpSequence backwardJump = new JumpSequence (40, 3.5f, -2.5f, cancelsForJump);

		RecoilSequence jabHitStun = new RecoilSequence (9, .6f, MoveType.IN_HITSTUN);
		RecoilSequence jabBlockStun = new RecoilSequence (7, .45f, MoveType.BLOCKING);
		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (1f, 0.2f), 
			new Vector3 (1f, .25f, 1f), 1, jabHitStun, jabBlockStun, HitType.HIT);
		MoveSequence jab = MoveSequence.GetAttackSequenceWithFrameData (3, 3, 6, jabAttackData);

		RecoilSequence throwBlockStun = new RecoilSequence (15, 1.5f, MoveType.BLOCKING);
		AttackFrameData throwAttackData = new AttackFrameData (new Vector2 (0.9f, -0.35f), 
			new Vector3 (.8f, .3f, 1f), 3, null, throwBlockStun, HitType.THROW);
		MoveSequence _throw = MoveSequence.GetAttackSequenceWithFrameData (4, 2, 13, throwAttackData);
		
		RecoilSequence aaHitStun = new RecoilSequence (9, 0.6f, MoveType.IN_HITSTUN);
		RecoilSequence aaBlockStun = new RecoilSequence (7, 0.35f, MoveType.BLOCKING);
		AttackFrameData aaAttackData = new AttackFrameData (new Vector2 (0.8f, 0.6f), new Vector3 (.6f, .8f, 1f), 4, aaHitStun, aaBlockStun, HitType.HIT);
		MoveSequence aa = MoveSequence.GetAttackSequenceWithFrameData (5, 5, 8, aaAttackData);

		Dictionary<string, IFrameSequence> blockDict = new Dictionary<string, IFrameSequence> ();
		blockDict.Add ("HIT", null);
		MoveSequence block = new MoveSequence (new MoveFrame[] {
			new MoveFrame (MoveType.BLOCKING)
		}
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
		neutralMoveOptions.Add ("C", aa);
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
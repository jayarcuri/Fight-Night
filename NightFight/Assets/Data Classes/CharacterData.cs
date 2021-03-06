﻿using UnityEngine;
using System.Collections.Generic;

public class CharacterData
{

	public readonly int maxHealth = 3;
	public readonly float walkSpeed = 0.15f;

	public Dictionary<string, IFrameSequence> defaultNeutralMovesDict { get; private set; }
	public Dictionary<string, IFrameSequence> orbCarryMovesDict { get; private set; }
	MoveFrame forwardStepFrame;
	MoveFrame backStepFrame;
	float orbSpeedModifier = 0.75f;

	public CharacterData ()
	{

		Dictionary<string, IFrameSequence> cancelsForJump = new Dictionary<string, IFrameSequence> ();
		cancelsForJump.Add ("HIT", null);

		RecoilSequence jumpAttackHitStun = new RecoilSequence (10, 1.15f, MoveType.IN_HITSTUN);
		RecoilSequence jumpAttackBlockStun = new RecoilSequence (8, 1.15f, MoveType.BLOCKING);
		AttackFrameData jumpAttackData = new AttackFrameData (new Vector2 (0.5f, -0.625f), 
			new Vector3 (1.5f, .25f, 1f), 3, jumpAttackHitStun, jumpAttackBlockStun, HitType.HIT);
		MoveSequence jumpAttack = SupplementaryJumpMove.GetSupplementaryJumpMoveWithFrameData (6, 5, jumpAttackData, ButtonInputCommand.ATTACK, false);
		cancelsForJump.Add ("A", jumpAttack);

		JumpSequence verticalJump = new JumpSequence (40, 3.5f, 0.0f, cancelsForJump, false);
		JumpSequence forwardJump = new JumpSequence (40, 3.5f, 2.5f, cancelsForJump, true);
		JumpSequence backwardJump = new JumpSequence (40, 3.5f, -2.5f, cancelsForJump, false);

		RecoilSequence jabHitStun = new RecoilSequence (13, 1f, MoveType.IN_HITSTUN);
		RecoilSequence jabBlockStun = new RecoilSequence (10, .85f, MoveType.BLOCKING);
		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (1.25f, 0.2f), 
			new Vector3 (1.5f, .25f, 1f), 1, jabHitStun, jabBlockStun, HitType.HIT);
		MoveSequence jab = MoveSequence.GetAttackSequenceWithFrameData (3, 3, 12, jabAttackData, ButtonInputCommand.ATTACK, true);

		this.forwardStepFrame = new MoveFrame (new Vector2 (walkSpeed, 0), MoveType.NONE);
		this.backStepFrame = new MoveFrame (new Vector2 (-walkSpeed, 0), MoveType.NONE);
		MoveSequence forwardStep = new MoveSequence (new MoveFrame[] { forwardStepFrame }, ButtonInputCommand.NONE);
		MoveSequence backwardStep = new MoveSequence (new MoveFrame[] { backStepFrame }, ButtonInputCommand.NONE);

		defaultNeutralMovesDict = new Dictionary<string, IFrameSequence> ();
		// Adding moves to default FSM
		defaultNeutralMovesDict.Add ("4", backwardStep);
		defaultNeutralMovesDict.Add ("6", forwardStep);
		defaultNeutralMovesDict.Add ("7", backwardJump);
		defaultNeutralMovesDict.Add ("8", verticalJump);
		defaultNeutralMovesDict.Add ("9", forwardJump);
		defaultNeutralMovesDict.Add ("A", jab);

		defaultNeutralMovesDict.Add("HIT", null);
		//	
		//  Orb Move Dict Below
		orbCarryMovesDict = new Dictionary<string, IFrameSequence> ();

		MoveFrame orbForwardStepFrame = new MoveFrame (new Vector2 (walkSpeed * orbSpeedModifier, 0), MoveType.NONE);
		MoveFrame orbBackStepFrame = new MoveFrame (new Vector2 (-walkSpeed * orbSpeedModifier, 0), MoveType.NONE);
		MoveSequence orbForwardStep = new MoveSequence (new MoveFrame[] { orbForwardStepFrame }, ButtonInputCommand.NONE);
		MoveSequence orbBackwardStep = new MoveSequence (new MoveFrame[] { orbBackStepFrame }, ButtonInputCommand.NONE);

		orbCarryMovesDict.Add ("6", orbForwardStep);
		orbCarryMovesDict.Add ("4", orbBackwardStep);
		orbCarryMovesDict.Add ("7", backwardJump);
		orbCarryMovesDict.Add ("8", verticalJump);
		orbCarryMovesDict.Add ("9", forwardJump);
		orbCarryMovesDict.Add ("A", jab);

		orbCarryMovesDict.Add("HIT", null);
	}

	public MoveBufferManager GetMoveBufferManager () {
		MoveBufferManager moveBufferManager = new MoveBufferManager ();

		return moveBufferManager;
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

	MoveSequence GetDashPunch () {
		MoveFrame nFrame = MoveFrame.GetEmptyLitFrame ();
		nFrame.moveType = MoveType.SPECIAL;
		MoveFrame dashFrame = new MoveFrame (new Vector2 (0.25f, 0), MoveType.SPECIAL, true);
		RecoilSequence dashPunchHitStun = new RecoilSequence (18, 2f, MoveType.IN_HITSTUN);
		RecoilSequence dashPunchBlockStun = new RecoilSequence (13, 1.5f, MoveType.BLOCKING);
		AttackFrameData dashPunchAttackData = new AttackFrameData (new Vector2 (1f, 0.2f), 
			new Vector3 (1f, .25f, 1f), 3, dashPunchHitStun, dashPunchBlockStun, HitType.HIT);
		MoveFrame dashAttackFrame = new MoveFrame (dashFrame.movementDuringFrame, MoveType.SPECIAL, dashPunchAttackData);

		MoveFrame[] frameArray = new MoveFrame[31];

		for (int i = 0; i < frameArray.Length; i++) {
			MoveFrame assignedFrame;
			if (15 < i) {
				assignedFrame = nFrame;
			}
			else if (11 < i) {
				assignedFrame = dashAttackFrame;
			} else {
				assignedFrame = dashFrame;
			}

			frameArray [i] = assignedFrame;
		}

		return new MoveSequence (frameArray, ButtonInputCommand.ATTACK);
	}

}
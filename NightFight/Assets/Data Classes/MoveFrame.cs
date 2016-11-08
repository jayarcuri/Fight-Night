using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFrame {
	public MoveType moveType = MoveType.RECOVERY;
	public Vector2 movementDuringFrame;
	public Dictionary<string, IFrameSequence> cancellableTo;
	public AttackFrameData attackData = null;
	public bool isLit { get; private set; }

	public MoveFrame() {
		movementDuringFrame = Vector2.zero;
		isLit = false;
		cancellableTo = GetDefaultCancellables ();
	}

	public MoveFrame (MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = Vector2.zero;
		isLit = false;
		cancellableTo = GetDefaultCancellables ();
	}

	public MoveFrame (Vector2 movementDuringFrame, MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		isLit = false;
		cancellableTo = GetDefaultCancellables ();
	}

	public MoveFrame (Vector2 movementDuringFrame, MoveType moveType, bool isLit) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		this.isLit = isLit;
		cancellableTo = GetDefaultCancellables ();
	}

	public MoveFrame (Vector2 movementDuringFrame, MoveType moveType, AttackFrameData attackData) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		isLit = true;
		cancellableTo = GetDefaultCancellables ();
		this.attackData = attackData;
	}

	public void SetLitness(bool isLit) {
		this.isLit = isLit;
	}

	static Dictionary<string, IFrameSequence> GetDefaultCancellables () {
		Dictionary<string, IFrameSequence> cancelDict = new Dictionary<string, IFrameSequence> ();
		cancelDict.Add("HIT", null);
		return cancelDict;
	}

	public static MoveFrame GetEmptyLitFrame() {
		return new MoveFrame (Vector2.zero, MoveType.NONE, true);
	}
}


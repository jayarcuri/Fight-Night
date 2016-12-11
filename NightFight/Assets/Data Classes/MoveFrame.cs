using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoveFrame {
	public MoveType moveType = MoveType.RECOVERY;
	public Vector2 movementDuringFrame;
	public Dictionary<string, IFrameSequence> cancellableTo;
	public AttackFrameData attackData = null;
	public bool isLit { get; private set; }
	public bool canBeExtended = false;

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
		isLit = (attackData != null);
		cancellableTo = GetDefaultCancellables ();
		this.attackData = attackData;
	}

	public MoveFrame CloneMoveFrame () {
		MoveFrame clone = new MoveFrame (movementDuringFrame, moveType, attackData);
		return clone;
	}

	public void SetLitness(bool isLit) {
		this.isLit = isLit;
	}

	public bool Equals(MoveFrame otherMoveFrame) {
		if (otherMoveFrame == null) {
			return false;
		}

		bool cancelDictionariesEqual = this.cancellableTo.Keys.Count == otherMoveFrame.cancellableTo.Keys.Count 
			&& this.cancellableTo.Keys.SequenceEqual (otherMoveFrame.cancellableTo.Keys);

		return this.moveType == otherMoveFrame.moveType
		&& this.movementDuringFrame.Equals (otherMoveFrame.movementDuringFrame)
		&& cancelDictionariesEqual
		&& (this.attackData == null && otherMoveFrame.attackData == null
				|| (this.attackData != null && this.attackData.Equals (otherMoveFrame.attackData)))
		&& this.isLit == otherMoveFrame.isLit;
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


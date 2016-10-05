using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFrame {
	public MoveType moveType = MoveType.RECOVERY;
	public Vector2 movementDuringFrame;
	public Dictionary<string, IFrameSequence> cancellableTo;
	public bool isLit;

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

	public static MoveFrame GetLitMoveFrame () {
		MoveFrame frame = new MoveFrame ();
		frame.isLit = true;
		frame.cancellableTo = GetDefaultCancellables ();
		return frame;
	}

	public static MoveFrame GetLitMoveFrame (Vector2 movementDuringFrame, MoveType moveType) {
		MoveFrame frame = new MoveFrame (movementDuringFrame, moveType);
		frame.isLit = true;
		frame.cancellableTo =  GetDefaultCancellables ();
		return frame;
	}

	static Dictionary<string, IFrameSequence> GetDefaultCancellables () {
		Dictionary<string, IFrameSequence> cancelDict = new Dictionary<string, IFrameSequence> ();
		cancelDict.Add("HIT", null);
		return cancelDict;
	}
}


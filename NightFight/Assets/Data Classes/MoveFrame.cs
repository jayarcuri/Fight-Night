using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFrame {
	public MoveType moveType = MoveType.RECOVERY;
	public Vector2 movementDuringFrame;
	public Dictionary<string, IFrameSequence> cancellableTo = new Dictionary<string, IFrameSequence> ();
	public bool isLit;

	public MoveFrame() {
		movementDuringFrame = Vector2.zero;
		isLit = false;
	}

	public MoveFrame (MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = Vector2.zero;
		isLit = false;
	}
	public MoveFrame (Vector2 movementDuringFrame, MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		isLit = false;
	}

	public static MoveFrame GetLitMoveFrame () {
		MoveFrame frame = new MoveFrame ();
		frame.isLit = true;
		return frame;
	}

	public static MoveFrame GetLitMoveFrame (Vector2 movementDuringFrame, MoveType moveType) {
		MoveFrame frame = new MoveFrame (movementDuringFrame, moveType);
		frame.isLit = true;
		return frame;
	}
}


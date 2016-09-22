using UnityEngine;
using System.Collections;

public class MoveFrame 
	{
	public MoveType moveType = MoveType.RECOVERY;
	public Vector2 movementDuringFrame;
	public bool isLit = true;

	public MoveFrame() {
		movementDuringFrame = Vector2.zero;
		return;
	}

	public MoveFrame (MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = Vector2.zero;
	}
	public MoveFrame (Vector2 movementDuringFrame, MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		}
	}


using UnityEngine;
using System.Collections;

public class MoveFrame 
	{
	public MoveType moveType = MoveType.RECOVERY;
	public Vector3 movementDuringFrame = Vector3.zero;
	public bool isLit = true;

	public MoveFrame() {
		return;
	}

	public MoveFrame (MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = Vector3.zero;
	}
	public MoveFrame (Vector3 movementDuringFrame, MoveType moveType) {
		this.moveType = moveType;
		this.movementDuringFrame = movementDuringFrame;
		}
	}


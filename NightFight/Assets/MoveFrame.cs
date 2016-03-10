using UnityEngine;
using System.Collections;

public class MoveFrame 
	{
	protected Vector3 movementDuringFrame;
	protected bool isLit;

	public static MoveFrame EmptyFrame() {
		MoveFrame newFrame = MoveFrame (Vector3.zero, true);
		return newFrame;
	}

	public MoveFrame (Vector3 movementDuringFrame)
		{
		this.movementDuringFrame = movementDuringFrame;
		isLit = true;
		}
	public MoveFrame (Vector3 movementDuringFrame, bool isLit)
		{
		this.movementDuringFrame = movementDuringFrame;
		this.isLit = isLit;
		}

	}


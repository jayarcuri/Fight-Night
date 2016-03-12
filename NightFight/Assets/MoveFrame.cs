using UnityEngine;
using System.Collections;

public class MoveFrame 
	{
	protected Vector3 movementDuringFrame;
	protected bool isLit;

	public MoveFrame () {
		this.movementDuringFrame = Vector3.zero;
		this.isLit = true;
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


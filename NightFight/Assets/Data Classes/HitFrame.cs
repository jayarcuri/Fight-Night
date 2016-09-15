using UnityEngine;
using System.Collections;

public class HitFrame : MoveFrame
{
	public Vector3 offset {get; set;}
	public Vector3 size {get; set;}
	public int damage {get; set;}
	public int hitStun {get; set;}
	public MoveSequence hitStunFrames;
	public int blockStun {get; set;}
	public MoveSequence blockStunFrames;

	public HitFrame (Vector3 offset, Vector3 size, Vector3 movementDuringFrame, int damage, int hitStun, int blockStun, MoveType moveType) : base (movementDuringFrame, moveType)
		{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = movementDuringFrame;

		this.damage = damage;
		this.hitStun = hitStun;
		hitStunFrames = new MoveSequence(new MoveFrame[hitStun]);
		this.blockStun = blockStun;
		blockStunFrames = new MoveSequence(new MoveFrame[blockStun]);

		this.isLit = true;
		}
	public HitFrame (Vector3 offset, Vector3 size, int damage, int hitStun, int blockStun) : base()
	{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = Vector3.zero;

		this.damage = damage;
		this.hitStun = hitStun;
		hitStunFrames = new MoveSequence(new MoveFrame[hitStun]);
		this.blockStun = blockStun;
		blockStunFrames = new MoveSequence(new MoveFrame[blockStun]);

		this.isLit = true;
	}

	}



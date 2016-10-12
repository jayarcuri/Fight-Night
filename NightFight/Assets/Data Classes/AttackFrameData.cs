using UnityEngine;
using System.Collections;

public class AttackFrameData
{
	public Vector2 offset {get; private set;}
	public Vector3 size {get; private set;}
	public int damage {get; private set;}
	public int hitStun {get; private set;}
	public MoveSequence hitStunFrames;
	public int blockStun {get; private set;}
	public MoveSequence blockStunFrames;
	public HitType hitType {get; private set;}

	public AttackFrameData (Vector2 offset, Vector3 size, int damage, int hitStun, int blockStun, HitType hitType)
	{
		this.offset = offset;
		this.size = size;

		this.damage = damage;
		this.hitStun = hitStun;
		hitStunFrames = new MoveSequence(new MoveFrame[] {new MoveFrame(MoveType.IN_HITSTUN), new MoveFrame(MoveType.IN_HITSTUN), 
			new MoveFrame(MoveType.IN_HITSTUN), new MoveFrame(MoveType.IN_HITSTUN)});
		this.blockStun = blockStun;
		blockStunFrames = new MoveSequence(new MoveFrame[blockStun]);
		this.hitType = hitType;
	}


}


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
		MoveFrame hitstunFrame = new MoveFrame (new Vector2 (-0.3f, 0), MoveType.IN_HITSTUN);
		hitstunFrame.isLit = true;
		MoveFrame[] rawHSFrames = new MoveFrame[hitStun];
		for (int i = 0; i < rawHSFrames.Length; i++) {
			rawHSFrames [i] = hitstunFrame;
		}
		hitStunFrames = new MoveSequence (rawHSFrames);
		this.blockStun = blockStun;
		MoveFrame blockstunFrame = new MoveFrame (new Vector2 (-0.2f, 0), MoveType.BLOCKING);
		blockstunFrame.isLit = true;
		MoveFrame[] rawBSFrames = new MoveFrame[blockStun];
		for (int i = 0; i < rawBSFrames.Length; i++) {
			rawBSFrames [i] = blockstunFrame;
		}
		blockStunFrames = new MoveSequence(rawBSFrames);
		this.hitType = hitType;
	}


}


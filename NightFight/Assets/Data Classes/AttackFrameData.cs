using UnityEngine;
using System.Collections;

public class AttackFrameData
{
	public Vector2 offset {get; private set;}
	public Vector3 size {get; private set;}
	public int damage {get; private set;}
	public int hitStunFrames {get; private set;}
	public RecoilSequence hitStunSequence;
	public int blockStunFrames {get; private set;}
	public RecoilSequence blockStunSequence;
	public HitType hitType {get; private set;}

	public AttackFrameData (Vector2 offset, Vector3 size, int damage, RecoilSequence hitStun, RecoilSequence blockStun, HitType hitType)
	{
		this.offset = offset;
		this.size = size;

		this.damage = damage;
		hitStunSequence = hitStun;
		hitStunFrames = hitStunSequence != null ? hitStunSequence.lengthInFrames : 0;
		blockStunSequence = blockStun;
		blockStunFrames = blockStunSequence != null ? blockStunSequence.lengthInFrames : 0;
		this.hitType = hitType;
	}

	public bool Equals(AttackFrameData otherAFD) 
	{
		if (otherAFD == null) {
			return false;
		}

		return (this.offset == otherAFD.offset) 
			&& (this.size == otherAFD.size) 
			&& (this.damage == otherAFD.damage)
			&& ((this.hitStunSequence == null && otherAFD.hitStunSequence == null) ||
				((this.hitStunFrames == otherAFD.hitStunFrames) && (this.hitStunSequence.distanceToRecoil == otherAFD.hitStunSequence.distanceToRecoil)))
			&& (this.blockStunFrames == otherAFD.blockStunFrames)
			&& (this.blockStunSequence.distanceToRecoil == otherAFD.blockStunSequence.distanceToRecoil)
			&& (this.hitType == otherAFD.hitType);
	}


}


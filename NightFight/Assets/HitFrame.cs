using UnityEngine;
using System.Collections;

public class HitFrame : MoveFrame
{
	public Vector3 offset {get; set;}
	public Vector3 size {get; set;}
	public float damage {get; set;}
	public int hitStun {get; set;}
	public int blockStun {get; set;}

	public HitFrame (Vector3 offset, Vector3 size, Vector3 movementDuringFrame, float damage, int hitStun, int blockStun, bool isLit) : base (movementDuringFrame, isLit)
		{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = movementDuringFrame;

		this.damage = damage;
		this.hitStun = hitStun;
		this.blockStun = blockStun;

		this.isLit = isLit;
		}
	public HitFrame (Vector3 offset, Vector3 size, float damage, int hitStun, int blockStun) : base()
	{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = Vector3.zero;

		this.damage = damage;
		this.hitStun = hitStun;
		this.blockStun = blockStun;

		this.isLit = true;
	}

	}



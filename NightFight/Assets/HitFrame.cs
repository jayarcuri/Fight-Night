using UnityEngine;
using System.Collections;

public class HitFrame : MoveFrame
{
	Vector3 offset;
	Vector3 size;
	float damage;
	int hitStun;
	int blockStun;

	public HitFrame (Vector3 offset, Vector3 size, Vector3 movementDuringFrame, float damage, int hitStun, int blockStun, bool isLit)
		{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = movementDuringFrame;

		this.damage = damage;
		this.hitStun = hitStun;
		this.blockStun = blockStun;

		this.isLit = isLit;
		}
	public HitFrame (Vector3 offset, Vector3 size, float damage, int hitStun, int blockStun)
	{
		this.offset = offset;
		this.size = size;
		this.movementDuringFrame = 0f;

		this.damage = damage;
		this.hitStun = hitStun;
		this.blockStun = blockStun;

		this.isLit = true;
	}

	}



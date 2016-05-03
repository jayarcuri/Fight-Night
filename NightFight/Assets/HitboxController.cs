using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
	Transform hitbox;
	HitFrame attackPayload;
	// Use this for initialization
	void Start () {
		hitbox = GetComponent<Transform> ();
		enabled = false;
		Reset();
	}
	
	public void ExecuteAttack (Vector3 offset, Vector3 size, HitFrame attackFrame) {
		enabled = true;
		hitbox.localPosition = offset;
		hitbox.localScale = size;
		attackPayload = attackFrame;
	}

	public void Reset () {
		enabled = false;
		hitbox.localPosition = hitbox.localScale = Vector3.zero;
		attackPayload = null;
	}

	public MoveSequence GetCurrentMoveHitstun() {
		return attackPayload.hitStunFrames;
	}

	public int GetCurrentMoveHitStunValue() {
		return attackPayload.hitStun;
	}

	public bool IsLoaded() {
		return (attackPayload != null);
	}

}

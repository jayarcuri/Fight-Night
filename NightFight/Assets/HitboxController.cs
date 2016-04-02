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
	
	public void ExecuteAttack (HitFrame attack) {
		enabled = true;
		hitbox.localPosition = attack.offset;
		hitbox.localScale = attack.size;
		if (attackPayload == null || !attackPayload.Equals(attack))
			attackPayload = attack;
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

}

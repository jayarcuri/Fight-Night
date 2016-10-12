using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
	Transform hitbox;
	AttackFrameData attackPayload;
	// Use this for initialization
	void Start () {
		hitbox = GetComponent<Transform> ();
		enabled = false;
		Reset();
	}
	
	public void ExecuteAttack (AttackFrameData attackFrame) {
		enabled = true;
		hitbox.localPosition = attackFrame.offset;
		hitbox.localScale = attackFrame.size;
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

	public AttackFrameData GetCurrentHitFrame() {
		return attackPayload;
	}

	public int GetCurrentMoveHitStunValue() {
		return attackPayload.hitStun;
	}

	public bool IsLoaded() {
		return (attackPayload != null);
	}

}

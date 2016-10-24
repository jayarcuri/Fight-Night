using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
	Transform hitbox;
	public AttackFrameData attackData { get; private set; }
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
		attackData = attackFrame;
	}

	public void Reset () {
		enabled = false;
		hitbox.localPosition = hitbox.localScale = Vector3.zero;
		attackData = null;
	}

	public MoveSequence GetCurrentMoveHitstun() {
		return attackData.hitStunFrames;
	}

	public AttackFrameData GetCurrentHitFrame() {
		return attackData;
	}

	public int GetCurrentMoveHitStunValue() {
		return attackData.hitStun;
	}

	public bool IsLoaded() {
		return (attackData != null);
	}

}

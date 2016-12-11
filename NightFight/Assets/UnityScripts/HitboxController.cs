using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
	public AttackFrameData attackData { get; private set; }
	public bool hasHit;

	Transform hitboxTransform;

	void Start () {
		hitboxTransform = GetComponent<Transform> ();
		enabled = false;
		Reset();
	}
	
	public void ExecuteAttack (AttackFrameData attackFrame) {
		enabled = true;
		hitboxTransform.localPosition = attackFrame.offset;
		hitboxTransform.localScale = attackFrame.size;
		attackData = attackFrame;
		hasHit = false;
	}

	public void Reset () {
		enabled = false;
		hitboxTransform.localPosition = hitboxTransform.localScale = Vector3.zero;
		attackData = null;
	}

	public AttackFrameData GetCurrentHitFrame() {
		return attackData;
	}

	public bool IsLoaded() {
		return (attackData != null);
	}

}

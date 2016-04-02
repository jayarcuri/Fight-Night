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
		print ("Called");
		enabled = true;
		hitbox.localPosition = attack.offset;
		hitbox.localScale = attack.size;
		if (attackPayload != null) {
			if (!attackPayload.Equals (attack)) {
				attackPayload = attack;
			}
		}
	}

	public void Reset () {
		enabled = false;
		hitbox.localPosition = hitbox.localScale = Vector3.zero;
		attackPayload = null;
	}

}

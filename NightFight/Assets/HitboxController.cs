using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
	Transform hitbox;
	HitFrame attackPayload;
	// Use this for initialization
	void Start () {
		hitbox = GetComponent<Transform> ();
		enabled = false;
		// Testing stuff below
		ExecuteAttack(new HitFrame (new Vector3 (0.7f, 0f, 0f), new Vector3 (.7f, .7f, 1f), Vector3.zero, 1f, 7, 6, true));
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

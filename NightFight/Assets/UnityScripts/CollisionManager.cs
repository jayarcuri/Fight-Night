using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour
{
	public HitboxController pendingAttackHitbox { get; private set; }
	HitboxController hitBox;
	Collider mostRecentIlluminatedArea;
	// Use this for initialization
	void Start ()
	{
		hitBox = GetComponentInChildren<HitboxController> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
		}
	}

	void OnTriggerLeave (Collider other) {

	}

	void ProcessHitboxCollision (Collider hitboxObject)
	{
		HitboxController attackingHitbox = hitboxObject.GetComponent <HitboxController> ();
		if (attackingHitbox.IsLoaded () && !attackingHitbox.attackData.didHit) {
			pendingAttackHitbox = attackingHitbox;
		}
	}

	public bool HasPendingHit () {
		return (pendingAttackHitbox != null && pendingAttackHitbox.attackData != null);
	}

	public bool InIlluminatedArea () {
		return (mostRecentIlluminatedArea != null);
	}

	public AttackFrameData GetPendingHit () {
		if (pendingAttackHitbox != null) {
			return pendingAttackHitbox.GetCurrentHitFrame ();
		} else {
			return null;
		}
	}

	public void ClearPendingHit() {
		pendingAttackHitbox.attackData.didHit = true;
		pendingAttackHitbox = null;
	}
}


using UnityEngine;
using System.Collections;

public class TriggerCollisionManager : MonoBehaviour
{
	public HitboxController pendingAttackHitbox { get; private set; }
	HitboxController hitBox;
	public Collider mostRecentIlluminatedArea;
	// Use this for initialization
	void Start ()
	{
		hitBox = GetComponentInChildren<HitboxController> ();
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == hitBox.gameObject) {
			return;
		}

		if (other == mostRecentIlluminatedArea) {
			mostRecentIlluminatedArea = null;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
		} else {
			ToggleIlluminationStatusAppropriately (other);
		}
	}

	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.collider.gameObject;
		if (other.tag == "Orb") {
			OrbController orb = other.GetComponent <OrbController> ();
			OrbState currentOrbState = orb.currentState;

			if (currentOrbState == OrbState.THROWN && !orb.WasLastOwner (gameObject)) {
				//  is being hit by orb, 
			} else if (currentOrbState == OrbState.NORMAL) {
				Debug.Log ("Orb was equipped");
				orb.currentState = OrbState.EQUIPPED;
				orb.lastOwner = gameObject;
				//  Equip orb
			}

		} 
	}

	void ToggleIlluminationStatusAppropriately (Collider other) {
		if (other.tag == "IlluminationArea" && mostRecentIlluminatedArea != other) {
			mostRecentIlluminatedArea = other;
		}
	}

	void ProcessHitboxCollision (Collider hitboxObject)
	{
		HitboxController attackingHitbox = hitboxObject.GetComponent <HitboxController> ();
		if (attackingHitbox.IsLoaded () && !attackingHitbox.hasHit) {
			pendingAttackHitbox = attackingHitbox;
		}
	}

	public bool HasPendingHit () {
		return (pendingAttackHitbox != null && pendingAttackHitbox.attackData != null);
	}

	public bool IsInAnIlluminatedArea () {
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
		pendingAttackHitbox.hasHit = true;
		pendingAttackHitbox = null;
	}
}


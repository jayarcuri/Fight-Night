using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {
	public OrbState currentState;
	public CharacterManager lastOwner;
	public 
	// Use this for initialization
	void Start () {
		currentState = OrbState.NORMAL;
	}

	void OnCollisionEnter(Collision collision) {
		if (currentState.Equals (OrbState.THROWN)) {
			if (lastOwner != null && collision.collider.gameObject != lastOwner.gameObject) {
				Debug.Log ("Orb has contacted wall or player");
				currentState = OrbState.NORMAL;
			}
		}
	}

	public bool WasLastOwner (GameObject character) {
		return (character.Equals(lastOwner));
	}
}

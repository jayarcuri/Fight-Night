using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {
	public OrbState currentState;
	public GameObject lastOwner;
	public 
	// Use this for initialization
	void Start () {
		currentState = OrbState.NORMAL;
	}

	public bool WasLastOwner (GameObject character) {
		return (character.Equals(lastOwner));
	}
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public string horizontalAxis;
	public string jump;
	PlayerMovement playerMovement;

	void Start () {
		playerMovement = GetComponent<PlayerMovement> ();

	}

	void FixedUpdate() {
		// read inputs
		float h = Input.GetAxis (horizontalAxis);
		bool jumping = Input.GetButtonDown (jump);

		playerMovement.Move (h);

		if (jumping)
			playerMovement.Jump ();
	}
}

using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public string horizontalAxis;
	public string verticalAxis;
	public string jump;
	public string lightAttack;
	public string heavyAtack;
	CharacterMovement playerMovement;

	void Start () {
		playerMovement = GetComponent<CharacterMovement> ();
	}

	void FixedUpdate() {
		// read inputs
		float horizontalInput = Input.GetAxisRaw (horizontalAxis);
		float verticalInput = Input.GetAxisRaw (verticalAxis);
		bool jumping = Input.GetButton (jump);

		if (jumping)
			playerMovement.Jump ();

		playerMovement.Move (horizontalInput);
	}
	public void GetInputs (out float horizontalInput, out float verticalInput, out bool jumping, out AttackType attack) {
		// read inputs
		horizontalInput = Input.GetAxisRaw (horizontalAxis);
		verticalInput = Input.GetAxisRaw (verticalAxis);
		jumping = Input.GetButton (jump);
		if (Input.GetButton (heavyAtack)) {
			attack = AttackType.Heavy;
		} else if (Input.GetButton (lightAttack)) {
			attack = AttackType.Light;
		} else
			attack = AttackType.None;
	}
}

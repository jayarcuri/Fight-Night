using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public string horizontalAxis;
	public string verticalAxis;
	public string jump;
	public string lightAttack;
	public string heavyAtack;
	public CharacterMovement playerMovement;
	public CharacterController characterController;

	void Start () {
		playerMovement = GetComponent<CharacterMovement> ();
		characterController = GetComponent<CharacterController> ();
	}

	// Inputs are represented by an enum which corrisponds with an int 

	public void GetInputs (out DirectionalInput directionalInput, out AttackType attack) {
		// read directional inputs
		float input = 5f;
		input = input + Input.GetAxisRaw (horizontalAxis) + (Input.GetAxisRaw (verticalAxis) * 3);

		directionalInput = (DirectionalInput)((int)input);

		if (Input.GetButtonDown (heavyAtack)) {
			attack = AttackType.Heavy;
		} // Light uses GetButton to allow for chaining jabs
		else if (Input.GetButton (lightAttack)) {
			attack = AttackType.Light;
		} else
			attack = AttackType.None;
	}
}

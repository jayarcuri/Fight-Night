using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public string horizontalAxis;
	public string verticalAxis;
	public string jump;
	public string lightAttack;
	public string heavyAtack;
	CharacterMovement playerMovement;
	public CharacterController characterController;

	void Start () {
		playerMovement = GetComponent<CharacterMovement> ();
		characterController = GetComponent<CharacterController> ();
	}

/*	void FixedUpdate() {
		// read inputs
		float horizontalInput, verticalInput;
		AttackType attack;
		//bool jumping = Input.GetButton (jump);
		GetInputs (out horizontalInput, out verticalInput, out attack);

		characterController.ExecuteInput (horizontalInput, verticalInput, attack);

		
		if (jumping)
			playerMovement.Jump (horizontalInput);

		playerMovement.Move (horizontalInput);
	}*/
	public void GetInputs (out float horizontalInput, out float verticalInput, out AttackType attack) {
		// read inputs
		horizontalInput = Input.GetAxisRaw (horizontalAxis);
		verticalInput = Input.GetAxisRaw (verticalAxis);

		if (Input.GetButton (heavyAtack)) {
			attack = AttackType.Heavy;
		} else if (Input.GetButton (lightAttack)) {
			attack = AttackType.Light;
		} else
			attack = AttackType.None;
	}
}

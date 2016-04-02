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

	public void GetInputs (out float horizontalInput, out float verticalInput, out AttackType attack) {
		// read inputs
		horizontalInput = Input.GetAxisRaw (horizontalAxis);
		verticalInput = Input.GetAxisRaw (verticalAxis);

		if (Input.GetButtonDown (heavyAtack)) {
			attack = AttackType.Heavy;
		} // Light uses GetButton to allow for chaining jabs
		else if (Input.GetButton (lightAttack)) {
			attack = AttackType.Light;
		} else
			attack = AttackType.None;
	}
}

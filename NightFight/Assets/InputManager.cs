using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public string horizontalAxis;
	public string verticalAxis;
	public string lightAttack;
	public string heavyAtack;
	public string illuminateButton;
	public string block;

	// Inputs are represented by an enum which corresponds with an int 

	public void GetInputs (out DirectionalInput directionalInput, out AttackType attack, out bool toggleLight) {
		// read directional inputs
		int horizontal =  (int)Input.GetAxisRaw (horizontalAxis);
		int vertical = (int)Input.GetAxisRaw (verticalAxis);

		directionalInput = new DirectionalInput(horizontal, vertical);

		if (Input.GetButton (heavyAtack)) {
			attack = AttackType.Heavy;
		} else if (Input.GetButton (lightAttack) && Input.GetButton (block)) {
			attack = AttackType.Throw;
		} else if (Input.GetButton (lightAttack)) {
			attack = AttackType.Light;
		} else if (Input.GetButton (block)) {
			attack = AttackType.Block;
		} else {
			attack = AttackType.None;
		}

		if (Input.GetButton (illuminateButton)) {
			toggleLight = true;
		} else {
			toggleLight = false;
		}
	}
}

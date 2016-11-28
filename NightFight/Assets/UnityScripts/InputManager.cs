using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	public string horizontalAxis;
	public string verticalAxis;
	public string lightAttack;
	bool lightAttackPressed;
	public string heavyAtack;
	bool heavyAttackPressed;
	public string illuminateButton;
	bool illuminateButtonPressed;
	public string block;

	// Inputs are represented by an enum which corresponds with an int

	public void GetInputs (out DirectionalInput directionalInput, out AttackType attack, out bool toggleLight)
	{
		// read directional inputs
		int horizontal = (int)Input.GetAxisRaw (horizontalAxis);
		int vertical = (int)Input.GetAxisRaw (verticalAxis);

		directionalInput = new DirectionalInput (horizontal, vertical);

		if (Input.GetButton (heavyAtack) && !heavyAttackPressed) {
			heavyAttackPressed = true;
			attack = AttackType.Heavy;
		} else if (Input.GetButton (lightAttack) && !lightAttackPressed) {
			if (Input.GetButton (block)) {
				lightAttackPressed = true;
				attack = AttackType.Throw;
			} else {
				lightAttackPressed = true;
				attack = AttackType.Light;
			}
		} else if (Input.GetButton (block)) {
			attack = AttackType.Block;
		} else {
			attack = AttackType.None;
		}

		if (Input.GetButton (illuminateButton) && !illuminateButtonPressed) {
			illuminateButtonPressed = true;
			toggleLight = true;
		} else {
			toggleLight = false;
		}

		RecordDisengagedButtons ();
	}

	void RecordDisengagedButtons ()
	{
		if (!Input.GetButton (heavyAtack)) {
			heavyAttackPressed = false;
		}
		if (!Input.GetButton (lightAttack)) {
			lightAttackPressed = false;
		}
		if (!Input.GetButton (illuminateButton)) {
			illuminateButtonPressed = false;
		}

	}
}

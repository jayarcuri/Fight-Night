using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public void NewGetInputs (out DirectionalInput directionalInput, out ButtonInput[] activeButtons, out bool toggleLight)
	{
		// read directional inputs
		List<ButtonInput> activeButtonList = new List<ButtonInput> ();
		int horizontal = (int)Input.GetAxisRaw (horizontalAxis);
		int vertical = (int)Input.GetAxisRaw (verticalAxis);

		directionalInput = new DirectionalInput (horizontal, vertical);

		if (Input.GetButton (heavyAtack) && !heavyAttackPressed) {
			heavyAttackPressed = true;
			activeButtonList.Add(new ButtonInput(AttackType.Heavy, ButtonState.depressed));
		} 

		if (Input.GetButton (lightAttack) && !lightAttackPressed) {
			if (Input.GetButton (block)) {
				lightAttackPressed = true;
				activeButtonList.Add(new ButtonInput(AttackType.Throw, ButtonState.depressed));
			} else {
				lightAttackPressed = true;
				activeButtonList.Add(new ButtonInput(AttackType.Light, ButtonState.depressed));
			}
		}
		if (Input.GetButton (block)) {
			activeButtonList.Add(new ButtonInput(AttackType.Block, ButtonState.sustained));
		}

		if (Input.GetButton (illuminateButton) && !illuminateButtonPressed) {
			illuminateButtonPressed = true;
			toggleLight = true;
		} else {
			toggleLight = false;
		}

		RecordDisengagedButtons ();

		activeButtons = activeButtonList.ToArray ();
	}
}

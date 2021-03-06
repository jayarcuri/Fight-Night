﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Eppy;

public class InputManager : MonoBehaviour
{
	public string horizontalAxis;
	public string verticalAxis;
	public string lightAttack;
	bool lightAttackPressed;
	public string illuminateButton;
	bool illuminateButtonPressed;

	Tuple<string, ButtonInputCommand>[] buttons;

	// Inputs are represented by an enum which corresponds with an int

	void Start () {
		buttons = new Tuple<string, ButtonInputCommand>[] {
			new Tuple<string, ButtonInputCommand>(lightAttack, ButtonInputCommand.ATTACK)
		};
	}

	public void GetInputs (out DirectionalInput directionalInput, out ButtonInputCommand attack, out bool toggleLight)
	{
		// read directional inputs
		int horizontal = (int)Input.GetAxisRaw (horizontalAxis);
		int vertical = (int)Input.GetAxisRaw (verticalAxis) * -1;

		directionalInput = new DirectionalInput (horizontal, vertical);

		if (Input.GetButton (lightAttack) && !lightAttackPressed) {
			lightAttackPressed = true;
			attack = ButtonInputCommand.ATTACK;
		} else {
			attack = ButtonInputCommand.NONE;
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
		if (!Input.GetButton (lightAttack)) {
			lightAttackPressed = false;
		}
		if (!Input.GetButton (illuminateButton)) {
			illuminateButtonPressed = false;
		}

	}

	public void NewGetInputs (out ButtonInput[] activeButtons, out bool toggleLight)
	{
		// read directional inputs
		List<ButtonInput> activeButtonList = new List<ButtonInput> ();
		/*
		int horizontal = (int)Input.GetAxisRaw (horizontalAxis);
		int vertical = (int)Input.GetAxisRaw (verticalAxis);

		directionalInput = new DirectionalInput (horizontal, vertical);
		*/

		foreach (Tuple<string, ButtonInputCommand> button in buttons) {
			ButtonState buttonState = GetCurrentButtonStateForButton (button.Item1);
			if (buttonState != ButtonState.NONE) {
				activeButtonList.Add (new ButtonInput(button.Item2, buttonState));
			}
		}

		if (Input.GetButton (illuminateButton) && !illuminateButtonPressed) {
			illuminateButtonPressed = true;
			toggleLight = true;
		} else {
			if (!Input.GetButton (illuminateButton)) {
				illuminateButtonPressed = false;
			}
			toggleLight = false;
		}

		activeButtons = activeButtonList.ToArray ();
	}

	ButtonState GetCurrentButtonStateForButton(string buttonName) {
		if (Input.GetButtonDown (buttonName)) {
			return ButtonState.DEPRESSED;
		} else if (Input.GetButtonUp (buttonName)) {
			return ButtonState.RELEASED;
		} else if (Input.GetButton (buttonName)) {
			return ButtonState.SUSTAINED;
		} else {
			return ButtonState.NONE;
		}
	}
}

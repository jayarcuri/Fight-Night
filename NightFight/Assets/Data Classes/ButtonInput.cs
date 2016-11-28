using System;

public struct ButtonInput
{
	public AttackType buttonType;
	public ButtonState buttonState;

	public ButtonInput(AttackType buttonType, ButtonState buttonState) {
		this.buttonType = buttonType;
		this.buttonState = buttonState;
	}
}


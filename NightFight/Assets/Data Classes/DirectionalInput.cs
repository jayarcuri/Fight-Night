using UnityEngine;
using System.Collections;

public class DirectionalInput
{
	public static DirectionalInput Neutral = new DirectionalInput (0, 0);
	public static DirectionalInput Right = new DirectionalInput (-1, 0);
	public static DirectionalInput Down = new DirectionalInput (0, -1);
	public static DirectionalInput DownRight = new DirectionalInput (1, -1);

	public int horizontalInput { get; protected set; }

	public int verticalInput { get; protected set; }

	public int numpadValue { get; protected set; }

	public DirectionalInput (int horizontalInput, int verticalInput)
	{
		if (horizontalInput < -1 || horizontalInput > 1 || verticalInput < -1 || verticalInput > 1) {
			throw new UnityException ("Invalid params for DirectionalInput");
		}
		this.horizontalInput = horizontalInput;
		this.verticalInput = verticalInput;
		numpadValue = GetNumpadNotation (verticalInput, horizontalInput);
	}

	public DirectionalInput (int numpadRepresentation)
	{
		if (numpadRepresentation < 1 || numpadRepresentation > 9) {
			throw new UnityException ("Invalid params for DirectionalInput");
		}

		int horizontal = 0;
		int vertical;

		switch (numpadRepresentation % 3) {
		case 1:
			horizontal = -1;
			break;
		case 2:
			horizontal = 0;
			break;
		case 0:
			horizontal = 1;
			break;
		}

		vertical = ((numpadRepresentation - 1) / 3) - 1;

		horizontalInput = horizontal;
		verticalInput = vertical;
		numpadValue = numpadRepresentation;
	}

	private int GetNumpadNotation (int verticalInput, int horizontalInput)
	{
		int numpadInt = 5;
		numpadInt += verticalInput * 3;
		numpadInt += horizontalInput;

		return numpadInt;
	}

	public void FlipHorizontalInput ()
	{
		horizontalInput *= -1;
		numpadValue = GetNumpadNotation (verticalInput, horizontalInput);
	}

	public bool Equals(DirectionalInput p)
	{
		if (p == null)
		{
			return false;
		}

		return (horizontalInput == p.horizontalInput && verticalInput == p.verticalInput);
	}

	public static DirectionalInput[] GetDirectionalInputArray (params int[] numpadValues) {
		DirectionalInput[] returnArray = new DirectionalInput[numpadValues.Length];
		for (int i = 0; i < numpadValues.Length; i++) {
			returnArray [i] = new DirectionalInput (numpadValues [i]);
		}

		return returnArray;
	}

}

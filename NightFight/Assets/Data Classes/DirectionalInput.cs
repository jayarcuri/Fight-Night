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

	public DirectionalInput (int horizontalInput, int verticalInput)
	{
		this.horizontalInput = horizontalInput;
		this.verticalInput = verticalInput;
	}

	public DirectionalInput (int numpadRepresentation)
	{
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
	}

	public int GetNumpadNotation ()
	{
		int numpadInt = 5;
		numpadInt += verticalInput * 3;
		numpadInt += horizontalInput;

		return numpadInt;
	}

	public void FlipHorizontalInput ()
	{
		horizontalInput *= -1;
	}

	public bool Equals(DirectionalInput p)
	{
		if (p == null)
		{
			return false;
		}

		return (horizontalInput == p.horizontalInput && verticalInput == p.verticalInput);
	}

}

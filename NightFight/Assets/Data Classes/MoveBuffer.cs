using System;

public class MoveBuffer {
	public string moveCode { get; private set;}

	protected int bufferActiveFor;
	protected int maxAllowedBufferLength;
	protected int bufferIndex;
	protected bool requiresButtonPress;
	protected DirectionalInput[] inputSequence;

	public static string bufferMove1 = "BU1";
	public static string bufferMove2 = "BU2";
	public static string bufferMove3 = "BU3";

	public MoveBuffer (int maxAllowedBufferLength, DirectionalInput[] inputSequence, bool requiresButtonPress, string moveCode) {
		this.maxAllowedBufferLength = maxAllowedBufferLength;
		this.inputSequence = inputSequence;
		this.requiresButtonPress = requiresButtonPress;
		this.moveCode = moveCode;
	}

	public string GetMove (DirectionalInput currentDirectionalInput, bool buttonPressOccurred) {
		if (bufferActiveFor > 0) {
			bufferActiveFor++;
		}

		if (bufferIndex < inputSequence.Length
			&& currentDirectionalInput.Equals(inputSequence [bufferIndex])) {

			if (bufferIndex == 0) {
				bufferActiveFor++;
			}

			bufferIndex++;
		}

		if (bufferIndex == inputSequence.Length && (!requiresButtonPress || buttonPressOccurred)) {
			return moveCode;
		}

		if (bufferActiveFor >= maxAllowedBufferLength) {
			this.Reset ();
		}
			
		return null;
	}

	public void Reset () {
		bufferIndex = 0;
		bufferActiveFor = 0;
	}
}
	
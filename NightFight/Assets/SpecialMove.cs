using System;
using System.Collections.Generic;

public class SpecialMove : BufferMove {
	Dictionary<AttackType, MoveSequence> moveDictionary;
	MoveSequence lightVersion;
	MoveSequence midVersion;
	MoveSequence heavyVersion;

	public SpecialMove (DirectionalInput[] inputCommand, MoveSequence move) {

		this.move = move;
		this.inputCommand = inputCommand;
		moveCompletionIndex = 0;
		bufferLength = inputCommand.Length + 5; // Change int to change length of buffer
		expiration = bufferLength;

		moveDictionary = new Dictionary<AttackType, MoveSequence> ();
		moveDictionary.Add (AttackType.Light, lightVersion);
		moveDictionary.Add (AttackType.Heavy, heavyVersion);
		}

	public MoveSequence GetSpecialMove (AttackType attack) {
		// Reset counters
		expiration = bufferLength;
		moveCompletionIndex = 0;
		return moveDictionary [attack];
	}

	// Returns true when the sMove could be executed
	public bool ReadyMove (DirectionalInput dirInput) {
		// Look for the next req. input in the command sequence.
		if (expiration > 0) {
			if (dirInput == inputCommand [moveCompletionIndex]) {
				++moveCompletionIndex;
			}
		}
		// Reset when the countdown has reached zero.
		else {
			expiration = bufferLength;
			moveCompletionIndex = 0;
		} 
		// De-increment the expiration date after the first required input is found.
		if (moveCompletionIndex > 0)
			--expiration;
		
		return (moveCompletionIndex == inputCommand.Length);
	}
}


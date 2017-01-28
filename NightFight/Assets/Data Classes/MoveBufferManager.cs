using System;
using System.Collections.Generic;
public class MoveBufferManager
{
	List<MoveBuffer> bufferList;
	Dictionary<string, MoveBuffer> bufferDictionary;

	public MoveBufferManager ()
	{
		bufferList = new List<MoveBuffer> ();
		bufferDictionary = new Dictionary<string, MoveBuffer> ();
	}

	public void AddMoveBuffer(MoveBuffer newMoveBuffer) {
		bufferList.Add (newMoveBuffer);

		string newMoveCode = newMoveBuffer.moveCode;
		bufferDictionary.Add (newMoveCode, newMoveBuffer);
	}

	public List<string> GetReadiedBufferMove (DirectionalInput currentDirectionalInput, ButtonInputCommand nextButtonInput) {
		List<string> moveCodeList = new List<string> ();
		foreach (MoveBuffer currentBuffer in bufferList) {
			bool buttonPressOccurred = nextButtonInput != ButtonInputCommand.NONE;
			string moveCode = currentBuffer.GetMove (currentDirectionalInput, buttonPressOccurred);

			if (moveCode != null) {
				moveCodeList.Add(moveCode);
			}
		}
		return moveCodeList;
	}

	public void ResetMoveBuffer(string forMove) {
		MoveBuffer bufferToReset;

		if (bufferDictionary.TryGetValue (forMove, out bufferToReset)) {
			bufferToReset.Reset ();
		}
	}

	public void ResetAll() {
		foreach (MoveBuffer currentBuffer in bufferList) {
			currentBuffer.Reset ();
		}
	}
}


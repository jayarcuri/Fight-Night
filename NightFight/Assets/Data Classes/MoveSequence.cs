using UnityEngine;
using System.Collections;

public class MoveSequence {
	int index;
	MoveFrame[] moveSequence;

	public MoveSequence(MoveFrame[] moveSequence) {
		index = -1;
		this.moveSequence = moveSequence;
	}

	public MoveFrame GetNext() {
		if(HasNext()) {
			++index;
			return moveSequence [index];
			}
		else
			throw new System.IndexOutOfRangeException("Current move sequence does not have a next move!");
		}

	public MoveFrame Peek () {
		if (HasNext()) {
			return moveSequence [index+1];
		}
		else
			throw new System.IndexOutOfRangeException("Current move sequence does not have a next move!");
	}

	public bool HasNext() {
		return (index < moveSequence.Length - 1);
	}

	public MoveFrame GetPrevious() {
		MoveFrame returnMove;

		if (index == -1)
			returnMove = null;
		else
			returnMove = moveSequence [index - 1];
		return returnMove;
	}

	public int MoveLength () {
		return moveSequence.Length;
	}


}

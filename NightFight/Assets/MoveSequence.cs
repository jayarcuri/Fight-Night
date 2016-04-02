using UnityEngine;
using System.Collections;

public class MoveSequence {
	int index;
	MoveFrame[] moveSequence;

	public MoveSequence(MoveFrame[] moveSequence) {
		index = 0;
		this.moveSequence = moveSequence;
	}

	public MoveFrame getNext() {
		if(hasNext()) {
			++index;
			return moveSequence [index];
			}
		else
			throw new System.IndexOutOfRangeException("Current move sequence does not have a next move!");
		}

	public bool hasNext() {
		return (index < moveSequence.Length - 1);
	}

	public MoveFrame getLast() {
		MoveFrame returnMove;

		if (index == 0)
			returnMove = null;
		else
			returnMove = moveSequence [index - 1];
		return returnMove;
	}


}

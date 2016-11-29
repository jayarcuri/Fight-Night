using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashSequence : IFrameSequence {
	Vector2 velocity;
	int maxFrame; 
	int currFrame; 
	MoveFrame returnFrame;
	public DashSequence(int dashLengthInFrames, float dashDistance) {
		//do I need this.currFrame? 
		currFrame = 0; 
		this.maxFrame = dashLengthInFrames; 
		this.velocity = new Vector2 (dashDistance / dashLengthInFrames, 0.0f);
		this.returnFrame = new MoveFrame (this.velocity, MoveType.RECOVERY);

	}

	public MoveFrame GetNext () {
		if (HasNext ()) {
			currFrame++; 
			return returnFrame; 

		} else {
			throw new System.IndexOutOfRangeException ("Current move sequence does not have a next move!"); 

		}
	}


	public bool HasNext() {
		//&& or ||? 
		return (currFrame < maxFrame); 


	}


	public void Reset () {
		currFrame = 0;


	}


	public MoveFrame Peek () {
		return returnFrame;
		//return frame without incrememnting 
		//return null;
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {

		return null;
	}

	public void IncrementIndex () {
		currFrame++;
	}

	public bool SequenceStartedWithButton(ButtonInput[] engagedButtons) {
		return false;
	}

}
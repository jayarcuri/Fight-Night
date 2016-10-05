using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashSequence : IFrameSequence {

	public DashSequence(int dashLengthInFrames, double dashDistance) {
		return;
	}

	public MoveFrame GetNext () {
		return null;
	}

	public bool HasNext () {
		return false;
	}

	public void Reset () {
		return;
	}

	public MoveFrame Peek () {
		return null;
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {
		return null;
	}
}

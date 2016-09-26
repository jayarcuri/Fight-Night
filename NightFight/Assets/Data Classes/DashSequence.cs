using UnityEngine;
using System.Collections;

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
}

using UnityEngine;
using System;
using System.Collections.Generic;

public class SupplementaryJumpMove : MoveSequence
	{
	MoveFrame foreverTerribleFrame;
	public SupplementaryJumpMove (MoveFrame[] moveSequence, Dictionary<string, IFrameSequence> cancelsTo) 
		: base (moveSequence, cancelsTo) {
		foreverTerribleFrame = new MoveFrame (Vector2.zero, MoveType.RECOVERY, true);
	}

	public override MoveFrame GetNext ()
	{
		MoveFrame nextFrame;
		if (base.HasNext ()) {
			nextFrame = base.GetNext ();
		} else {
			nextFrame = foreverTerribleFrame;
		}

		return nextFrame;
	}

	public override MoveFrame Peek ()
	{
		MoveFrame peekFrame;
		if (base.HasNext ()) {
			peekFrame = base.Peek ();
		} else {
			peekFrame = foreverTerribleFrame;
		}

		return peekFrame;
	}

	public override bool HasNext ()
	{
		return true;
	}

}


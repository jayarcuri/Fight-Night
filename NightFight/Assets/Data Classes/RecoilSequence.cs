using UnityEngine;
using System;
using System.Collections.Generic;

public class RecoilSequence : IFrameSequence
{
	int lengthInFrames;
	float velocity;
	float friction = 0.05f;
	MoveType recoilType;
	Dictionary<string, IFrameSequence> cancellableDictionary;

	public RecoilSequence (int lengthInFrames, float distanceToTravel, MoveType recoilType)
	{
		this.lengthInFrames = lengthInFrames;
		velocity = Mathf.Sqrt((float)lengthInFrames * 2f * friction * distanceToTravel);
		this.recoilType = recoilType;
		Dictionary<string, IFrameSequence> blockDict = new Dictionary<string, IFrameSequence> ();
		blockDict.Add ("HIT", null);
		cancellableDictionary = blockDict;

	}

	public MoveFrame GetNext () {
		MoveFrame returnFrame = GetNextFrame ();
		IncrementIndex ();
		return returnFrame;
	}

	public bool HasNext () {
		return (velocity > 0);
	}

	public void Reset () {
		velocity = lengthInFrames * friction;
	}

	public MoveFrame Peek () {
		return GetNextFrame ();
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {
		return cancellableDictionary;
	}

	public void IncrementIndex () {
		velocity -= friction;
	}

	MoveFrame GetNextFrame () {
		if (velocity <= 0) {
			throw new Exception ("RecoilSequence does not have next!");
		}

		return new MoveFrame (new Vector2 (-velocity, 0), recoilType, true);
	}

}


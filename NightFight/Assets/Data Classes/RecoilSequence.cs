using UnityEngine;
using System;
using System.Collections.Generic;

public class RecoilSequence : IFrameSequence
{
	public int lengthInFrames { get; private set; }
	int currentFrameCount;
	public float distanceToRecoil { get; private set; }
	MoveType recoilType;
	Dictionary<string, IFrameSequence> cancellableDictionary;

	public RecoilSequence (int lengthInFrames, float distance, MoveType recoilType)
	{
		this.lengthInFrames = lengthInFrames;
		this.distanceToRecoil = distance;
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
		return (currentFrameCount < lengthInFrames);
	}

	public void Reset () {
		currentFrameCount = 0;
	}

	public MoveFrame Peek () {
		return GetNextFrame ();
	}

	public Dictionary<string, IFrameSequence> GetCancellableDictionary () {
		return cancellableDictionary;
	}

	public void IncrementIndex () {
		currentFrameCount++;
	}

	MoveFrame GetNextFrame () {
		if (!HasNext()) {
			throw new Exception ("RecoilSequence does not have next!");
		}
		float currentPercentage = Mathf.Sin ((float) (currentFrameCount + 1) / lengthInFrames * Mathf.PI * 0.5f);
		float previousPercentage = Mathf.Sin ((float) currentFrameCount / lengthInFrames * Mathf.PI * 0.5f);
		float currentFrameDistance = distanceToRecoil * (currentPercentage - previousPercentage);
		return new MoveFrame (new Vector2 (-currentFrameDistance, 0), recoilType, true);
	}

}


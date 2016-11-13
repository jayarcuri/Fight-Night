using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {
	public int roundLengthInSeconds;
	public int secondsLeftInRound {get; private set; }

	GameDirector gameDirector;
	Text timerText;
	float roundStartTime;

	void Start () {		
		timerText = GetComponent<Text> ();
		gameDirector = GameObject.FindGameObjectWithTag ("GameDirector").GetComponent<GameDirector> ();
		SetUpTimer ();
	}
	
	public void UpdateTimer () {
		float currentTime = Time.time;
		int secondsSinceRoundHasStarted = Mathf.RoundToInt (currentTime - roundStartTime);

		secondsLeftInRound = roundLengthInSeconds - secondsSinceRoundHasStarted;

		timerText.text = secondsLeftInRound.ToString().PadLeft(2, '0');

		if (secondsLeftInRound <= 0) {
			CancelInvoke ();
			gameDirector.timeOver = true;
		}
	}

	public void SetUpTimer () {
		timerText.text = roundLengthInSeconds.ToString ();
		roundStartTime = Time.time;
		InvokeRepeating ("UpdateTimer", 0f, 1f); 
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {
	public int roundLengthInSeconds;
	public int secondsLeftInRound {get; private set; }

	Text timerText;
	float roundStartTime;

	void Start () {		
		timerText = GetComponent<Text> ();
	}

	public void UpdateTimer () {
		float currentTime = Time.time;
		int secondsSinceRoundHasStarted = Mathf.RoundToInt (currentTime - roundStartTime);

		secondsLeftInRound = roundLengthInSeconds - secondsSinceRoundHasStarted;

		timerText.text = secondsLeftInRound.ToString().PadLeft(2, '0');

		if (secondsLeftInRound <= 0 || GameStateManager.GetCurrentGameState() == GameState.GAME_OVER) {
			StopTimer ();
		}
	}

	public void SetUpTimer () {
		timerText.text = roundLengthInSeconds.ToString ();
		roundStartTime = Time.time;
		secondsLeftInRound = roundLengthInSeconds;
		InvokeRepeating ("UpdateTimer", 0f, 1f); 
	}

	public void StopTimer () {
		CancelInvoke ();
	}
}

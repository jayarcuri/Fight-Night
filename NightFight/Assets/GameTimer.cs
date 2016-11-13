using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {
	public int roundLengthInSeconds;
	public GameObject victoryScreen;

	Text timerText;
	float roundStartTime;
	int secondsLeftInRound;

	// Use this for initialization
	void Start () {		
		timerText = GetComponent<Text> ();
		SetUpTimer ();
	}
	
	public void UpdateTimer () {
		float currentTime = Time.time;
		int secondsSinceRoundHasStarted = Mathf.RoundToInt (currentTime - roundStartTime);

		secondsLeftInRound = roundLengthInSeconds - secondsSinceRoundHasStarted;

		timerText.text = secondsLeftInRound.ToString().PadLeft(2, '0');

		if (secondsLeftInRound <= 0) {
			//	TODO: tigger game over screen (with Time Over)
			victoryScreen.SetActive(true);
			CancelInvoke ();
			Time.timeScale = 0;
		}
	}

	public void SetUpTimer () {
		timerText.text = roundLengthInSeconds.ToString ();
		roundStartTime = Time.time;
		InvokeRepeating ("UpdateTimer", 0f, 1f); 
	}
}

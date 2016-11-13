using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameMenuController : MonoBehaviour {
	Text victoryTitle;
	Text victoryText;
	readonly string defaultVictoryText = "You have emerged from The Night (tm) as the victor. " +
		"Your parents are finally proud of you & you are a step closer to achieving your dreams.";

	void Start () {
		victoryTitle = GameObject.FindGameObjectWithTag ("VictoryWindowTitle").GetComponent<Text> ();
		victoryText = GameObject.FindGameObjectWithTag ("VictoryWindowText").GetComponent<Text> ();
	}

	public void SetVictoryTitleForWinner(WinningPlayer winner) {
		switch (winner) {
		case WinningPlayer.Player1:
			victoryTitle.text = "Player 1 Wins!";
			victoryText.text = defaultVictoryText;
			break;
		case WinningPlayer.Player2:
			victoryTitle.text = "Player 2 Wins!";
			victoryText.text = defaultVictoryText;
			break;

		default:
			victoryTitle.text = "Nobody Wins!";
			victoryText.text = "Great work, jerks, now no one gets a trophy.";
			break;
		}
	}
}

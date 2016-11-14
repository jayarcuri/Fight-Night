using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Eppy;

public class GameDirector : MonoBehaviour {
	public EndGameMenuController victoryWindowController;
	public bool timeOver;
	public HitEffectsManager shaker;
	public CharacterManager[] characters;
	public HitboxController[] pendingAttackHitboxes;

	// Use this for initialization
	void Start () {
		timeOver = false;
		victoryWindowController = GameObject.FindGameObjectWithTag ("VictoryWindow").GetComponent<EndGameMenuController> ();
		victoryWindowController.gameObject.SetActive (false);
		foreach (CharacterManager characterManager in characters) {
			if (characterManager == null) {
				throw new UnityException ("We are missing a player's character.");
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (shaker.shakeCounter >= 0) {
			shaker.StepShakeForward ();
		} else {
			bool[] hitsOccurred = new bool[2];
			Tuple<MoveFrame, bool>[] currentFrames = new  Tuple<MoveFrame, bool>[2];
			for (int i = 0; i < characters.Length; i++) {
				// Execute current move
				currentFrames [i] = characters [i].GetCurrentFrame ();
			}
			Tuple<Vector2, Vector2> newVelocities = ResolveCharacterCollisions (currentFrames [0].Item1, currentFrames [1].Item1);
			if (!newVelocities.Item1.Equals (CollisionUtils.NaV2)) {
				characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, newVelocities.Item1, currentFrames [0].Item2);
				characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, newVelocities.Item2, currentFrames [1].Item2);

			} else {
				characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, currentFrames [0].Item1.movementDuringFrame, currentFrames [0].Item2);
				characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, currentFrames [1].Item1.movementDuringFrame, currentFrames [1].Item2);
			}

			for (int i = 0; i < characters.Length; i++) {
				hitsOccurred [i] = characters [i].ResolveAttackCollisions ();

				if (hitsOccurred [i]) {
					
				}
			}

			for (int i = 0; i < characters.Length; i++) {
				characters [i].UpdateCharacterState ();
			}

			if (hitsOccurred [0] || hitsOccurred [1]) {
				shaker.SetCameraToShake ();
				for (int i = 0; i < characters.Length; i++) {
					if (hitsOccurred [i]) {
						MoveType lastFrameMoveType = characters [i].GetLastFrameMoveType () == MoveType.BLOCKING 
							? MoveType.BLOCKING 
							: MoveType.IN_HITSTUN;
						characters [i].SetCharacterLight (true, lastFrameMoveType);
					}
				}
			}
		}

		CheckIfGameHasEnded ();
	}

	Tuple<Vector2, Vector2> ResolveCharacterCollisions (MoveFrame player1Frame, MoveFrame player2Frame) {
		Transform player1Location = characters[0].gameObject.transform;
		Transform player2Location = characters[1].gameObject.transform;
		Vector2 player1Velocity = player1Frame.movementDuringFrame;
		Vector2 player2Velocity = player2Frame.movementDuringFrame;
		Vector2 newPlayer1Velocity = player1Velocity;
		Vector2 newPlayer2Velocity = player2Velocity;

		Tuple<Vector2, Vector2> newVelocities = CollisionUtils.GetUpdatedVelocities (player1Location, player1Velocity, player2Location, player2Velocity);

		newPlayer1Velocity = !newVelocities.Item1.Equals (CollisionUtils.NaV2) ? newVelocities.Item1 : newPlayer1Velocity;
		newPlayer2Velocity = !newVelocities.Item2.Equals (CollisionUtils.NaV2) ? newVelocities.Item2 : newPlayer2Velocity;

		Vector2 levelConstrainedP1Velocity = CollisionUtils.GetLevelConstraintedVelocity (player1Location, player1Velocity);
		Vector2 levelConstrainedP2Velocity = CollisionUtils.GetLevelConstraintedVelocity (player2Location, player2Velocity);
		bool p1VelocityChanged = !levelConstrainedP1Velocity.Equals (newPlayer1Velocity) && !float.IsNaN (levelConstrainedP1Velocity.x);
		bool p2VelocityChanged = !levelConstrainedP2Velocity.Equals (newPlayer2Velocity) && !float.IsNaN (levelConstrainedP2Velocity.x);

		newPlayer1Velocity = p1VelocityChanged ? levelConstrainedP1Velocity : newPlayer1Velocity;
		newPlayer2Velocity = p2VelocityChanged ? levelConstrainedP2Velocity : newPlayer2Velocity;

		if (p1VelocityChanged) {
			float newXVelocity = CollisionUtils.GetNonOverlappingXVelocity(player1Location, newPlayer1Velocity.x, player2Location, newPlayer2Velocity.x);
			if (!float.IsNaN (newXVelocity)) {
				newPlayer2Velocity = new Vector2 (newXVelocity, newPlayer2Velocity.y);
			}
		} else if (p2VelocityChanged) {
			float newXVelocity = CollisionUtils.GetNonOverlappingXVelocity(player2Location, newPlayer2Velocity.x, player1Location, newPlayer1Velocity.x);
			if (!float.IsNaN (newXVelocity)) {
				newPlayer1Velocity = new Vector2 (newXVelocity, newPlayer1Velocity.y);
			}
		}

		return new Tuple<Vector2, Vector2>(newPlayer1Velocity, newPlayer2Velocity);
	}

	public void RestartGame() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		Time.timeScale = 1;
	}

	void CheckIfGameHasEnded () {
		int player1Health = characters [0].characterDataManager.GetCurrentHealth ();
		int player2Health = characters [1].characterDataManager.GetCurrentHealth ();
		if (player1Health < 1 || player2Health < 1 || timeOver) {
			EndGameWithWinner (GetWinner (player1Health, player2Health));
		}
	}

	WinningPlayer GetWinner (int player1Health, int player2Health) {
		WinningPlayer winner;
		if (player1Health < 1 && player2Health < 1 || player1Health == player2Health) {
				winner = WinningPlayer.None;
			} else if (player1Health < player2Health) {
				winner = WinningPlayer.Player2;
			} else {
				winner = WinningPlayer.Player1;
		}
		return winner;
	}

	void EndGameWithWinner (WinningPlayer winner) {
		victoryWindowController.gameObject.SetActive (true);
		victoryWindowController.SetVictoryTitleForWinner (winner);
		Time.timeScale = 0;
	}


}

public enum WinningPlayer {
	Player1,
	Player2,
	None
}

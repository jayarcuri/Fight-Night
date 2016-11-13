using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Eppy;

public class GameDirector : MonoBehaviour {
	public EndGameMenuController victoryWindowController;
	public bool timeOver;
	public ScreenShakeTest shaker;
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
			Tuple<MoveFrame, bool>[] currentFrames = new  Tuple<MoveFrame, bool>[2];
			for (int i = 0; i < characters.Length; i++) {
				// Execute current move
				currentFrames [i] = characters [i].GetCurrentFrame ();
			}
			// TODO: 4: Check & resolve collisions (hits & bumps)
			Tuple<Vector2, Vector2> newVelocities = ResolveCharacterCollisions (currentFrames [0].Item1, currentFrames [1].Item1);
			if (!newVelocities.Item1.Equals (CollisionUtils.NaV2)) {
				characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, newVelocities.Item1, currentFrames [0].Item2);
				characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, newVelocities.Item2, currentFrames [1].Item2);

			} else {
				characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, currentFrames [0].Item1.movementDuringFrame, currentFrames [0].Item2);
				characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, currentFrames [1].Item1.movementDuringFrame, currentFrames [1].Item2);
			}

			for (int i = 0; i < characters.Length; i++) {
				bool hitOccurred = characters [i].ResolveAttackCollisions ();

				if (hitOccurred) {
					shaker.SetCameraToShake ();
				}
			}
			// 5: Update character/game/UI state.
			for (int i = 0; i < characters.Length; i++) {
				characters [i].UpdateCharacterState ();
			}
		}

		CheckIfGameHasEnded ();
	}

	Tuple<Vector2, Vector2> ResolveCharacterCollisions (MoveFrame player1Frame, MoveFrame player2Frame) {
		//	---------		---------
		//	|		|		|		|
		//	|		|		|		|
		//	|		a |---| b		|
		//	|		|		|		|
		//	|		|		|		|
		//	---------		---------
		//
		//	Gets character velocities which are legal in regards to avoiding character overlap & remaining within the bounds of the game.
		Transform player1Location = characters[0].gameObject.transform;
		Transform player2Location = characters[1].gameObject.transform;
		Vector2 player1Velocity = player1Frame.movementDuringFrame;
		Vector2 player2Velocity = player2Frame.movementDuringFrame;
		Vector2 newPlayer1Velocity = player1Velocity;
		Vector2 newPlayer2Velocity = player2Velocity;

		Tuple<Vector2, Vector2> newVelocities = CollisionUtils.GetUpdatedVelocities (player1Location, player1Velocity, player2Location, player2Velocity);
		if (!newVelocities.Item1.Equals (CollisionUtils.NaV2)) {
			newPlayer1Velocity = newVelocities.Item1;
		}

		if (!newVelocities.Item2.Equals (CollisionUtils.NaV2)) {
			newPlayer2Velocity = newVelocities.Item2;

			if (newPlayer2Velocity.x > 1f || newPlayer2Velocity.x < -1f ) {
				Debug.Log ("Done fucked up");
			}

		}

		Vector2 modifiedP1Velocities = CollisionUtils.GetLevelConstraintedVelocity (player1Location, player1Velocity);
		Vector2 modifiedP2Velocities = CollisionUtils.GetLevelConstraintedVelocity (player2Location, player2Velocity);
		bool p1HorizontalVelocityChanged = modifiedP1Velocities.x != newPlayer1Velocity.x && !float.IsNaN(modifiedP1Velocities.x);
		bool p2HorizontalVelocityChanged = modifiedP2Velocities.x != newPlayer2Velocity.x && !float.IsNaN(modifiedP2Velocities.x);

		if (!modifiedP1Velocities.Equals(newPlayer1Velocity) && !float.IsNaN(modifiedP1Velocities.x)) {
			newPlayer1Velocity = modifiedP1Velocities;
		}
		if (!modifiedP2Velocities.Equals(newPlayer2Velocity) && !float.IsNaN(modifiedP2Velocities.x)) {
			newPlayer2Velocity = modifiedP2Velocities;
		}

		if (p1HorizontalVelocityChanged) {
			float newXVelocity = CollisionUtils.GetNonOverlappingXVelocity(player1Location, newPlayer1Velocity.x, player2Location, newPlayer2Velocity.x);
			if (!float.IsNaN (newXVelocity)) {
				newPlayer2Velocity = new Vector2 (newXVelocity, newPlayer2Velocity.y);
			}
		} else if (p2HorizontalVelocityChanged) {
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

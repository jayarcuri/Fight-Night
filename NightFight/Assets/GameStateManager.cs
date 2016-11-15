using UnityEngine;

public enum GameState {
	INSTRUCTIONS,
	GAME_RUNNING,
	GAME_OVER
}

public class GameStateManager : MonoBehaviour {

	public GameState currentGameState;

	void Start () {
		currentGameState = GameState.INSTRUCTIONS;
	}

	public GameState GetCurrentGameState () {
		return currentGameState;
	}

	public void SetCurrentGameState (GameState newState) {
		currentGameState = newState;
	}

}

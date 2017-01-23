public class GameStateManager {

	public GameState currentGameState;
	protected static GameStateManager gameStateManagerSingleton;

	protected GameStateManager () {
		this.currentGameState = GameState.GAME_RUNNING;
	}

	public static GameStateManager GetGameStateManager () {
		if (gameStateManagerSingleton == null) {
			gameStateManagerSingleton = new GameStateManager ();
		}

		return gameStateManagerSingleton;
	}

	public static GameState GetCurrentGameState () {
		GameStateManager singleton = GameStateManager.GetGameStateManager ();

		return singleton.currentGameState;
	}

	public static void SetCurrentGameState (GameState newState) {
		GameStateManager singleton = GameStateManager.GetGameStateManager ();
		singleton.currentGameState = newState;
	}

}

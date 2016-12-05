using UnityEngine;
using System.Collections;

public class HitEffectsManager : MonoBehaviour {
	Camera cameraRef;
	float originalCameraSize;
	public int shakeCounter { get; private set; }

	// Use this for initialization
	void Start () {
		cameraRef = GetComponent<Camera> ();
		originalCameraSize = cameraRef.orthographicSize;
		shakeCounter = -1;
	}

	public void StepShakeForward () {
		shakeCounter++;
		if (shakeCounter < 12) {
			cameraRef.orthographicSize = cameraRef.orthographicSize == originalCameraSize ?
				originalCameraSize * 98f / 100f : originalCameraSize;
		} else {
			GameStateManager.SetCurrentGameState (GameState.GAME_RUNNING);
			shakeCounter = -1;
		}

	}

	public void SetCameraToShake () {
		GameStateManager.SetCurrentGameState (GameState.HIT_SHAKE);
		shakeCounter = 0;
	}

}

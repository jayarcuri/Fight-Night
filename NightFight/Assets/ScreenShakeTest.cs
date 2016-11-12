using UnityEngine;
using System.Collections;

public class ScreenShakeTest : MonoBehaviour {
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
				originalCameraSize * 99f / 100f : originalCameraSize;
		} else {
			shakeCounter = -1;
		}

	}

	public void SetCameraToShake () {
		shakeCounter = 0;
	}

}

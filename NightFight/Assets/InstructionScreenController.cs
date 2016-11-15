using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionScreenController : MonoBehaviour {
	Text startGameText;
	GameStateManager stateManager;

	// Use this for initialization
	void Start () {
		startGameText = GetComponentInChildren<Text> ();
		stateManager = GameObject.FindGameObjectWithTag ("GameDirector").GetComponent<GameStateManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		Color newColor = startGameText.color;
		newColor.a = Mathf.Lerp(1f, 0.1f, Mathf.Sin (Time.time * 2f));
		startGameText.color = newColor;

		if (Input.GetButton ("P1_Block") && Input.GetButton ("P1_Light") && Input.GetButton ("P2_Light") && Input.GetButton ("P2_Block")) {
			stateManager.SetCurrentGameState (GameState.GAME_RUNNING);
			gameObject.SetActive (false);
		}
	}
}

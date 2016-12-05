using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionScreenController : MonoBehaviour {
	Text startGameText;

	// Use this for initialization
	void Start () {
		startGameText = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		Color newColor = startGameText.color;
		newColor.a = Mathf.Lerp(1f, 0.1f, Mathf.Sin (Time.time * 2f));
		startGameText.color = newColor;

		if (Input.GetButton ("p1_block") && Input.GetButton ("p1_light") && Input.GetButton ("p2_light") && Input.GetButton ("p2_block")) {
			GameStateManager.SetCurrentGameState (GameState.GAME_RUNNING);
			gameObject.SetActive (false);
		}
	}
}

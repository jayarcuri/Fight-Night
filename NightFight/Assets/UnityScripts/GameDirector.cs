using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameDirector : MonoBehaviour {
	public GameObject victoryWindow;
	public CharacterManager[] characters;
	public HitboxController[] pendingAttackHitboxes;

	// Use this for initialization
	void Start () {
		foreach (CharacterManager characterManager in characters) {
			if (characterManager == null) {
				throw new UnityException ("We are missing a player's character.");
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		bool[] isLit = new bool[2];
		MoveType[] currentMoveType = new MoveType[2];

		for (int i = 0; i < characters.Length; i++) {
			// Execute current move
			characters[i].ExecuteCurrentFrame();
		}
			// TODO: 4: Check & resolve collisions (hits & bumps)
			//
			// 	Try to move characters
			//	If (either would be outside the bounds of the game
			// 		change destination to be just within the bounds
			//	If (
			//
		for (int i = 0; i < characters.Length; i++) {
			characters [i].ResolveCollisions ();
		}
//			AttackFrameData hit = pendingAttackHitbox.GetCurrentHitFrame ();
//			// TODO: make sure to look into this; hitbox should not have return a null frame.
//			if (hit != null) {
//				bool characterWasHit = characterManager.ProcessHitFrame (hit, previousFrame);
//				if (characterWasHit) {
//					healthBar.UpdateHealthBar (characterManager.GetCurrentHealth ());
//
//					pendingAttackHitbox.Reset ();
//					pendingAttackHitbox = null;
//					// TODO: remove horrid temporary win code from here
//					if (characterManager.GetCurrentHealth () <= 0) {
//						victoryWindow.SetActive (true);
//						Time.timeScale = 0;
//					}
//				}
//			}
//		}
			// 5: Update character/game/UI state.
		for (int i = 0; i < characters.Length; i++) {
			characters [i].UpdateCharacterState ();
		}

	}

	// If there is not a non-active frame between two active frames and a hit has already been dealt w/,
	// this class will not allows the hitbox to be reapplied.


}

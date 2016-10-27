using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Eppy;

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

	// assumes a collision has occurred
	Tuple<Vector2, Vector2> ResolveCharacterCollisions () {
		//	---------		---------
		//	|		|		|		|
		//	|		|		|		|
		//	|		a |---| b		|
		//	|		|		|		|
		//	|		|		|		|
		//	---------		---------
		//
		// get character positions, presumptive locations after frame concludes
		CharacterManager player1 = characters[0];
		CharacterManager player2 = characters[1];

		float p1Width;
		float p2Width;
		MoveFrame player1Frame = characters[0];
		MoveFrame player2Frame = characters[1];
		Vector3 player1Location = characters[0].gameObject.transform;
		Vector3 player2Location = characters[1].gameObject.transform;
		Vector2 player1Velocity = player1Frame.movementDuringFrame;
		Vector2 player2Velocity = player2Frame.movementDuringFrame;

		return CollisionUtils.GetUpdatedVelocities (player1Location, player1Velocity, player2Location, player2Velocity);

//		if (p1Location.x < p2Location.x) {
//			a = p1Location.x + p1Width;
//			b = p2Location - p2Width;
//		} else if (p1Location.x > p2Location.x) {
//			a = p1Location.x + p1Width;
//			b = p2Location - p2Width;
//		}
//
//		if (a + p1Movement.x > b + p2Movement.x) {
//			//	an overlap will occur
//			//	where?
//
//		}

		// if both have the same vector orientation...
//		if ((p1Movement >= 0 && p2Movement >= 0) || (p1Movement <= 0 && p2Movement <= 0)) {
//			if (p1Movement > p2Movement) {
//				p2Movement = p1Movement;
//			} else {
//				p1Movement = p2Movement;
//			}
//		} 
//		// otherwise the collision is a conflict-collision (opposite oriented vectors)
//		else if (p1Movement > p2Movement || p1Movement < p2Movement) {
//			if (p1Movement > p2Movement) {
//				float newMovement = p1Movement - p2Movement;
//				}
//		}
	}


}

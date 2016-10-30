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
		Tuple<MoveFrame, bool>[] currentFrames = new  Tuple<MoveFrame, bool>[2];
		for (int i = 0; i < characters.Length; i++) {
			// Execute current move
			currentFrames[i] = characters[i].GetCurrentFrame ();
		}
		// TODO: 4: Check & resolve collisions (hits & bumps)
		Tuple<Vector2, Vector2> newVelocities = ResolveCharacterCollisions (currentFrames [0].Item1, currentFrames [1].Item1);
		if (!newVelocities.Item1.Equals (CollisionUtils.NaV2)) {
			//currentFrames[0].Item1
			characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, newVelocities.Item1, currentFrames [0].Item2);
			characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, newVelocities.Item2, currentFrames [1].Item2);
				
		} else {
			characters [0].ExecuteCurrentFrame (currentFrames [0].Item1, currentFrames[0].Item1.movementDuringFrame, currentFrames [0].Item2);
			characters [1].ExecuteCurrentFrame (currentFrames [1].Item1, currentFrames [1].Item1.movementDuringFrame, currentFrames [1].Item2);
		}
		//	If (either would be outside the bounds of the game)

		// 		change destination to be just within the bounds

		//	If (
		//
		for (int i = 0; i < characters.Length; i++) {
			characters [i].ResolveCollisions ();
		}
		// 5: Update character/game/UI state.
		for (int i = 0; i < characters.Length; i++) {
			characters [i].UpdateCharacterState ();
		}

	}

	// assumes a collision has occurred
	Tuple<Vector2, Vector2> ResolveCharacterCollisions (MoveFrame player1Frame, MoveFrame player2Frame) {
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
		Vector3 player1Location = characters[0].gameObject.transform.localPosition;
		Vector3 player2Location = characters[1].gameObject.transform.localPosition;
		Vector2 player1Velocity = player1Frame.movementDuringFrame;
		Vector2 player2Velocity = player2Frame.movementDuringFrame;

		return CollisionUtils.GetUpdatedVelocities (player1Location, player1Velocity, player2Location, player2Velocity);
	}


}

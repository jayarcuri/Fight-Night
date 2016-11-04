﻿using UnityEngine;
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

			if (newPlayer2Velocity.x > 1f || newPlayer2Velocity.x < -1f ) {
				Debug.Log ("Done fucked up");
			}

		}

		if (p1HorizontalVelocityChanged) {
			float newXVelocity = CollisionUtils.GetNonOverlappingXVelocity(player1Location, newPlayer1Velocity.x, player2Location, newPlayer2Velocity.x);
			if (!float.IsNaN (newXVelocity)) {
				newPlayer2Velocity = new Vector2 (newXVelocity, newPlayer2Velocity.y);

				if (newPlayer2Velocity.x > 1f || newPlayer2Velocity.x < -1f ) {
					Debug.Log ("Done fucked up");
				}

			}
		} else if (p2HorizontalVelocityChanged) {
			float newXVelocity = CollisionUtils.GetNonOverlappingXVelocity(player2Location, newPlayer2Velocity.x, player1Location, newPlayer1Velocity.x);
			if (!float.IsNaN (newXVelocity)) {
				newPlayer1Velocity = new Vector2 (newXVelocity, newPlayer1Velocity.y);
			}
		}

		return new Tuple<Vector2, Vector2>(newPlayer1Velocity, newPlayer2Velocity);
	}


}

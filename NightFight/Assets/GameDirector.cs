using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameDirector : MonoBehaviour {
	public GameObject victoryWindow;
	CharacterManager[] characters;
	public HitboxController[] pendingAttackHitboxes;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;

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
			// 1: Get input.
			AttackType attack;
			DirectionalInput directionalInput;
			bool toggleIllumination = false;
			CharacterManager characterManager = characters [i];
			characterManager.inputManager.GetInputs (out directionalInput, out attack, out toggleIllumination);

			if (toggleIllumination) {
				characterManager.characterDataManager.ToggleCharacterIllumination ();
			}

			if (!characterManager.characterMovement.isFacingRight) {
				directionalInput.FlipHorizontalInput ();
			}
			// 2: Execute input.
			MoveFrame currentFrame = null;
			currentFrame = characterManager.characterDataManager.GetCurrentFrame (directionalInput, attack, out isLit[i]);
			// 3: Execute frame.
			if (currentFrame != null) {
				characterManager.ExecuteFrame (currentFrame);
			}

			currentMoveType[i] = currentFrame != null ? currentFrame.moveType : MoveType.NONE;
			characterManager.characterLight.SetLight(isLit[i], currentMoveType[i]);
		}
			// TODO: 4: Check & resolve collisions (hits & bumps)
			//
			//
			// 5: Update character/game/UI state.
		for (int i = 0; i < characters.Length; i++) {
			characters [i].UpdateCharacterLight ();
		}



	}

	// If there is not a non-active frame between two active frames and a hit has already been dealt w/,
	// this class will not allows the hitbox to be reapplied.


}

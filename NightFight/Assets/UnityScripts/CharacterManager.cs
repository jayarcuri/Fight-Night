using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Eppy;
// Tracks all components related to a given player's avatar.
public class CharacterManager : MonoBehaviour {
	public CharacterDataManager characterDataManager;
	public CharacterLightController characterLight;
	public CharacterMovement characterMovement;
	public TriggerCollisionManager collisionManager;
	public CharacterGuiController guiController;
	public HitboxController hitBox;
	public InputManager inputManager;

	public bool isPlayer1;
	public bool isBot;
	public int botDirectionalInputRaw;
	public float overrideWalkSpeed;
	public ButtonInputCommand botAttackInput;

	MoveFrame lastExecutedFrame;

	// Use this for initialization
	void Start () {
		characterDataManager = new CharacterDataManager ();
		if (overrideWalkSpeed > 0) {
			characterDataManager.SetWalkSpeed (overrideWalkSpeed);
		}
		inputManager = GetComponent<InputManager> ();
		characterMovement = GetComponent<CharacterMovement> ();
		characterLight = GetComponent<CharacterLightController> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		collisionManager = GetComponent<TriggerCollisionManager> ();
		collisionManager.parentCharacter = this;
		characterLight.SetLight (false, MoveType.NONE);

		string opponentCharacterTag;
		string characterGuiTag;
		if (isPlayer1) {
			opponentCharacterTag = "Player2";
			characterGuiTag = "Player1Gui";
		} else {
			opponentCharacterTag = "Player1";
			characterGuiTag = "Player2Gui";
		}
		characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag (opponentCharacterTag).GetComponent <Transform> ());
		guiController = GameObject.FindGameObjectWithTag (characterGuiTag).GetComponentInChildren <CharacterGuiController> ();

		guiController.maxHealth = characterDataManager.GetCurrentHealth ();
	}

	public Tuple<MoveFrame, bool> GetCurrentFrame () {
		ButtonInputCommand attack;
		DirectionalInput directionalInput;
		bool toggleIllumination = false;


		ButtonInput[] buttons = new ButtonInput[0];
		bool hey = false;
		// 1: Get input.
		if (isBot) {
			directionalInput = new DirectionalInput (botDirectionalInputRaw);
			attack = botAttackInput;
			toggleIllumination = true;
		} else {
			inputManager.GetInputs (out directionalInput, out attack, out toggleIllumination);
			inputManager.NewGetInputs (out buttons, out hey);
		}

		if (toggleIllumination) {
			characterDataManager.ToggleCharacterIllumination ();
		}

		if (!characterMovement.isFacingRight) {
			directionalInput.FlipHorizontalInput ();
		}
		// 2: Execute input.
		MoveFrame currentFrame = null;
		bool isLit;

		currentFrame = characterDataManager.GetCurrentFrame (directionalInput, attack, out isLit);

		return new Tuple<MoveFrame, bool>(currentFrame, isLit);
	}

	public void ExecuteFrame(MoveFrame currentFrame, Vector2 movementDuringFrame, bool isLit) {
		// 3: Execute frame.
		if (currentFrame != null) {
			PerformFrame (currentFrame, movementDuringFrame);
		}

//		MoveType currentMoveType = currentFrame != null ? currentFrame.moveType : MoveType.NONE;
//		characterLight.SetLight(isLit, currentMoveType);
	}

	public bool ResolveAttackCollisions () {
		if (collisionManager.HasPendingHit ()) {
			AttackFrameData hit = collisionManager.GetPendingHit ();

			bool wasHit = characterDataManager.ProcessHitFrame (hit, lastExecutedFrame);

			if (wasHit) {
				guiController.UpdateHealthBar (characterDataManager.GetCurrentHealth ());
				collisionManager.ClearPendingHit ();
				return true;
			}
		}
		return false;
	}

	public void UpdateCharacterState () {
		UpdateCharacterLight ();
		if (lastExecutedFrame.moveType == MoveType.NONE) {
			characterMovement.FlipRotation ();
		}
	}

	public void PerformFrame (MoveFrame currentFrame, Vector2 movementDuringFrame)
	{
		if (movementDuringFrame != Vector2.zero) {
			characterMovement.MoveByVector (movementDuringFrame);
		} 

		if (currentFrame.attackData != null && (lastExecutedFrame == null || !currentFrame.attackData.Equals (lastExecutedFrame.attackData))) { 
				hitBox.ExecuteAttack (currentFrame.attackData);
		} else if (currentFrame.attackData == null && hitBox.IsLoaded ()) {
				hitBox.Reset ();
		}

		lastExecutedFrame = currentFrame;
	}

	public void UpdateCharacterLight() {
		bool isSelfIlluminated = characterDataManager.IsInIlluminatedState ();
		if (lastExecutedFrame != null) {
			isSelfIlluminated = isSelfIlluminated || lastExecutedFrame.isLit;
		}

		MoveType currentMoveType = lastExecutedFrame != null ? lastExecutedFrame.moveType : MoveType.NONE;

		characterLight.SetLight (isSelfIlluminated, currentMoveType);
	}

	public void SetCharacterLight(bool lightShouldBeOn, MoveType forMoveType) {
		characterLight.SetLight (lightShouldBeOn, forMoveType);
	}

	public void EquipOrb (OrbController orb) {
		Debug.Log ("Orb was equipped");
		orb.currentState = OrbState.EQUIPPED;
		orb.lastOwner = this;
		orb.gameObject.SetActive (false);
		characterDataManager.isCarryingOrb = true;
		//  Replace magic figure with predefined variable within characterDataManager
		characterDataManager.MultiplyWalkSpeedByFactor (0.75f);
	}

	public MoveType GetLastFrameMoveType () {
		return lastExecutedFrame != null ? lastExecutedFrame.moveType : MoveType.NONE;
	}

	public bool IsBlockingOrHit () {
		//	will not count last frame as in blockstun
		MoveType lastMoveType = GetLastFrameMoveType ();
		bool isInBlockstun = lastMoveType == MoveType.BLOCKING && characterDataManager.currentMove.HasNext ();
		bool isInHitstun = lastMoveType == MoveType.IN_HITSTUN;
		return isInBlockstun || isInHitstun;
	}

}

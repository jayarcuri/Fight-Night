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
	int drainRateForHeldMoves;

	// Use this for initialization
	void Start () {
		characterDataManager = new CharacterDataManager ();
		if (overrideWalkSpeed > 0) {
			characterDataManager.SetWalkSpeed (overrideWalkSpeed);
		}
		drainRateForHeldMoves = characterDataManager.GetDrainRateForHeldMoves ();
		inputManager = GetComponent<InputManager> ();
		characterMovement = GetComponent<CharacterMovement> ();
		characterLight = GetComponent<CharacterLightController> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		collisionManager = GetComponent<TriggerCollisionManager> ();
		guiController.maxHealth = characterDataManager.GetCurrentHealth ();
		characterLight.SetLight (false, MoveType.NONE);
		if (isPlayer1) {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player2").GetComponent <Transform> ());
		} else {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player1").GetComponent <Transform> ());
		}
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
			//inputManager.NewGetInputs (out buttons, out hey);
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

		if ((lastExecutedFrame != null && lastExecutedFrame.canBeExtended) 
			&& (characterDataManager.currentMove != null && characterDataManager.currentMove.SequenceStartedWithButton (buttons))
			&& characterDataManager.GetIlluminationCount () >= drainRateForHeldMoves) {
			currentFrame = lastExecutedFrame;
			isLit = currentFrame.isLit || characterDataManager.isSelfIlluminated;
			characterDataManager.SubtractFromIllumination (drainRateForHeldMoves);
		} else {
			currentFrame = characterDataManager.GetCurrentFrame (directionalInput, attack, out isLit);
		}

		return new Tuple<MoveFrame, bool>(currentFrame, isLit);
	}

	public void ExecuteFrame(MoveFrame currentFrame, Vector2 movementDuringFrame, bool isLit) {
		// 3: Execute frame.
		if (currentFrame != null) {
			PerformFrame (currentFrame, movementDuringFrame);
		}

		MoveType currentMoveType = currentFrame != null ? currentFrame.moveType : MoveType.NONE;
		characterLight.SetLight(isLit, currentMoveType);
	}

	public bool ResolveAttackCollisions () {
		if (collisionManager.HasPendingHit ()) {
			AttackFrameData hit = collisionManager.GetPendingHit ();

			bool wasHit = characterDataManager.ProcessHitFrame (hit, lastExecutedFrame);

			if (wasHit) {
				guiController.UpdateguiController (characterDataManager.GetCurrentHealth ());
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
		//	Illumination stuff
		if (lastExecutedFrame.moveType != MoveType.SPECIAL && (characterLight.LightEnabled () || collisionManager.IsInAnIlluminatedArea ())) {
			characterDataManager.IncrementIlluminationCounter ();
		}

		guiController.UpdateChargeBar (characterDataManager.GetIlluminationCount ());
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
		bool isLit;
		if (lastExecutedFrame != null) {
			isLit = characterDataManager.isSelfIlluminated || lastExecutedFrame.isLit;
		} else {
			isLit = characterDataManager.isSelfIlluminated;
		}
		MoveType currentMoveType = lastExecutedFrame != null ? lastExecutedFrame.moveType : MoveType.NONE;

		characterLight.SetLight (isLit, currentMoveType);
	}

	public void SetCharacterLight(bool lightShouldBeOn, MoveType forMoveType) {
		characterLight.SetLight (lightShouldBeOn, forMoveType);
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

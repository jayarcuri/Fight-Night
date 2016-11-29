using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Eppy;
// Tracks all components related to a given player's avatar.
public class CharacterManager : MonoBehaviour {

	public bool isPlayer1;
	public HitboxController hitBox;
	public CharacterMovement characterMovement;
	public CharacterDataManager characterDataManager;
	public InputManager inputManager;
	public CollisionManager collisionManager;
	public HealthBarController healthBar;
	public CharacterLightController characterLight;
	public GameObject victoryWindow;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;
	public float overrideWalkSpeed;

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
		collisionManager = GetComponent<CollisionManager> ();
		healthBar.maxHealth = characterDataManager.GetCurrentHealth ();
		characterLight.SetLight (false, MoveType.NONE);
		if (isPlayer1) {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player2").GetComponent <Transform> ());
		} else {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player1").GetComponent <Transform> ());
		}
	}

	public Tuple<MoveFrame, bool> GetCurrentFrame () {
		AttackType attack;
		DirectionalInput directionalInput;
		bool toggleIllumination = false;
		// 1: Get input.
		if (isBot) {
			directionalInput = new DirectionalInput (botDirectionalInputRaw);
			attack = botAttackInput;
			toggleIllumination = true;
		} else {
			inputManager.GetInputs (out directionalInput, out attack, out toggleIllumination);
			ButtonInput[] buttons = new ButtonInput[0];
			bool hey = false;
			inputManager.NewGetInputs (out buttons, out hey);

			if (buttons.Length != 0 && isPlayer1) {
				foreach (ButtonInput input in buttons) {
					Debug.Log (input.buttonType + " is currently " + input.buttonState);
				}

			}
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
		/*
		 *	if (the previous moveframe is sustainable && the current move sequence was executed with a button which is still being pressed) {
		 *		currentFrame = lastExecutedFrame;
		 *		isLit = currentFrame.isLit && characterDataManager.isCharacterLit ();
		 *	}
		 */
		currentFrame = characterDataManager.GetCurrentFrame (directionalInput, attack, out isLit);

		return new Tuple<MoveFrame, bool>(currentFrame, isLit);
	}

	public void ExecuteCurrentFrame(MoveFrame currentFrame, Vector2 movementDuringFrame, bool isLit) {
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

			// put this on CharacterManager
			bool wasHit = characterDataManager.ProcessHitFrame (hit, lastExecutedFrame);

			if (wasHit) {
				healthBar.UpdateHealthBar (characterDataManager.GetCurrentHealth ());
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
		if (characterLight.LightEnabled ()) {
			characterDataManager.IncrementIlluminationCounter ();
		}

		healthBar.UpdateChargeBar (characterDataManager.GetIlluminationCount ());

		if (characterDataManager.GetCurrentHealth () <= 0) {
			victoryWindow.SetActive (true);
		}
	}

	public void PerformFrame (MoveFrame currentFrame, Vector2 movementDuringFrame)
	{
		if (movementDuringFrame != Vector2.zero) {
			characterMovement.MoveByVector (movementDuringFrame);
		} 
		// Smelly Code Below
		if (currentFrame.attackData != null) { 
			if (lastExecutedFrame == null || !currentFrame.attackData.Equals (lastExecutedFrame.attackData)) {
				hitBox.ExecuteAttack (currentFrame.attackData);
			}
		} else {
			if (hitBox.IsLoaded ()) {
				hitBox.Reset ();
			}
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

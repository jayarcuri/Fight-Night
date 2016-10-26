using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// Tracks all components related to a given player's avatar.
public class CharacterManager : MonoBehaviour {

	public bool isPlayer1;
	public HitboxController hitBox;
	public CharacterMovement characterMovement;
	public CharacterDataManager characterDataManager;
	public InputManager inputManager;
	public CollisionManager collisionManager;
	public HitboxController pendingAttackHitbox;
	public HealthBarController healthBar;
	public CharacterLightController characterLight;
	public GameObject victoryWindow;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;

	MoveFrame lastExecutedFrame;

	// Use this for initialization
	void Start () {
		characterDataManager = new CharacterDataManager ();
		pendingAttackHitbox = null;
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

	public void ExecuteCurrentFrame () {
		AttackType attack;
		DirectionalInput directionalInput;
		bool toggleIllumination = false;
		// 1: Get input.
		inputManager.GetInputs (out directionalInput, out attack, out toggleIllumination);

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
		// 3: Execute frame.
		if (currentFrame != null) {
			UpdateCharacterBody (currentFrame);
		}

		MoveType currentMoveType = currentFrame != null ? currentFrame.moveType : MoveType.NONE;
		characterLight.SetLight(isLit, currentMoveType);
	}

	public void ResolveCollisions () {
		if (collisionManager.HasPendingHit ()) {
			AttackFrameData hit = collisionManager.GetPendingHit ();

			// put this on CharacterManager
			bool wasHit = characterDataManager.ProcessHitFrame (hit, lastExecutedFrame);

			if (wasHit) {
				healthBar.UpdateHealthBar (characterDataManager.GetCurrentHealth ());
				collisionManager.ClearPendingHit ();
			}
		}
	}

	public void UpdateCharacterState () {
		UpdateCharacterLight ();
		if (lastExecutedFrame.moveType == MoveType.NONE) {
			characterMovement.FlipRotation ();
		}

		if (characterDataManager.GetCurrentHealth () <= 0) {
			victoryWindow.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void UpdateCharacterBody (MoveFrame currentFrame)
	{
		if (currentFrame.movementDuringFrame != Vector2.zero) {
			characterMovement.MoveByVector (currentFrame.movementDuringFrame);
		} 
		// Smelly Code Below
		if (currentFrame.attackData != null) { 
			if (lastExecutedFrame == null || !currentFrame.attackData.Equals ((lastExecutedFrame.attackData))) {
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

}

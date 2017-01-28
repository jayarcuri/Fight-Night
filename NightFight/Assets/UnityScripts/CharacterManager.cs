using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Eppy;
// Tracks all components related to a given player's avatar.
public class CharacterManager : MonoBehaviour {
	public CharacterDataManager characterDataManager;
	CharacterLightController characterLight;
	CharacterMovement characterMovement;
	TriggerCollisionManager collisionManager;
	CharacterGuiController guiController;
	HitboxController hitBox;
	InputManager inputManager;
	OrbController orbController;

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
		// 1: Get input.
		if (isBot) {
			directionalInput = new DirectionalInput (botDirectionalInputRaw);
			attack = botAttackInput;
			toggleIllumination = true;
		} else {
			bool garbage = false;
			inputManager.GetInputs (out directionalInput, out attack, out toggleIllumination);
			inputManager.NewGetInputs (out buttons, out garbage);
		}

		if (toggleIllumination) {
			if (!characterDataManager.isCarryingOrb) {
				characterDataManager.ToggleCharacterIllumination ();
			} else {
				//  Throw orb
				ThrowOrb ();
			}
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
		orbController = orb;
	}

	void ThrowOrb () {
		orbController.gameObject.SetActive (true);
		orbController.currentState = OrbState.THROWN;
		characterDataManager.isCarryingOrb = false;
		//  Activate Hitbox
		//  Place in front of character
		orbController.transform.localPosition = this.transform.localPosition;
		//  Add force
		Rigidbody orbRigidbody = orbController.gameObject.GetComponent<Rigidbody> ();
		orbRigidbody.velocity = Vector3.zero;
		float directionalModifier = characterMovement.isFacingRight ? 1.0f : -1.0f;

		orbRigidbody.AddForce(new Vector2(10 * directionalModifier, 6), ForceMode.Impulse);

		orbController = null;
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

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*	
 * The purpose of this class is to act as the overseer for the 
 *	routine which composes each frame of any given character's
 *	"turn". This class should manipulate NO DATA.
 */
public class FrameManager : MonoBehaviour
{
	public bool isPlayer1;
	public HitboxController hitBox;
	public Light bodyLight;
	// To flip orientation if necessary. Move to CharacterMovement.
	public CharacterMovement characterMovement;
	public Text healthText;
	public GameObject victoryWindow;
	string defaultHealthText;
	CharacterState characterState;
	CharacterManager characterManager;
	InputManager InputManager;
	MoveFrame previousFrame;
	public HitboxController pendingAttackHitbox;

	void Start ()
	{
		characterManager = new CharacterManager ();
		characterState = new CharacterState (characterManager.GetStartingHealth());
		pendingAttackHitbox = null;
		InputManager = GetComponent<InputManager> ();
		characterMovement = GetComponent<CharacterMovement> ();
		bodyLight = GetComponentInChildren<Light> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		bodyLight.enabled = false;
		defaultHealthText = healthText.text;
		healthText.text = defaultHealthText + characterState.health;

		if (isPlayer1) {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player2").GetComponent <Transform> ());
		
		} else {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player1").GetComponent <Transform> ());
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		DirectionalInput directionalInput;
		AttackType attack;
		if (isPlayer1)
			InputManager.GetInputs (out directionalInput, out attack);
		else {
			directionalInput = DirectionalInput.Neutral;
			attack = AttackType.Block;
		}
		// if (the character manager doesn't have a queued frame), attempt to flip the rotation.
		if (!characterManager.HasQueuedFrames ()) {
			characterMovement.FlipRotation ();
		}

		bool isFacingRight = characterMovement.isFacingRight;

		MoveFrame currentFrame = characterManager.GetCurrentFrame (directionalInput, attack, isFacingRight);

		// AM I HIT?
		if (pendingAttackHitbox != null) {
			HitFrame hit = pendingAttackHitbox.GetCurrentHitFrame();
			MoveSequence inducedMoveSequence;
			if (currentFrame.moveType != MoveType.BLOCKING) {
				characterState.TakeDamage (hit.damage);
				healthText.text = defaultHealthText + characterState.health;
				inducedMoveSequence = pendingAttackHitbox.GetCurrentMoveHitstun ();
			} else {
				// TODO: would normally get blockstun frames
				inducedMoveSequence = pendingAttackHitbox.GetCurrentMoveHitstun ();
			}
			// TODO: remove horrid temporary win code from here
			if (characterState.health <= 0) {
				victoryWindow.SetActive (true);
				Time.timeScale = 0;
			}
			// TODO: assign hitstun to character
			characterManager.QueueMove(pendingAttackHitbox.GetCurrentMoveHitstun());
			pendingAttackHitbox.Reset ();
			pendingAttackHitbox = null;
		}

		if (currentFrame != null) {
			
			/*if (currentFrame.moveType == MoveType.BLOCKING) {
				Debug.Log ("Character is blocking.");
			}*/

			if (currentFrame.moveType == MoveType.STEP_BACK || currentFrame.moveType == MoveType.STEP_FORWARD) {
				characterMovement.Move (currentFrame);
			} else if (MoveType.IN_HITSTUN.Equals (currentFrame.moveType)) {
				float moveBy = -0.5f;
				characterMovement.MoveByVector (new Vector2 (moveBy, 0f));
			} else if (currentFrame.movementDuringFrame != Vector2.zero) {
				characterMovement.MoveByVector (currentFrame.movementDuringFrame);
			}
			else {
				ExecuteNextFrame (currentFrame);
			}
		}

		// TODO: implement collision checking.

		if (attack != AttackType.None)
			bodyLight.enabled = true;
		else
			bodyLight.enabled = false;

		previousFrame = currentFrame;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
			}
	}

	void OnTriggerStay (Collider other) {
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
		}
	}

	void ProcessHitboxCollision(Collider hitboxObject) {
		HitboxController attackingHitbox = hitboxObject.GetComponent <HitboxController> ();
		if (attackingHitbox.IsLoaded ()) {
			// if not blocking or not immune to attack
			if (characterMovement.action != CharacterAction.Blocking || characterMovement.action != CharacterAction.BlockStunned) {
				pendingAttackHitbox = attackingHitbox;
			}
		}
	}

	// If there is not a non-active frame between two active frames and a hit has already been dealt w/,
	// this class will not allows the hitbox to be reapplied.
	void ExecuteNextFrame (MoveFrame currentMoveFrame)
	{
		if (currentMoveFrame.moveType == MoveType.ACTIVE) {
			if (previousFrame.moveType != MoveType.ACTIVE) {
				HitFrame attackFrame = (HitFrame)currentMoveFrame;
				hitBox.ExecuteAttack (attackFrame.offset, attackFrame.size, attackFrame);
			} 
//			else {
//				Debug.Log ("Move already executed and resolved.");
//			}
			// TODO: add movement related stuff here
		} else if (hitBox.IsLoaded () && currentMoveFrame.moveType != MoveType.ACTIVE)
			hitBox.Reset ();
	}



}


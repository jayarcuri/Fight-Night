using UnityEngine;
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
	CharacterState characterState;
	CharacterManager characterManager;
	InputController inputController;
	MoveFrame previousFrame;
	public HitboxController pendingAttackHitbox;

	void Start ()
	{
		characterManager = new CharacterManager ();
		characterState = new CharacterState (characterManager.GetStartingHealth());
		pendingAttackHitbox = null;
		inputController = GetComponent<InputController> ();
		characterMovement = GetComponent<CharacterMovement> ();
		bodyLight = GetComponentInChildren<Light> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		bodyLight.enabled = false;

		if (isPlayer1) {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player2").GetComponent <Transform> ());
		
		} else {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player1").GetComponent <Transform> ());
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//		if (isPlayer1) {
		DirectionalInput directionalInput;
		AttackType attack;
		if (isPlayer1)
			inputController.GetInputs (out directionalInput, out attack);
		else {
			directionalInput = DirectionalInput.Neutral;
			attack = AttackType.None;
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
			characterState.TakeDamage (hit.damage);
			// TODO: assign hitstun to character
			characterManager.QueueMove(pendingAttackHitbox.GetCurrentMoveHitstun());
			pendingAttackHitbox.Reset ();
			pendingAttackHitbox = null;
		}

		if (currentFrame != null) {
			if (currentFrame.moveType == MoveType.STEP_BACK || currentFrame.moveType == MoveType.STEP_FORWARD) {
				characterMovement.Move (currentFrame);
			} else if (MoveType.IN_HITSTUN.Equals (currentFrame.moveType)) {
				float moveBy = -0.5f;
				characterMovement.MoveByVector (new Vector3 (moveBy, 0f, 0f));
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


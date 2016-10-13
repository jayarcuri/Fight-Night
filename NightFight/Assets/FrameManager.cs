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
	CharacterManager characterManager;
	InputManager InputManager;
	MoveFrame previousFrame;
	public HitboxController pendingAttackHitbox;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;

	void Start ()
	{
		characterManager = new CharacterManager ();
		pendingAttackHitbox = null;
		InputManager = GetComponent<InputManager> ();
		characterMovement = GetComponent<CharacterMovement> ();
		bodyLight = GetComponentInChildren<Light> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		bodyLight.enabled = false;
		defaultHealthText = healthText.text;
		healthText.text = defaultHealthText + characterManager.GetCurrentHealth ();

		if (isPlayer1) {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player2").GetComponent <Transform> ());
		
		} else {
			characterMovement.SetOpponentTransform (GameObject.FindGameObjectWithTag ("Player1").GetComponent <Transform> ());
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// AM I HIT?
		if (pendingAttackHitbox != null) {
			AttackFrameData hit = pendingAttackHitbox.GetCurrentHitFrame ();
			// TODO: make sure to look into this; hitbox should not have return a null frame.
			if (hit != null) {
				bool characterWasHit = characterManager.ProcessHitFrame (hit, previousFrame);
				if (characterWasHit) {
					healthText.text = defaultHealthText + characterManager.GetCurrentHealth ();
					pendingAttackHitbox.Reset ();
					pendingAttackHitbox = null;
					// TODO: remove horrid temporary win code from here
					if (characterManager.GetCurrentHealth () <= 0) {
						victoryWindow.SetActive (true);
						Time.timeScale = 0;
					}
				}
			}
		}

		DirectionalInput directionalInput;
		AttackType attack;

		if (isBot) {
			directionalInput = new DirectionalInput (botDirectionalInputRaw);
			attack = botAttackInput;
		} else {
			InputManager.GetInputs (out directionalInput, out attack);
		}

		// if (the character manager doesn't have a queued frame), attempt to flip the rotation.
		if (!characterManager.HasQueuedFrames ()) {
			characterMovement.FlipRotation ();
		}
		// unneccesary unless orientation has flipped
		bool isFacingRight = characterMovement.isFacingRight;

		if (!isFacingRight) {
			directionalInput.FlipHorizontalInput ();
		}

		MoveFrame currentFrame = null;

		currentFrame = characterManager.GetCurrentFrame (directionalInput, attack);

		// Attempt to move (should be put into ExecuteFrame()
		if (currentFrame != null) {
			// TODO: remove this garbage for walking
			if (currentFrame.moveType == MoveType.STEP_BACK || currentFrame.moveType == MoveType.STEP_FORWARD) {
				characterMovement.Move (currentFrame);
			} else if (currentFrame.movementDuringFrame != Vector2.zero) {
				characterMovement.MoveByVector (currentFrame.movementDuringFrame);
			} 

			ExecuteFrame (currentFrame);


		}

		if (currentFrame == null || !currentFrame.isLit) {
			bodyLight.enabled = false;
		} else {
			bodyLight.enabled = true;
		}

		// TODO: implement collision checking here

		previousFrame = currentFrame;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			ProcessHitboxCollision (other);
		}
	}

	void ProcessHitboxCollision (Collider hitboxObject)
	{
		HitboxController attackingHitbox = hitboxObject.GetComponent <HitboxController> ();
		if (attackingHitbox.IsLoaded ()) {
			// TODO: hand hit frame to CharacterManager, let it deal with logic re: am I hit or not, &
			// then queuing up the hitframes if necessary.

				pendingAttackHitbox = attackingHitbox;
		}
	}

	// If there is not a non-active frame between two active frames and a hit has already been dealt w/,
	// this class will not allows the hitbox to be reapplied.
	void ExecuteFrame (MoveFrame currentMoveFrame)
	{
		if (currentMoveFrame.attackData != null) {
			if (previousFrame.attackData == null) {
				AttackFrameData attackFrameData = currentMoveFrame.attackData;
				hitBox.ExecuteAttack (attackFrameData);
			}
			// TODO: add movement related stuff here
		} else if (hitBox.IsLoaded () && currentMoveFrame.attackData == null) {
			hitBox.Reset ();
		}
	}

}


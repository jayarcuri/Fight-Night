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
	// To flip orientation if necessary. Move to CharacterMovement.
	public CharacterMovement characterMovement;
	public Text healthText;
	public GameObject victoryWindow;
	string defaultHealthText;
	CharacterManager characterManager;
	InputManager inputManager;
	MoveFrame previousFrame;
	public HitboxController pendingAttackHitbox;
	public HealthBarController healthBar;
	public CharacterLightController characterLight;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;

	void Start ()
	{
		characterManager = new CharacterManager ();
		pendingAttackHitbox = null;
		inputManager = GetComponent<InputManager> ();
		characterMovement = GetComponent<CharacterMovement> ();
		characterLight = GetComponent<CharacterLightController> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		defaultHealthText = healthText.text;
		healthBar.maxHealth = characterManager.GetCurrentHealth ();
		characterLight.TurnOffLight ();

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
					healthBar.UpdateHealthBar (characterManager.GetCurrentHealth ());

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
		bool toggleLight = false;

		if (isBot) {
			directionalInput = new DirectionalInput (botDirectionalInputRaw);
			attack = botAttackInput;
		} else {
			inputManager.GetInputs (out directionalInput, out attack, out toggleLight);
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
			ExecuteFrame (currentFrame);
		}

		if (currentFrame == null || !currentFrame.isLit) {
			characterLight.TurnOffLight ();
		} else {
			characterLight.TurnOnLight ();
		}

		if (currentFrame != null && MoveType.IN_HITSTUN.Equals (currentFrame.moveType)) {
			characterLight.SetLightColorHitstun ();
		} else {
			characterLight.SetLightColorDefault ();
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
	public void ExecuteFrame (MoveFrame currentFrame)
	{
		if (currentFrame.movementDuringFrame != Vector2.zero) {
			characterMovement.MoveByVector (currentFrame.movementDuringFrame);
		} 

		if (currentFrame.attackData != null) { 
			if (previousFrame.attackData == null || !currentFrame.attackData.Equals ((previousFrame.attackData))) {
				hitBox.ExecuteAttack (currentFrame.attackData);
			}
		} else {
			if (hitBox.IsLoaded ()) {
				hitBox.Reset ();
			}
		}
	}

}


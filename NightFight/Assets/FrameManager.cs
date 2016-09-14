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
	public CharacterController opponent;
	// To flip orientation if necessary. Move to CharacterMovement.
	public CharacterMovement characterMovement;
	CharacterState characterState;
	CharacterManager characterManager;
	InputController inputController;

	void Start ()
	{
		characterState = new CharacterState (100);
		characterManager = new CharacterManager ();
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
		if (isPlayer1) {
			DirectionalInput rawDirInput;
			AttackType attack;

			inputController.GetInputs (out rawDirInput, out attack);
			// if (the character manager doesn't have a queued frame), attempt to flip the rotation.
			if (!characterManager.HasQueuedFrames ()) {
				characterMovement.FlipRotation ();
			}

			// include orientation in current frame calculations
			MoveFrame currentFrame = characterManager.GetCurrentFrame (rawDirInput, attack);
			if (currentFrame != null) {
				if (currentFrame.moveType == MoveType.STEP_BACK || currentFrame.moveType == MoveType.STEP_FORWARD) {
					characterMovement.Move (currentFrame);
				} else {
					ExecuteNextFrame (currentFrame);
				}
			}

//			if (currentFrame == null || currentFrame.moveType == MoveType.STEP_BACK || currentFrame.moveType == MoveType.STEP_FORWARD) {
//				characterMovement.FlipRotation(
//			}
			// Only rotate character if a move isn't currently being executed.

			if (attack != AttackType.None)
				bodyLight.enabled = true;
			else
				bodyLight.enabled = false;
		}
	}

	void ExecuteNextFrame (MoveFrame currentMoveFrame)
	{
		if (currentMoveFrame.moveType == MoveType.ACTIVE) {
			HitFrame attackFrame = (HitFrame)currentMoveFrame;
			Debug.Log ("Attack in progress!");
			hitBox.ExecuteAttack (attackFrame.offset, attackFrame.size, attackFrame);
		} else if (hitBox.IsLoaded () && currentMoveFrame.moveType != MoveType.ACTIVE)
			hitBox.Reset ();
	}

}


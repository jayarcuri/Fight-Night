using UnityEngine;
using System.Collections;

public enum AttackType{Light, Heavy, None}
public delegate void Move();

public class CharacterController : MonoBehaviour {
	public Move move;
	public Light bodyLight;
	CharacterMovement characterMovement;
	InputController inputController;
	HitFrame jabHitbox;
	HitFrame pokeHitbox;
	HitFrame AAHitbox;
	MoveFrame[] currentMoveSequence;
	int currentMoveIndex = 0;
	bool orientationReversed;

	void Start () {
		orientationReversed = false;
		inputController = GetComponent<InputController> ();
		characterMovement = GetComponent<CharacterMovement> ();
		bodyLight = GetComponentInChildren<Light> ();
		bodyLight.enabled = false;
		// Must change hit/block stun to factor in active frames as potential recovery.
		jabHitbox = new HitFrame (new Vector3 (0.8f, 0.2f, 0f), new Vector3 (.7f, .25f, 1f), Vector3.zero, 1f, 7, 6, true);
		pokeHitbox = new HitFrame (new Vector3 (1f, -0.4f, 0f), new Vector3 (1.2f, .2f, 1f), Vector3.zero, 2.5f, 11, 9, true);
		AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4f, 11, 7, true);
	}

	void FixedUpdate() {
		//if (currentMoveSequence == null) {
			float horizontalInput;
			float verticalInput;
			AttackType attack;
			inputController.GetInputs (out horizontalInput, out verticalInput, out attack);
		ExecuteInput (horizontalInput, verticalInput, attack);
		// } else {
			//ExecuteNextMoveFrame ();
		//}

		if (attack != AttackType.None)
			bodyLight.enabled = true;
		else
			bodyLight.enabled = false;

	}
	
	public void ExecuteInput(float horizontalInput, float verticalInput, AttackType attackType) {
		// If no action is currently being executed...
		//if (move == null) {
			// Attacks take priority over movement
		if (attackType != AttackType.None) {
				if (attackType == AttackType.Light) {
					if (verticalInput < 0) {
						// Crouch attack
					}
					if (horizontalInput > 0 && !orientationReversed) {
						// Solar plexus blow
					} else if (horizontalInput < 0 && orientationReversed) {
						// Same
					} else { // Create default jab
						currentMoveSequence = Jab ();
					}
				} else {
					// Execute heavy attack
				}
			} // If there is no attack, jumps take priority
			else {
			print ("hey");
			if (verticalInput > 0 && characterMovement.state != CharacterState.Jumping) {
				characterMovement.Jump (horizontalInput);
			}
					characterMovement.Move (horizontalInput);
				}
		//}
		// Execute the current move if it exists.
		//else
		//	move ();
	}

	void ExecuteNextMoveFrame() {
		if (currentMoveIndex < currentMoveSequence.Length) {
			MoveFrame frame = currentMoveSequence [currentMoveIndex];
			// Extend hitbox
			// if (frame.isLit && 

		} else {
			currentMoveSequence = null;
			currentMoveIndex = 0;
		}
	}

	MoveFrame[] Jab() {
		MoveFrame[] jab = {
			new MoveFrame (), 
			new MoveFrame (),
			jabHitbox,
			jabHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		};
		return jab;
	}

	MoveFrame[] Poke() {
		MoveFrame[] poke = {
			new MoveFrame (), 
			new MoveFrame (),
			new MoveFrame (),
			pokeHitbox,
			pokeHitbox,
			pokeHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		};
		return poke;
	}

	MoveFrame[] anitAir () {
		MoveFrame[] AA = {
			new MoveFrame (), 
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			AAHitbox,
			AAHitbox,
			AAHitbox,
			AAHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		};
		return AA;
	}
}

using UnityEngine;
using System.Collections;

public enum AttackType{Light, Heavy}
public delegate void Move();

public class CharacterController : MonoBehaviour {
	public Move move;
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
		// Must change hit/block stun to factor in active frames as potential recovery.
		jabHitbox = new HitFrame (new Vector3 (0.8f, 0.2f, 0f), new Vector3 (.7f, .25f, 1f), Vector3.zero, 1f, 7, 6, true);
		pokeHitbox = new HitFrame (new Vector3 (1f, -0.4f, 0f), new Vector3 (1.2f, .2f, 1f), Vector3.zero, 2.5f, 11, 9, true);
		AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4f, 11, 7, true);
	}

	void FixedUpdate() {
		if (currentMoveSequence == null) {
			float horInput;
			float vertInput;
			bool jumpButton;
			inputController.GetInputs (out horInput, out vertInput, out jumpButton);
		} else {
			
		}

	}
	
	public void ExecuteInput(int horizontalInput, int verticalInput, AttackType attackType) {
		if (move == null) {
			if (attackType != null) {
				if (attackType == AttackType.Light) {
					if (verticalInput < 0) {
						// Crouch attack
					}
					if (horizontalInput > 0 && !orientationReversed) {
						// Solar plexus blow
					} else if (horizontalInput < 0 && orientationReversed) {
						// Same
					} else { // Create default jab
						currentMoveSequence = Jab();
					}
				} else {
					// Execute heavy attack
				}
			} // If there is no button input
		else if (verticalInput > 0) {
				switch (horizontalInput) {
				case 1:
				// right jump
					break;
				case 0:
				// vert jump
					break;
				case -1:
				// left jump
					break;
				}
				// Jump
			} else if (horizontalInput != 0) {
				// move right or left
			}
		}
		move ();
	}

	void ExecuteNextMoveFrame() {
		if (currentMoveIndex < currentMoveSequence.Length) {
			
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

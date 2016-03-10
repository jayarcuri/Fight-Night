using UnityEngine;
using System.Collections;

public enum AttackType{Light, Heavy}
delegate void Move();

public class CharacterController : MonoBehaviour {
	public Move move;
	MoveFrame[] currentMoveSequence;
	int currentMoveIndex = 0;
	bool orientationReversed;

	void Start () {
		orientationReversed = false;
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
			MoveFrame.EmptyFrame (), 
			MoveFrame.EmptyFrame (),
			HitFrame (Vector3 (0.35f, 0f, 0f), Vector3 (.7f, .7f, 1f), 1f, 7f, 6f),
			HitFrame (Vector3 (0.35f, 0f, 0f), Vector3 (.7f, .7f, 1f), 1f, 7f, 6f),
			MoveFrame.EmptyFrame (),
			MoveFrame.EmptyFrame,
			MoveFrame.EmptyFrame,
			MoveFrame.EmptyFrame
		};
	}
}

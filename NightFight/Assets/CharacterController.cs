using UnityEngine;
using System.Collections;

//public delegate void Move();

public class CharacterController : MonoBehaviour {
	//public Move move;
	public HitboxController hitBox;
	public Light bodyLight;
	public bool isPlayer1;
	public bool isFacingRight { get { 
			return _isFacingRight; } 
		set { 
			Vector3 newRotation = transform.localEulerAngles; 
			newRotation.y -= 180; 
			transform.localEulerAngles = newRotation; 
			_isFacingRight = value; } 
	}
	 

	bool _isFacingRight; //{ get { return isFacingRight; } set { isFacingRight = value; Vector3 newRotation = transform.localEulerAngles; newRotation.y -= 180; transform.localEulerAngles = newRotation; } } // Orientation reversed or not
	public CharacterController opponent;
	public CharacterMovement characterMovement;
	InputController inputController;
	HitFrame jabHitbox;
	HitFrame pokeHitbox;
	HitFrame AAHitbox;

	MoveSequence currentMoveSequence;

	void Start () {
		if (isPlayer1) {
			opponent = GameObject.FindGameObjectWithTag ("Player2").GetComponent <CharacterController> ();
		} else {
			opponent = GameObject.FindGameObjectWithTag ("Player1").GetComponent <CharacterController> ();
		}

		isFacingRight = opponent.transform.localPosition.x > transform.localPosition.x;

		inputController = GetComponent<InputController> ();
		characterMovement = GetComponent<CharacterMovement> ();
		bodyLight = GetComponentInChildren<Light> ();
		hitBox = GetComponentInChildren<HitboxController> ();
		bodyLight.enabled = false;
		// Must change hit/block stun to factor in active frames as potential recovery.
		jabHitbox = new HitFrame (new Vector3 (0.8f, 0.2f, 0f), new Vector3 (.7f, .25f, 1f), Vector3.zero, 1f, 7, 6, MoveType.ACTIVE);
		pokeHitbox = new HitFrame (new Vector3 (1f, -0.4f, 0f), new Vector3 (1.2f, .2f, 1f), Vector3.zero, 2.5f, 11, 9, MoveType.ACTIVE);
		AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4f, 11, 7, MoveType.ACTIVE);
	}

	void FixedUpdate() {
		if (isPlayer1) {
			DirectionalInput rawDirInput;
			AttackType attack;

			inputController.GetInputs (out rawDirInput, out attack);
			ExecuteInput (rawDirInput, attack);
			// if there is not a current move being executed...
			if (isFacingRight && opponent.transform.localPosition.x < transform.localPosition.x)
				isFacingRight = false;
			else if (!isFacingRight && opponent.transform.localPosition.x > transform.localPosition.x)
				isFacingRight = true;
			// } else {
			//ExecuteNextMoveFrame ();
			//}

			if (attack != AttackType.None)
				bodyLight.enabled = true;
			else
				bodyLight.enabled = false;
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Hitbox" && other.gameObject != hitBox.gameObject) {
			// if not blocking or not immune to attack
			HitboxController attackingHitbox = other.GetComponent <HitboxController> ();
			if (attackingHitbox.IsLoaded ()) {
				if (characterMovement.action == CharacterAction.Standing) {
					currentMoveSequence = attackingHitbox.GetCurrentMoveHitstun ();
					attackingHitbox.Reset ();
				}
			}
		}
		}
	
	public void ExecuteInput(DirectionalInput directionalInput, AttackType attackType) {
		// If no action is currently being executed...
		if (currentMoveSequence == null) {
			// Attacks take priority over movement
			if (attackType != AttackType.None) {
				if (attackType == AttackType.Light) {
					currentMoveSequence = Jab ();
				}
			} // If there is no attack, jumps take priority
			else {
				int inputAsInt = (int)directionalInput;
				if (inputAsInt > 6 && characterMovement.action != CharacterAction.Jumping) {
					characterMovement.Jump (inputAsInt - 8);
				}
				print (inputAsInt);
				characterMovement.Move (inputAsInt % 3);
			}
		}
		// Execute the current move if it exists.
		if (currentMoveSequence != null) {
			ExecuteNextMoveFrame ();
		}
	}

	void ExecuteNextMoveFrame() {
		MoveFrame lastFrame = currentMoveSequence.getLast ();
		MoveFrame frame = currentMoveSequence.getNext ();
		if (frame.moveType == MoveType.ACTIVE)
			hitBox.ExecuteAttack ((HitFrame)frame);
		else if (lastFrame != null)
			if (lastFrame.moveType == MoveType.ACTIVE && frame.moveType != MoveType.ACTIVE)
				hitBox.Reset ();
		if (!currentMoveSequence.hasNext())
			currentMoveSequence = null;
	}

	public void CheckCollisions() {

	}

	MoveSequence Jab() {
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
		return new MoveSequence(jab);
	}

	MoveSequence Poke() {
		MoveFrame[] poke = {
			new MoveFrame (MoveType.STARTUP), 
			new MoveFrame (MoveType.STARTUP),
			new MoveFrame (MoveType.STARTUP),
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
		return new MoveSequence(poke);
	}

	MoveSequence antiAir () {
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
		return new MoveSequence(AA);
	}
}

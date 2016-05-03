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
	public CharacterController opponent; // To flip orientation if necessary. Move to CharacterMovement.
	public CharacterMovement characterMovement;
	CharacterState characterState;
	InputController inputController;

	void Start ()
	{
	characterState = new CharacterState ();
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
			//characterState.
			// Only rotate character if a move isn't currently being executed.

			// } else {
			//ExecuteNextMoveFrame ();
			//}

			if (attack != AttackType.None)
				bodyLight.enabled = true;
			else
				bodyLight.enabled = false;
		}
	}
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// Tracks all components related to a given player's avatar.
public class CharacterManager : MonoBehaviour {

	public HitboxController hitBox;
	public CharacterMovement characterMovement;
	public CharacterDataManager characterDataManager;
	public InputManager inputManager;
	MoveFrame lastExecutedFrame;
	public HitboxController pendingAttackHitbox;
	public HealthBarController healthBar;
	public CharacterLightController characterLight;

	public bool isBot;
	public int botDirectionalInputRaw;
	public AttackType botAttackInput;


	// Use this for initialization
	void Start () {
	
	}

		public void ExecuteFrame (MoveFrame currentFrame)
		{
			if (currentFrame.movementDuringFrame != Vector2.zero) {
				characterMovement.MoveByVector (currentFrame.movementDuringFrame);
			} 
			// Smelly Code Below
			if (currentFrame.attackData != null) { 
			if (lastExecutedFrame.attackData == null || !currentFrame.attackData.Equals ((lastExecutedFrame.attackData))) {
					hitBox.ExecuteAttack (currentFrame.attackData);
				}
			} else {
				if (hitBox.IsLoaded ()) {
					hitBox.Reset ();
				}
			}
		}

	public void UpdateCharacterLight() {
		bool isLit;
		if (lastExecutedFrame != null) {
			isLit = characterDataManager.isSelfIlluminated || lastExecutedFrame.isLit;
		} else {
			isLit = characterDataManager.isSelfIlluminated;
		}
		MoveType currentMoveType = lastExecutedFrame != null ? lastExecutedFrame.moveType : MoveType.NONE;

		characterLight.SetLight (isLit, currentMoveType);
	}
}

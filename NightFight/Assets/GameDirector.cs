//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//
//public class GameDirector : MonoBehaviour {
//	public HitboxController[] hitBoxes;
//	public CharacterMovement[] characterMovements;
//	public GameObject victoryWindow;
//	CharacterManager[] characterManagers;
//	InputManager[] inputManagers;
//	MoveFrame[] previousFrames;
//	public HitboxController[] pendingAttackHitboxes;
//	public HealthBarController healthBar;
//	public CharacterLightController characterLight;
//
//	public bool isBot;
//	public int botDirectionalInputRaw;
//	public AttackType botAttackInput;
//
//	// Use this for initialization
//	void Start () {
//		foreach (InputManager inputManager in inputManagers) {
//			if (inputManager == null) {
//				throw new UnityException ("We are missing a player's input source.");
//			}
//		}
//	}
//
//	// Update is called once per frame
//	void FixedUpdate ()
//	{
//		
//		//DirectionalInput[] directionalInput = new DirectionalInput[2];
//		AttackType[] attack = new AttackType[2];
//
//		for (int i = 0; i < inputManagers.Length; i++) {
//			// 1: Get input.
//			DirectionalInput directionalInput = new DirectionalInput();
//			bool toggleIllumination = false;
//			inputManagers[i].GetInputs (out directionalInput, out attack[i], out toggleIllumination[i]);
//
//			if (toggleIllumination) {
//				characterManagers [i].ToggleCharacterIllumination ();
//			}
//
//			if (!characterMovements [i].isFacingRight) {
//				directionalInput.FlipHorizontalInput ();
//			}
//			// 2: Execute input.
//			MoveFrame currentFrame = null;
//			bool isLit = false;
//			currentFrame = characterManagers[i].GetCurrentFrame (directionalInput, attack, out isLit);
//			// 3: Execute frame.
//			if (currentFrame != null) {
//				ExecuteFrame (currentFrame);
//			}
//			// TODO: 4: Check & resolve collisions (hits & bumps)
//			MoveType currentMoveType = currentFrame != null ? currentFrame.moveType : MoveType.NONE;
//			characterLight.SetLight (isLit, currentMoveType);
//
//			if (currentFrame != null && MoveType.IN_HITSTUN.Equals (currentFrame.moveType)) {
//				characterLight.SetLightColorHitstun ();
//			} else {
//				characterLight.SetLightColorDefault ();
//			}
//
//
//		}
//
//
//
//	}
//
//	// If there is not a non-active frame between two active frames and a hit has already been dealt w/,
//	// this class will not allows the hitbox to be reapplied.
//	public void ExecuteFrame (MoveFrame currentFrame)
//	{
//		if (currentFrame.movementDuringFrame != Vector2.zero) {
//			characterMovement.MoveByVector (currentFrame.movementDuringFrame);
//		} 
//		// Smelly Code Below
//		if (currentFrame.attackData != null) { 
//			if (previousFrame.attackData == null || !currentFrame.attackData.Equals ((previousFrame.attackData))) {
//				hitBox.ExecuteAttack (currentFrame.attackData);
//			}
//		} else {
//			if (hitBox.IsLoaded ()) {
//				hitBox.Reset ();
//			}
//		}
//	}
//
//}

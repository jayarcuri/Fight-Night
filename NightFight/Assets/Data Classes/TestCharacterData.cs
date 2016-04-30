using UnityEngine;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class TestCharacterData : CharacterData {

	public TestCharacterData (CharacterState cState) : base(cState) {
		characterState = cState;
	}
	
	public override MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		CharacterAction currentAction = characterState.GetCurrentAction ();
		return null;
	}
}


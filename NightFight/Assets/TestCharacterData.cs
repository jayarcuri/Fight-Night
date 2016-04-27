using UnityEngine;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class TestCharacterData : CharacterData {
	
	public override MoveSequence ReadyInput(float horizontalInput, float verticalInput, AttackType attackType) {
		CharacterAction currentAction = characterState.GetCurrentAction ();
		return null;
	}
}


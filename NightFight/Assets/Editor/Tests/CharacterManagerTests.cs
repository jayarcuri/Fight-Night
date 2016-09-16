using System;
using System.Collections;
using NUnit.Framework;

public class CharacterManagerTests {

	[Test]
	public void BackwardStepTest () {
		CharacterManager tester = new CharacterManager ();
		tester.GetCurrentFrame (DirectionalInput.Right, AttackType.None, true);
	Assert.IsNotNull (tester.currentMove);
	}

	[Test]
	public void LightAttackTest () {
		CharacterManager tester = new CharacterManager ();
		tester.GetCurrentFrame (DirectionalInput.Neutral, AttackType.Light, true);
		Assert.IsNotNull (tester.currentMove);
		Console.Write (tester.currentMove.MoveLength ());
		// This is going to change, probably not a great test...
		//Assert.IsTrue(tester.currentMove.MoveLength() == 18);
	}

}

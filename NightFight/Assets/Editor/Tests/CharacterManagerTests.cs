using System;
using System.Collections;
using NUnit.Framework;

public class CharacterManagerTests {

	[Test]
	public void BackwardStepTest () {
		CharacterManager tester = new CharacterManager ();
		tester.GetCurrentFrame (DirectionalInput.Right, AttackType.None);
	Assert.IsNotNull (tester.currentMove);
	}

	[Test]
	public void LightAttackTest () {
		CharacterManager tester = new CharacterManager ();
		tester.GetCurrentFrame (DirectionalInput.Neutral, AttackType.Light);
		Assert.IsNotNull (tester.currentMove);
	}

}

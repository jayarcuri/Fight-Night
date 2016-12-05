using UnityEngine;
using System.Collections;
using System;
using NUnit.Framework;

public class EnumerationTests
{
	[Test]
	public void AttackAndDirectionalConversionToString () {
		char lightAttackCharacter = (char)AttackType.LIGHT;
		Assert.IsTrue ('A'.Equals (lightAttackCharacter));
		string directionalCharacter = DirectionalInput.Down.numpadValue.ToString();
		Console.WriteLine (directionalCharacter);
		Assert.IsTrue ("2".Equals (directionalCharacter));
		string crouchJab = directionalCharacter + lightAttackCharacter;
		Assert.IsTrue ("2A".Equals (crouchJab));
	}
}


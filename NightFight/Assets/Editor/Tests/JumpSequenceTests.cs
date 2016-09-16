using UnityEngine;
using System;
using System.Collections;
using NUnit.Framework;

public class JumpSequenceTests {

	[Test]
	public void TestResolvesInExpectedTime() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f);
		for (int i = 0; i < 10; i++) {
			testRunner.GetNext ();
		}
		Assert.IsTrue (testRunner.HasNext());
	}

	[Test]
	public void TestVelocityChangesCorrectlyWithAddedMove() {
		Assert.IsTrue (false);
	}

	[Test]
	public void TestJumpSequenceReset() {
		Assert.IsTrue (false);
	}

}

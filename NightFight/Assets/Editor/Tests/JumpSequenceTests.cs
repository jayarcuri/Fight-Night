using UnityEngine;
using System;
using System.Collections;
using NUnit.Framework;

public class JumpSequenceTests {

	[Test]
	public void TestResolvesInExpectedTime() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f);
		TestAppropriateResolution (testRunner, 10);
	}

/*	[Test]
	public void TestVelocityChangesCorrectlyWithAddedMove() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f);
		for (int i = 0; i < 10; i++) {
			
			testRunner.GetNext ();
		}
		Assert.IsTrue (false);
	}*/

	[Test]
	public void TestJumpSequenceReset() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f);
		for (int i = 0; i < 10; i++) {
			testRunner.GetNext ();
		}
		testRunner.Reset ();
		Assert.IsTrue (testRunner.HasNext());

		TestAppropriateResolution (testRunner, 10);
	}

	void TestAppropriateResolution(JumpSequence tester, int length) {
		for (int i = 0; i < length; i++) {
			tester.GetNext ();
			//Console.WriteLine (testRunner.currentHeight);
			//Console.WriteLine (i);
		}
		Assert.IsTrue (!tester.HasNext());
	}

}

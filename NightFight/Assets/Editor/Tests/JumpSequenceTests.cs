using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class JumpSequenceTests {

	[Test]
	public void TestResolvesInExpectedTime() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f, null);
		TestAppropriateResolution (testRunner, 10);
	}

	[Test]
	public void TestJumpSequenceReset() {
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f, null);
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

	[Test]
	public void TestJumpKick() {
		// Correct length
		MoveFrame neutralFrame = new MoveFrame (MoveType.AIRBORNE);
		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (0.8f, 0.2f), 
			new Vector3 (.7f, .25f, 1f), 1, null, null, HitType.HIT);
		MoveFrame jabHitbox = new MoveFrame (Vector2.zero, MoveType.NONE, jabAttackData);
		MoveSequence flyingJumpKick = new MoveSequence(new MoveFrame[] {
			neutralFrame,
			jabHitbox,
			jabHitbox
		}, ButtonInputCommand.LIGHT);
		JumpSequence testRunner = new JumpSequence (10, 2.0f, 1.5f, null);
		for (int i = 0; i < 10; i++) {
			MoveFrame frame = testRunner.GetNext ();
			Console.WriteLine(i);
			Console.WriteLine(frame.moveType);
			if (i == 4) {
				testRunner.AddSupplementaryFrameSequence (flyingJumpKick);
			}
			if (i > 4 && i < 8) {
				Assert.IsTrue (frame.attackData != null);
			}
		}

		Assert.IsTrue(testRunner.HasNext () == false);
	}

}

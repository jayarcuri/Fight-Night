using UnityEngine;
using System;
using System.Collections;
using NUnit.Framework;

// Note to Faith: you can create print statements alongside your test results by using the method 
// 'Console.WriteLine ([whatever you want here]);'.

public class DashSequenceTests {

	[Test]
	public void TestResolvesInExpectedTime() {
		int expectedLength = 5;
		DashSequence testRunner = new DashSequence (expectedLength, 3.0f);
		int actualLength = 0;
		while (testRunner.HasNext ()) {
			actualLength++;
			testRunner.GetNext ();
		}
		Assert.IsTrue (expectedLength == actualLength);
	}

	[Test]
	public void TestTravelsExpectedDistance() {
		float expectedDistance = 2.0f;
		DashSequence testRunner = new DashSequence (10, expectedDistance);
		double actualDistance = 0;
		while (testRunner.HasNext ()) {
			actualDistance += testRunner.GetNext ().movementDuringFrame.x;
		}
		bool isWithinAllowableMargin = (Math.Abs (expectedDistance - actualDistance) < 1);
		Assert.IsTrue (isWithinAllowableMargin);
	}

	[Test]
	public void TestJumpSequenceCanReset() {
		DashSequence testRunner = new DashSequence (10, 2.0f);
		while (testRunner.HasNext ()) {
			testRunner.GetNext ();
		}
		testRunner.Reset ();
		Assert.IsTrue (testRunner.HasNext());
	}

}

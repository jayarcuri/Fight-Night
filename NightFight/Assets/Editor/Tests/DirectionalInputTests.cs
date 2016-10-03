using System;
using System.Collections;
using NUnit.Framework;

public class DirectionalInputTests {

	[Test]
	public void TestAllDeclaringConfigurations() {
		DirectionalInput one = new DirectionalInput (-1, -1);
		//Console.WriteLine (one.numpadValue);
		Assert.IsTrue (one.numpadValue == 1);

		DirectionalInput two = new DirectionalInput (0, -1);
		//Console.WriteLine (two.numpadValue);
		Assert.IsTrue (two.numpadValue == 2);

		DirectionalInput three = new DirectionalInput (1, -1);
		//Console.WriteLine (three.numpadValue);
		Assert.IsTrue (three.numpadValue == 3);

		DirectionalInput four = new DirectionalInput (-1, 0);
		//Console.WriteLine (four.numpadValue);
		Assert.IsTrue (four.numpadValue == 4);

		DirectionalInput five = new DirectionalInput (0, 0);
		//Console.WriteLine (five.numpadValue);
		Assert.IsTrue (five.numpadValue == 5);

		DirectionalInput six = new DirectionalInput (1, 0);
		//Console.WriteLine (six.numpadValue);
		Assert.IsTrue (six.numpadValue == 6);

		DirectionalInput seven = new DirectionalInput (-1, 1);
		//Console.WriteLine (seven.numpadValue);
		Assert.IsTrue (seven.numpadValue == 7);

		DirectionalInput eight = new DirectionalInput (0, 1);
		//Console.WriteLine (eight.numpadValue);
		Assert.IsTrue (eight.numpadValue == 8);

		DirectionalInput nine = new DirectionalInput (1, 1);
		//Console.WriteLine (nine.numpadValue);
		Assert.IsTrue (nine.numpadValue == 9);
	}

	[Test]
	public void TestAllDeclaringConfigurationsViaNumpadNotation() {
		DirectionalInput one = new DirectionalInput (1);
		//Console.WriteLine ("One:");
		//Console.WriteLine (one.horizontalInput);
		Assert.IsTrue (one.horizontalInput == -1);
		//Console.WriteLine (one.verticalInput);
		Assert.IsTrue (one.verticalInput == -1);

		DirectionalInput two = new DirectionalInput (2);
		//Console.WriteLine ("two:");
		//Console.WriteLine (two.horizontalInput);
		Assert.IsTrue (two.horizontalInput == 0);
		//Console.WriteLine (two.verticalInput);
		Assert.IsTrue (two.verticalInput == -1);

		DirectionalInput three = new DirectionalInput (3);
		//Console.WriteLine ("three:");
		//Console.WriteLine (three.horizontalInput);
		Assert.IsTrue (three.horizontalInput == 1);
		//Console.WriteLine (three.verticalInput);
		Assert.IsTrue (three.verticalInput == -1);

		DirectionalInput four = new DirectionalInput (4);
		//Console.WriteLine ("four:");
		//Console.WriteLine (four.horizontalInput);
		Assert.IsTrue (four.horizontalInput == -1);
		//Console.WriteLine (four.verticalInput);
		Assert.IsTrue (four.verticalInput == 0);

		DirectionalInput five = new DirectionalInput (5);
		//Console.WriteLine ("five:");
		//Console.WriteLine (five.horizontalInput);
		Assert.IsTrue (five.horizontalInput == 0);
		//Console.WriteLine (five.verticalInput);
		Assert.IsTrue (five.verticalInput == 0);

		DirectionalInput six = new DirectionalInput (6);
		//Console.WriteLine ("six:");
		//Console.WriteLine (six.horizontalInput);
		Assert.IsTrue (six.horizontalInput == 1);
		//Console.WriteLine (six.verticalInput);
		Assert.IsTrue (six.verticalInput == 0);

		DirectionalInput seven = new DirectionalInput (7);
		//Console.WriteLine ("seven:");
		//Console.WriteLine (seven.horizontalInput);
		Assert.IsTrue (seven.horizontalInput == -1);
		//Console.WriteLine (seven.verticalInput);
		Assert.IsTrue (seven.verticalInput == 1);

		DirectionalInput eight = new DirectionalInput (8);
		//Console.WriteLine ("eight:");
		//Console.WriteLine (eight.horizontalInput);
		Assert.IsTrue (eight.horizontalInput == 0);
		//Console.WriteLine (eight.verticalInput);
		Assert.IsTrue (eight.verticalInput == 1);

		DirectionalInput nine = new DirectionalInput (9);
		//Console.WriteLine ("nine:");
		//Console.WriteLine (nine.horizontalInput);
		Assert.IsTrue (nine.horizontalInput == 1);
		//Console.WriteLine (nine.verticalInput);
		Assert.IsTrue (nine.verticalInput == 1);

	}
}

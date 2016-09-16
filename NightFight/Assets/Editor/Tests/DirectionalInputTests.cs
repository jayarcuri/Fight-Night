using System;
using System.Collections;
using NUnit.Framework;

public class DirectionalInputTests {

	[Test]
	public void TestAllDeclaringConfigurations() {
		DirectionalInput one = new DirectionalInput (-1, -1);
		Console.WriteLine (one.GetNumpadNotation ());
		Assert.IsTrue (one.GetNumpadNotation () == 1);

		DirectionalInput two = new DirectionalInput (0, -1);
		Console.WriteLine (two.GetNumpadNotation ());
		Assert.IsTrue (two.GetNumpadNotation () == 2);

		DirectionalInput three = new DirectionalInput (1, -1);
		Console.WriteLine (three.GetNumpadNotation ());
		Assert.IsTrue (three.GetNumpadNotation () == 3);

		DirectionalInput four = new DirectionalInput (-1, 0);
		Console.WriteLine (four.GetNumpadNotation ());
		Assert.IsTrue (four.GetNumpadNotation () == 4);

		DirectionalInput five = new DirectionalInput (0, 0);
		Console.WriteLine (five.GetNumpadNotation ());
		Assert.IsTrue (five.GetNumpadNotation () == 5);

		DirectionalInput six = new DirectionalInput (1, 0);
		Console.WriteLine (six.GetNumpadNotation ());
		Assert.IsTrue (six.GetNumpadNotation () == 6);

		DirectionalInput seven = new DirectionalInput (-1, 1);
		Console.WriteLine (seven.GetNumpadNotation ());
		Assert.IsTrue (seven.GetNumpadNotation () == 7);

		DirectionalInput eight = new DirectionalInput (0, 1);
		Console.WriteLine (eight.GetNumpadNotation ());
		Assert.IsTrue (eight.GetNumpadNotation () == 8);

		DirectionalInput nine = new DirectionalInput (1, 1);
		Console.WriteLine (nine.GetNumpadNotation ());
		Assert.IsTrue (nine.GetNumpadNotation () == 9);
	}

	[Test]
	public void TestAllDeclaringConfigurationsViaNumpadNotation() {
		DirectionalInput one = new DirectionalInput (1);
		Console.WriteLine ("One:");
		Console.WriteLine (one.horizontalInput);
		Assert.IsTrue (one.horizontalInput == -1);
		Console.WriteLine (one.verticalInput);
		Assert.IsTrue (one.verticalInput == -1);

		DirectionalInput two = new DirectionalInput (2);
		Console.WriteLine ("two:");
		Console.WriteLine (two.horizontalInput);
		Assert.IsTrue (two.horizontalInput == 0);
		Console.WriteLine (two.verticalInput);
		Assert.IsTrue (two.verticalInput == -1);

		DirectionalInput three = new DirectionalInput (3);
		Console.WriteLine ("three:");
		Console.WriteLine (three.horizontalInput);
		Assert.IsTrue (three.horizontalInput == 1);
		Console.WriteLine (three.verticalInput);
		Assert.IsTrue (three.verticalInput == -1);

		DirectionalInput four = new DirectionalInput (4);
		Console.WriteLine ("four:");
		Console.WriteLine (four.horizontalInput);
		Assert.IsTrue (four.horizontalInput == -1);
		Console.WriteLine (four.verticalInput);
		Assert.IsTrue (four.verticalInput == 0);

		DirectionalInput five = new DirectionalInput (5);
		Console.WriteLine ("five:");
		Console.WriteLine (five.horizontalInput);
		Assert.IsTrue (five.horizontalInput == 0);
		Console.WriteLine (five.verticalInput);
		Assert.IsTrue (five.verticalInput == 0);

		DirectionalInput six = new DirectionalInput (6);
		Console.WriteLine ("six:");
		Console.WriteLine (six.horizontalInput);
		Assert.IsTrue (six.horizontalInput == 1);
		Console.WriteLine (six.verticalInput);
		Assert.IsTrue (six.verticalInput == 0);

		DirectionalInput seven = new DirectionalInput (7);
		Console.WriteLine ("seven:");
		Console.WriteLine (seven.horizontalInput);
		Assert.IsTrue (seven.horizontalInput == -1);
		Console.WriteLine (seven.verticalInput);
		Assert.IsTrue (seven.verticalInput == 1);

		DirectionalInput eight = new DirectionalInput (8);
		Console.WriteLine ("eight:");
		Console.WriteLine (eight.horizontalInput);
		Assert.IsTrue (eight.horizontalInput == 0);
		Console.WriteLine (eight.verticalInput);
		Assert.IsTrue (eight.verticalInput == 1);

		DirectionalInput nine = new DirectionalInput (9);
		Console.WriteLine ("nine:");
		Console.WriteLine (nine.horizontalInput);
		Assert.IsTrue (nine.horizontalInput == 1);
		Console.WriteLine (nine.verticalInput);
		Assert.IsTrue (nine.verticalInput == 1);

	}
}

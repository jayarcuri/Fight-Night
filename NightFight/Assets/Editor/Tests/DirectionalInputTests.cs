using System;
using System.Collections;
using NUnit.Framework;

public class DirectionalInputTests {

	[Test]
	public void TestAllConfigurations() {
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
}

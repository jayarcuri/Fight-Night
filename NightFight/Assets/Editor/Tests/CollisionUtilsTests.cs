using UnityEngine;
using System.Collections;
using System;
using NUnit.Framework;

public class CollisionUtilsTests
{

	[Test]
	public void GetPointOfImpactTest () {
		//	Different directions, collision TRUE 
		Vector2 a = new Vector2 (0, 0);
		Vector2 b = new Vector2 (1, 0);
		Vector2 V_a = new Vector2 (1, 0);
		Vector2 V_b = new Vector2 (-2, 0);

		Vector2 result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Assert.IsTrue (result.x > 0.33f && result.x < 0.34f);
		//	Different directions, collision TRUE 
		b = new Vector2 (0, 0);
		a = new Vector2 (1, 0);
		V_b = new Vector2 (1, 0);
		V_a = new Vector2 (-2, 0);

		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Assert.IsTrue (result.x > 0.33f && result.x < 0.34f);
		//	Different directions, collision FALSE
		a = new Vector2 (0, 0);
		b = new Vector2 (1, 0);
		V_a = new Vector2 (0.4f, 0);
		V_b = new Vector2 (-0.4f, 0);
		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Assert.IsTrue (result.Equals(CollisionUtils.NaV2));

		a = new Vector2 (0, 0);
		b = new Vector2 (1, 0);
		V_a = new Vector2 (0.5f, 0);
		V_b = new Vector2 (-0.5f, 0);
		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Assert.IsTrue (result.x == 0.5f);
		//	Same direction, collision TRUE
		a = new Vector2 (0, 0);
		b = new Vector2 (1, 0);
		V_a = new Vector2 (-1, 0);
		V_b = new Vector2 (-3, 0);
		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Console.WriteLine (result.x);
		Assert.IsTrue (result.x == -0.5f);

		//	Same direction, collision TRUE
		b = new Vector2 (0, 0);
		a = new Vector2 (1, 0);
		V_b = new Vector2 (-1, 0);
		V_a = new Vector2 (-3, 0);
		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Console.WriteLine (result.x);
		Assert.IsTrue (result.x == -0.5f);

		//	Same direction, collision FALSE
		a = new Vector2 (0, 0);
		b = new Vector2 (1, 0);
		V_a = new Vector2 (-1, 0);
		V_b = new Vector2 (-1, 0);
		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);

		Assert.IsTrue(CollisionUtils.NaV2.Equals(result));
	}

	[Test]
	public void GetUpdatedVelocitiesTest() {

	}


	[Test]
	public void TakeThreeTests() {

	}

}


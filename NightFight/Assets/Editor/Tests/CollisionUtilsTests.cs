using UnityEngine;
using System.Collections;
using System;
using NUnit.Framework;
using Eppy;

public class CollisionUtilsTests
{

//	[Test]
//	public void GetPointOfImpactTest () {
//		//	Different directions, collision TRUE 
//		Vector2 a = new Vector2 (0, 0);
//		Vector2 b = new Vector2 (1, 0);
//		Vector2 V_a = new Vector2 (1, 0);
//		Vector2 V_b = new Vector2 (-2, 0);
//
//		Vector2 result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Assert.IsTrue (result.x > 0.33f && result.x < 0.34f);
//		//	Different directions, collision TRUE 
//		b = new Vector2 (0, 0);
//		a = new Vector2 (1, 0);
//		V_b = new Vector2 (1, 0);
//		V_a = new Vector2 (-2, 0);
//
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Assert.IsTrue (result.x > 0.33f && result.x < 0.34f);
//		//	Different directions, collision FALSE
//		a = new Vector2 (0, 0);
//		b = new Vector2 (1, 0);
//		V_a = new Vector2 (0.4f, 0);
//		V_b = new Vector2 (-0.4f, 0);
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Assert.IsTrue (result.Equals(CollisionUtils.NaV2));
//
//		a = new Vector2 (0, 0);
//		b = new Vector2 (1, 0);
//		V_a = new Vector2 (0.5f, 0);
//		V_b = new Vector2 (-0.5f, 0);
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Assert.IsTrue (result.x == 0.5f);
//		//	Same direction, collision TRUE
//		a = new Vector2 (0, 0);
//		b = new Vector2 (1, 0);
//		V_a = new Vector2 (-1, 0);
//		V_b = new Vector2 (-3, 0);
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Console.WriteLine (result.x);
//		Assert.IsTrue (result.x == -0.5f);
//
//		//	Same direction, collision TRUE
//		b = new Vector2 (0, 0);
//		a = new Vector2 (1, 0);
//		V_b = new Vector2 (-1, 0);
//		V_a = new Vector2 (-3, 0);
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Console.WriteLine (result.x);
//		Assert.IsTrue (result.x == -0.5f);
//
//		//	Same direction, collision FALSE
//		a = new Vector2 (0, 0);
//		b = new Vector2 (1, 0);
//		V_a = new Vector2 (-1, 0);
//		V_b = new Vector2 (-1, 0);
//		result = CollisionUtils.GetPointOfImpact (a, V_a, b, V_b);
//
//		Assert.IsTrue(CollisionUtils.NaV2.Equals(result));
//	}

	[Test]
	public void GetUpdatedVelocitiesTest() {
		GameObject p1 = new GameObject ();
		GameObject p2 = new GameObject ();
//		p1.AddComponent<Transform> ();
//		p2.AddComponent<Transform> ();
		p1.transform.position = new Vector2 (0, 0);
		p1.transform.localScale = new Vector2 (0.5f, 1f);
		p2.transform.position = new Vector2 (0.75f, 0);
		p2.transform.localScale = new Vector2 (0.5f, 1f);

		//	end on same location
		Vector2 p1Velocity = new Vector2 (0.25f, 0);
		Vector2 p2Velocity = new Vector2 (-0.25f, 0);

		Tuple<Vector2, Vector2> newVelocities = CollisionUtils.GetUpdatedVelocities (p1.transform, p1Velocity, p2.transform, p2Velocity);

		Console.WriteLine ("New velocities: " + newVelocities.Item1 + " & " + newVelocities.Item2);

		//	no collision
		p1Velocity = new Vector2 (0.1f, 0);
		p2Velocity = new Vector2 (-0.1f, 0);

		newVelocities = CollisionUtils.GetUpdatedVelocities (p1.transform, p1Velocity, p2.transform, p2Velocity);

		Console.WriteLine ("New velocities: " + newVelocities.Item1 + " & " + newVelocities.Item2);

		//	Notable overlap
		p1Velocity = new Vector2 (1, 0);
		p2Velocity = new Vector2 (-2, 0);

		newVelocities = CollisionUtils.GetUpdatedVelocities (p1.transform, p1Velocity, p2.transform, p2Velocity);

		Console.WriteLine ("New velocities: " + newVelocities.Item1 + " & " + newVelocities.Item2);

		//	TODO: write reversed instances (verify method can identify the left & right characters)
	}

}


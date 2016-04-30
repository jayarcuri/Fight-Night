using UnityEngine;
using UnityEditor;
using NUnit.Framework;


public class SpecialMoveTests {
	SpecialMove specialMoveTestObj;

	public void SetUp ()
	{
		HitFrame AAHitbox = new HitFrame (new Vector3 (0.6f, 0.4f, 0f), new Vector3 (.7f, .5f, 1f), Vector3.zero, 4f, 11, 7, MoveType.ACTIVE);
		MoveSequence AA = new MoveSequence (new MoveFrame[] {
			new MoveFrame (), 
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			AAHitbox,
			AAHitbox,
			AAHitbox,
			AAHitbox,
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame (),
			new MoveFrame ()
		});
		specialMoveTestObj = new SpecialMove (new DirectionalInput[] { DirectionalInput.Down, 
			DirectionalInput.DownRight, 
			DirectionalInput.Right
		}, AA);
	}

	[Test]
	public void BufferTest () {
		SetUp ();
		// Is true when input is buffer length & correct sequence is input
		for (int i = 0; i < 5; i++) {
			specialMoveTestObj.ReadyMove (DirectionalInput.Neutral);
		}
		specialMoveTestObj.ReadyMove (DirectionalInput.Down);
		specialMoveTestObj.ReadyMove (DirectionalInput.DownRight);
	
		Assert.IsTrue (specialMoveTestObj.ReadyMove (DirectionalInput.Right));
		Assert.IsNotNull(specialMoveTestObj.GetSpecialMove (AttackType.Light));

		specialMoveTestObj.Reset ();
		// Is false when input is buffer length + 1 & correct sequence is input
		specialMoveTestObj.ReadyMove (DirectionalInput.Down);
		for (int i = 0; i < 6; i++) {
			specialMoveTestObj.ReadyMove (DirectionalInput.Neutral);
		}
		specialMoveTestObj.ReadyMove (DirectionalInput.DownRight);

		Assert.IsFalse (specialMoveTestObj.ReadyMove (DirectionalInput.Right));

		specialMoveTestObj.Reset ();
		// Is False when wrong input is given
		for (int i = 0; i < 7; i++) {
			specialMoveTestObj.ReadyMove (DirectionalInput.Neutral);
		}

		Assert.IsFalse (specialMoveTestObj.ReadyMove (DirectionalInput.Right));
	}
}
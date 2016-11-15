using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;

public class MoveSequenceTests {

	[Test]
	public void InitializeSequenceWithFrameData () {
		RecoilSequence jabHitStun = new RecoilSequence (9, .6f, MoveType.IN_HITSTUN);
		RecoilSequence jabBlockStun = new RecoilSequence (7, .45f, MoveType.BLOCKING);
		Dictionary<string, IFrameSequence> cancelsTo = new Dictionary<string, IFrameSequence> ();
		cancelsTo.Add ("HIT", null);
		AttackFrameData jabAttackData = new AttackFrameData (new Vector2 (1f, 0.2f), 
			new Vector3 (1f, .25f, 1f), 1, jabHitStun, jabBlockStun, HitType.HIT);

		MoveSequence convenienceMS = MoveSequence.GetAttackSequenceWithFrameData (3, 3, 6, jabAttackData);
		MoveFrame neutralFrame = MoveFrame.GetEmptyLitFrame ();
		MoveFrame jabHitbox =  new MoveFrame (Vector2.zero, MoveType.NONE, jabAttackData);

		MoveSequence oldMS = new MoveSequence(new MoveFrame[] {
			neutralFrame, 
			neutralFrame,
			jabHitbox,
			jabHitbox,
			jabHitbox,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame,
			neutralFrame
		});
		int counter = 0;
		while (oldMS.HasNext () && convenienceMS.HasNext ()) {
			counter++;
			MoveFrame conFrame = convenienceMS.GetNext ();
			MoveFrame oldFrame = oldMS.GetNext ();

			Assert.IsTrue(conFrame.Equals(oldFrame));
		}

		Assert.IsTrue (!convenienceMS.HasNext () && !oldMS.HasNext ());
	}

}


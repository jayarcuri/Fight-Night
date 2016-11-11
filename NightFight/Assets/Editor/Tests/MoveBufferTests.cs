using System;
using System.Collections.Generic;
using NUnit.Framework;

	public class MoveBufferTests
	{
	
	protected DirectionalInput[] QCF = new DirectionalInput[] {
		new DirectionalInput(2), new DirectionalInput(3), new DirectionalInput(6)
	};
	protected DirectionalInput[] SRK = new DirectionalInput[] {
		new DirectionalInput(6), new DirectionalInput(2), new DirectionalInput(3)
	};
	protected DirectionalInput[] ForwardDash = new DirectionalInput[] {
		new DirectionalInput(6), new DirectionalInput(6)
	};
	protected DirectionalInput[] ScissorKick = new DirectionalInput[] {
		new DirectionalInput(4), new DirectionalInput(6)
	};

	string specialMove1Code = "SP1";
	string specialMove2Code = "SP2";
	string specialMove3Code = "SP3";
	string forwardDashCode = "FrD";

	[Test]
	public void BasicBufferTest () {
		MoveBuffer qcfBuffer = new MoveBuffer (6, QCF, true, specialMove1Code);
		MoveBuffer srkBuffer = new MoveBuffer (6, SRK, true, specialMove2Code);
		MoveBuffer sKickBuffer = new MoveBuffer (4, ScissorKick, true, specialMove3Code);
		MoveBuffer dashBuffer = new MoveBuffer (4, ForwardDash, true, forwardDashCode);

		MoveBufferManager bufferManager = new MoveBufferManager ();
		bufferManager.AddMoveBuffer (qcfBuffer);
		bufferManager.AddMoveBuffer (srkBuffer);
		bufferManager.AddMoveBuffer (sKickBuffer);
		bufferManager.AddMoveBuffer (dashBuffer);

		bufferManager.GetReadiedBufferMove (new DirectionalInput (6), AttackType.None);
		bufferManager.GetReadiedBufferMove (new DirectionalInput (2), AttackType.None);
		bufferManager.GetReadiedBufferMove (new DirectionalInput (3), AttackType.None);
		List<string> readyMoveCodes = bufferManager.GetReadiedBufferMove (new DirectionalInput (6), AttackType.Heavy);

		foreach (string s in readyMoveCodes) {
			Console.WriteLine (s);
		}

		bufferManager.GetReadiedBufferMove (new DirectionalInput (2), AttackType.None);
		bufferManager.GetReadiedBufferMove (new DirectionalInput (2), AttackType.None);
		readyMoveCodes = bufferManager.GetReadiedBufferMove (new DirectionalInput (2), AttackType.Heavy);

		Console.WriteLine (readyMoveCodes.Count);
		Assert.IsTrue (readyMoveCodes.Count == 1);


		//bufferManager.ResetMoveBuffer (readyMoveCodes [0]);

		List<string> readyMoveCodesPostReset = bufferManager.GetReadiedBufferMove (new DirectionalInput (6), AttackType.Heavy);
		foreach (string s in readyMoveCodesPostReset) {
			Console.WriteLine (s);
		}

		bufferManager.ResetAll ();
	}

	}


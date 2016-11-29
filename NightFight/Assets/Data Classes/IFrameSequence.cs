using System.Collections;
using System.Collections.Generic;

public interface IFrameSequence
{
	/// <summary>
	/// Advances the move sequence by one and returns the now current MoveFrame.
	/// </summary>
	MoveFrame GetNext ();

	/// <summary>
	/// Returns a boolean signaling whether or not the sequence has completed, and therefore could not return a 
	/// valid value if GetNext () is called.
	/// </summary>
	bool HasNext ();

	/// <summary>
	/// Restores the sequence object to the state it was in when it was created. This is extremely important due 
	/// to the persistance and re-use of the same specific frame sequence throughout a given game session; if not
	/// reset, the move will essentially be usable once.
	/// </summary>
	void Reset ();

	/// <summary>
	/// Restores the sequence object to the state it was in when it was created. This is extremely important due 
	/// to the persistance and re-use of the same specific frame sequence throughout a given game session; if not
	/// reset, the move will essentially be usable once.
	/// </summary>
	MoveFrame Peek ();

	void IncrementIndex ();

	bool SequenceStartedWithButton(ButtonInput[] engagedButtons);

}


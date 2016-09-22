using System.Collections;

public interface FrameSequence
{
	MoveFrame Peek ();
	MoveFrame GetNext ();
	MoveFrame GetPrevious ();
	bool HasNext ();
	void Reset ();
	int MoveLength ();


}


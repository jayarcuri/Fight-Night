using System.Collections;

public interface FrameSequence
{
	MoveFrame Peek ();
	MoveFrame GetNext ();
	bool HasNext ();
	void Reset ();
	int MoveLength ();


}


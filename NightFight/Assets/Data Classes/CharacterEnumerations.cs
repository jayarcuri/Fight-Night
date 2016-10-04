using System;

public enum AttackType {
	Light = 'A', 
	Heavy = 'C',
	Throw = 'T',
	Block = 'X',
	None = 'Z'
}
public enum CharacterAction {
	Standing, 
	Jumping, 
	Blocking, 
	BlockStunned, 
	HitStunned
}
public enum MoveType {
	STARTUP,
	RECOVERY,
	ACTIVE,
	BLOCKING,
	AIRBORNE,
	INVLUNERABLE,
	STEP_BACK,
	STEP_FORWARD,
	IN_HITSTUN
}
public enum MovementDirection {
	None, 
	Left, 
	Right
}
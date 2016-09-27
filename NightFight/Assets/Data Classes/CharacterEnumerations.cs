using System;

public enum AttackType {
	Block,
	Light, 
	Heavy,
	Throw,
	None
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
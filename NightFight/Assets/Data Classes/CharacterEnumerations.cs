using System;

public enum AttackType {
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
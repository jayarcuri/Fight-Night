using System;


public enum DirectionalInput {
	DownLeft = 1,
	Down,
	DownRight,
	Left,
	Neutral,
	Right,
	UpLeft,
	Up,
	UpRight
}
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
	STEP_FORWARD
}
public enum MovementDirection {
	None, 
	Left, 
	Right
}
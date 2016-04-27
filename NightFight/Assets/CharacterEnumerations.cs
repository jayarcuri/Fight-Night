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
	INVLUNERABLE
}
public enum MovementDirection {
	None, 
	Left, 
	Right
}
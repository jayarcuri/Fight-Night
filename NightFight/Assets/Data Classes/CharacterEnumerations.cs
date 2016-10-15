using System;

public enum AttackType {
	Light = 'A', 
	Heavy = 'C',
	Throw = 'T',
	Block = 'X',
	None = 'Z'
}
public enum MoveType {
	NONE,
	STARTUP,
	RECOVERY,
	THROW,
	BLOCKING,
	AIRBORNE,
	AIR_INVULUNERABLE,
	INVLUNERABLE,
	STEP_BACK,
	STEP_FORWARD,
	IN_HITSTUN
}

public enum HitType {
	HIT,
	THROW,
	PROJECTILE
}

public enum MovementDirection {
	None, 
	Left, 
	Right
}
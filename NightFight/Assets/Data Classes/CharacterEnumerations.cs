﻿using System;

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
	NONE,
	STARTUP,
	RECOVERY,
	ACTIVE,
	THROW,
	BLOCKING,
	AIRBORNE,
	AIR_INVULUNERABLE,
	INVLUNERABLE,
	STEP_BACK,
	STEP_FORWARD,
	IN_HITSTUN
}

public enum ActiveMoveType {
	HIT,
	THROW,
	PROJECTILE
}

public enum MovementDirection {
	None, 
	Left, 
	Right
}
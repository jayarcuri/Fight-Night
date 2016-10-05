using UnityEngine;

public class CharacterState {
	public int health { get; private set; }
	public float charge { get; private set; }
	public bool defaultOrientation;
	public CharacterAction action = CharacterAction.Standing;
	public MovementDirection moveDirection = MovementDirection.None;

	protected HitboxController hitBox;

	public CharacterState (int maxHealth) {
		health = maxHealth;
		charge = 0f;
	}

	public virtual bool CanAct() {
		return (action != CharacterAction.BlockStunned || action != CharacterAction.HitStunned);
	}

	public virtual CharacterAction GetCurrentAction () {
		return action;
	}

	public void TakeDamage(int damage) {
		health -= damage;
		// TODO: if health <= 0, win game.
	}
}

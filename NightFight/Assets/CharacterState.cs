using UnityEngine;

public class CharacterState {
	protected int health;
	protected float charge = 0f;
	public bool defaultOrientation;
	public CharacterAction action = CharacterAction.Standing;
	public MovementDirection moveDirection = MovementDirection.None;

	protected HitboxController hitBox;

	public CharacterState (int maxHealth) {
		health = maxHealth;
	}

	public virtual bool CanAct() {
		return (action != CharacterAction.BlockStunned || action != CharacterAction.HitStunned);
	}

	public virtual CharacterAction GetCurrentAction () {
		return action;
	}

	public void TakeDamage(int damage) {
		health -= damage;
		Debug.Log("New health total is " + health);
		// TODO: if health <= 0, win game.
	}
}

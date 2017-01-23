using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterGuiController : MonoBehaviour {
	public int maxHealth;
	float defaultHealthBarLength;
	public RectTransform healthBarTransform;
	bool isPlayer1;

	// Use this for initialization
	void Start () {
		defaultHealthBarLength = healthBarTransform.rect.width;
	}

	public void UpdateHealthBar(int currentHealth) {
		float percentFull = (float)currentHealth / (float) maxHealth;
		float newWidth = Mathf.Lerp (defaultHealthBarLength, 0, percentFull);
		healthBarTransform.offsetMax = new Vector2(-newWidth, 0);

	}
}

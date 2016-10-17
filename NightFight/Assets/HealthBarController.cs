using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour {
	public int maxHealth;
	float defaultBarLength;
	public RectTransform healthBarTransform;
	bool isPlayer1;

	// Use this for initialization
	void Start () {
		defaultBarLength = healthBarTransform.rect.width;
	}

	public void UpdateHealthBar(int currentHealth) {
		float percentFull = (float)currentHealth / (float) maxHealth;
		float newWidth = Mathf.Lerp (defaultBarLength, 0, percentFull);
		healthBarTransform.offsetMax = new Vector2(-newWidth, 0);
		//healthBarTransform.sizeDelta = new Vector2 (newWidth, 0);
		// lerp

	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterGuiController : MonoBehaviour {
	public int maxHealth;
	int maxCharge;
	float defaultHealthBarLength;
	float defaultChargeBarLength;
	public RectTransform healthBarTransform;
	public RectTransform chargeBarTransform;
	bool isPlayer1;

	// Use this for initialization
	void Start () {
		defaultHealthBarLength = healthBarTransform.rect.width;
		defaultChargeBarLength = chargeBarTransform.rect.width;
		maxCharge = CharacterData.maxCharge;
	}

	public void UpdateHealthBar(int currentHealth) {
		float percentFull = (float)currentHealth / (float) maxHealth;
		float newWidth = Mathf.Lerp (defaultHealthBarLength, 0, percentFull);
		healthBarTransform.offsetMax = new Vector2(-newWidth, 0);
		//healthBarTransform.sizeDelta = new Vector2 (newWidth, 0);
		// lerp

	}

	public void UpdateChargeBar(int currentCharge) {
		float percentFull = (float)currentCharge / (float) maxCharge;
		float newWidth = Mathf.Lerp (defaultChargeBarLength, 0, percentFull);
		chargeBarTransform.offsetMax = new Vector2(-newWidth, 0);
	}
}

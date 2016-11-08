using UnityEngine;
using System.Collections;

public class CharacterLightController : MonoBehaviour {
	Color defaultLightColor;
	public Color hitstunColor;
	public Color blockstunColor;
	public Light characterLight;

	// Use this for initialization
	void Start () {
		characterLight = GetComponentInChildren<Light> ();
		defaultLightColor = Color.white;
	}

	public void SetLight(bool isLit, MoveType currentMoveType) {
		if (isLit && !characterLight.enabled) {
			characterLight.enabled = true;
		} else if (!isLit && characterLight.enabled) {
			characterLight.enabled = false;
		}

		switch (currentMoveType) {
		case MoveType.IN_HITSTUN:
			characterLight.color = hitstunColor;
			break;
		case MoveType.BLOCKING:
			characterLight.color = blockstunColor;
			break;
		default:
			if (characterLight.color != defaultLightColor) {
				characterLight.color = defaultLightColor;
			}
			break;
		}
	}

}

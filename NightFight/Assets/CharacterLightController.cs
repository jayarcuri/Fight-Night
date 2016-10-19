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
		defaultLightColor = characterLight.color;
	}

	public void SetLightColorDefault() {
		characterLight.color = defaultLightColor;
	}

	public void SetLightColorHitstun() {
		characterLight.color = hitstunColor;
	}

	public void SetLightColorBlockstun() {
		characterLight.color = blockstunColor;
	}

	public void TurnOnLight () {
		characterLight.enabled = true;
	}

	public void TurnOffLight () {
		characterLight.enabled = false;
	}

}

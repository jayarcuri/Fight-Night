using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameMenuController : MonoBehaviour {

	public void ResetLevel () {
		Debug.Log ("Reset called.");
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		Time.timeScale = 1;
	}
}

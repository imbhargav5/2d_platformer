using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

	public string levelSelect;
	public string mainMenu;
	public GameObject thePauseScreen;
	private LevelManager theLevelManager;

	private PlayerController thePlayer;

	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager> ();
		thePlayer = FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pause")){
			if (Time.timeScale == 0f) {
				ResumeGame ();
			} else {
				PauseGame ();
			}
		}
	}

	public void PauseGame(){
		if (thePlayer.canMove) {
			Time.timeScale = 0;
			thePlayer.canMove = false;
			theLevelManager.levelMusic.Pause ();
			thePauseScreen.SetActive (true);
		}
	}

	public void ResumeGame(){
		Time.timeScale = 1f;
		thePlayer.canMove = true;
		theLevelManager.levelMusic.Play ();
		thePauseScreen.SetActive (false);
	}

	public void LevelSelect(){
		Time.timeScale = 1f;
		PlayerPrefs.SetInt ("CoinCount", theLevelManager.currentLives);
		PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);
		SceneManager.LoadScene (levelSelect);
	}

	public void QuitToMainMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene (mainMenu);
	}
}

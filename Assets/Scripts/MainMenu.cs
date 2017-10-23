using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public string firstLevel;
	public string LevelSelect;
	public string[] levelNames;

	public int startingLives;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NewGame(){
		for(int i = 1; i< levelNames.Length; i++){
			PlayerPrefs.SetInt(levelNames[i], 0);
		}
		PlayerPrefs.SetInt("PlayerLives", startingLives);
		PlayerPrefs.SetInt("CoinCount", 0);
		SceneManager.LoadScene (firstLevel);
	}

	public void ContinueGame(){
		SceneManager.LoadScene (LevelSelect);
	}

	public void QuitGame(){
		Application.Quit ();
	}
}

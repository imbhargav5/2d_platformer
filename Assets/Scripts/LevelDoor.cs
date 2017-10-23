using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour {

	public string levelToLoad;
	public bool unlocked;

	public Sprite doorBottomOpen;
	public Sprite doorTopOpen;
	public Sprite doorBottomClosed;
	public Sprite doorTopClosed;

	public SpriteRenderer doorTop;
	public SpriteRenderer doorBottom;

	private PlayerController thePlayer;

	// Use this for initialization
	void Start () {
		thePlayer = FindObjectOfType<PlayerController> ();
		PlayerPrefs.SetInt ("Level1", 1);

		if (PlayerPrefs.GetInt (levelToLoad) == 1) {
			unlocked = true;
		} else {
			unlocked = false;
		}

		if (unlocked) {
			doorTop.sprite = doorTopOpen;
			doorBottom.sprite = doorBottomOpen;
		} else {
			doorTop.sprite = doorTopClosed;
			doorBottom.sprite = doorBottomClosed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			if (unlocked) {
				if (thePlayer.uiJump || Input.GetButtonDown ("Jump") ) {
					
					SceneManager.LoadScene (levelToLoad);
				}

			}
		}
	}
}

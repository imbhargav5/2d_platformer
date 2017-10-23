using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

	public string levelToLoad;
	public string levelToUnlock;

	public float waitToMove;
	public float waitToLoad;

	private bool movePlayer;

	private PlayerController thePlayer;
	private CameraController theCamera;
	private LevelManager theLevelManager;

	public Sprite flagOpen;

	private SpriteRenderer theSpriteRenderer;


	// Use this for initialization
	void Start () {
		thePlayer = FindObjectOfType<PlayerController> ();		
		theCamera = FindObjectOfType<CameraController> ();
		theLevelManager = FindObjectOfType<LevelManager> ();
		theSpriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (movePlayer) {
			thePlayer.myRigidbody.velocity = new Vector3(thePlayer.moveSpeed, thePlayer.myRigidbody.velocity.y, 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			theSpriteRenderer.sprite = flagOpen;
			StartCoroutine ("LevelEndCo");
		}
	}


	public IEnumerator LevelEndCo(){
		thePlayer.canMove = false;
		theCamera.followTarget = false;
		theLevelManager.invincible = true;
		thePlayer.myRigidbody.velocity = Vector3.zero;
		theLevelManager.levelMusic.Stop ();
		theLevelManager.gameOverMusic.Play ();

		PlayerPrefs.SetInt ("CoinCount", theLevelManager.coinCount);
		PlayerPrefs.SetInt ("PlayerLives", theLevelManager.currentLives);
		PlayerPrefs.SetInt (levelToUnlock, 1);

		yield return new WaitForSeconds (waitToMove);
		movePlayer = true;

		yield return new WaitForSeconds (waitToLoad);
		movePlayer = false;
		thePlayer.canMove = true;
		theCamera.followTarget = true;
		theLevelManager.invincible = false;

		SceneManager.LoadScene (levelToLoad);

	}

}

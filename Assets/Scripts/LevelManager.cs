using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float waitToRespawn;
	public int healthCount;
	public int maxHealth;
	public PlayerController thePlayer;
	public GameObject deathSplosion;

	public int coinCount;
	public int bonusLifeThreshold;
	private int coinBonusLifeCount;

	public Text coinText;

	public Image heart1;
	public Image heart2;
	public Image heart3;

	public Sprite heartFull;
	public Sprite heartHalf;
	public Sprite heartEmpty;

	public int startingLives;
	public int currentLives;

	public Text livesText;

	private bool isRespawning;

	public ResetOnRespawn[] objectsToReset;
	public bool invincible;

	public GameObject gameOverScreen;

	public AudioSource coinsSound;

	public AudioSource levelMusic;
	public AudioSource gameOverMusic;
	public bool respawnCoActive;

	// Use this for initialization
	void Start () {
		levelMusic.Play ();
		thePlayer = FindObjectOfType<PlayerController> ();
		currentLives = startingLives;

		if (PlayerPrefs.HasKey ("CoinCount")) {
			coinCount = PlayerPrefs.GetInt ("CoinCount");
		}

		if (PlayerPrefs.HasKey ("PlayerLives")) {
			currentLives = PlayerPrefs.GetInt ("PlayerLives");
		}

		coinBonusLifeCount = coinCount;
		coinText.text = "Coins: " + coinCount;
		livesText.text = "Lives x " + currentLives;
		healthCount = maxHealth;
		UpdateHeartMeter ();
		objectsToReset = FindObjectsOfType<ResetOnRespawn> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (healthCount <= 0 && !isRespawning) {
			Respawn ();
		}
		if (coinBonusLifeCount >= bonusLifeThreshold) {
			int livesToAdd = Mathf.CeilToInt (coinBonusLifeCount / bonusLifeThreshold);
			coinBonusLifeCount = coinBonusLifeCount % bonusLifeThreshold;
			AddLives (livesToAdd);
		}
	}

	public void Respawn(){
		if (isRespawning == true) {
			return;
		}
		Debug.Log ("Respawing");
		currentLives -= 1;
		livesText.text = "Lives x " + currentLives;
		if (currentLives > 0) {
			isRespawning = true;
			StartCoroutine ("RespawnCo");
		} else {
			thePlayer.gameObject.SetActive (false);
			gameOverScreen.SetActive (true);
			gameOverMusic.Play ();
			levelMusic.volume = levelMusic.volume / 4f;
		}
	}

	public IEnumerator RespawnCo(){
		respawnCoActive = true;
		thePlayer.gameObject.SetActive (false);
		Instantiate (deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);
		yield return new WaitForSeconds (waitToRespawn);
		respawnCoActive = false;
		healthCount = maxHealth;
		isRespawning = false;
		thePlayer.transform.position = thePlayer.respawnPosition;
		thePlayer.gameObject.SetActive (true);
		UpdateHeartMeter ();

		coinCount = 0;
		coinBonusLifeCount = 0;
		coinText.text = "Coins: 0";

		for (int i = 0; i < objectsToReset.Length; i++) {
			ResetOnRespawn currentObject = objectsToReset [i];
			currentObject.ResetObject ();
		}
	}

	public void AddCoins(int coinsToAdd){
		coinCount += coinsToAdd;
		coinBonusLifeCount += coinsToAdd;
		coinText.text = "Coins: " + coinCount;
		coinsSound.Play ();
	}

	public void HurtPlayer(int damageToTake){
		if (!invincible) {
			this.healthCount -= damageToTake;
			UpdateHeartMeter ();
			thePlayer.Knockback ();
		}
	}

	public void AddHealth(int healthToGive){
		healthCount = Mathf.Min (maxHealth, healthCount + healthCount);
		UpdateHeartMeter ();
		coinsSound.Play ();
	}

	public void UpdateHeartMeter(){
		UpdateHeart (Mathf.Max(healthCount -4 , 0), heart3 );
		UpdateHeart (Mathf.Max(healthCount - 2 , 0), heart2 );
		UpdateHeart (Mathf.Max(healthCount , 0), heart1 );
	}

	public void UpdateHeart(int health, Image heart){
		switch (health) {
		case 1:
			heart.sprite = heartHalf;
			break;
		case 0:
			heart.sprite = heartEmpty;
			break;
		default:
			heart.sprite = heartFull;
			break;
		}
	}

	public void AddLives(int livesToAdd){
		currentLives += livesToAdd;
		livesText.text = "Lives x " + currentLives;
		coinsSound.Play ();
	}

}

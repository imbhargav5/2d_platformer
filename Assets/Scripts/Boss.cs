using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public bool bossActive;
	public float timeBetweenDrops;
	private float currentTimeBetweenDrops;

	private float dropCount;

	public float waitForPlatforms;
	private float platformCount;


	public Transform leftPoint;
	public Transform rightPoint;
	public Transform dropSawPoint;

	public GameObject dropSaw;
	public GameObject theBoss;

	public GameObject rightPlatforms;
	public GameObject leftPlatforms;

	public bool takeDamage;

	public bool bossRight;


	public int startingHealth;
	private int currentHealth;

		
	public GameObject levelExit;
	private CameraController theCamera;
	private LevelManager theLevelManager;
	public bool waitingForRespawn;

	// Use this for initialization
	void Start () {
		currentTimeBetweenDrops = timeBetweenDrops;
		dropCount = currentTimeBetweenDrops;
		platformCount = waitForPlatforms;
		theBoss.transform.position = rightPoint.position;
		currentHealth = startingHealth;
		theCamera = FindObjectOfType<CameraController> ();
		theLevelManager = FindObjectOfType<LevelManager> ();
		bossRight = true;
	}
	
	// Update is called once per frame
	void Update () {


		if (theLevelManager.respawnCoActive) {
			bossActive = false;
			waitingForRespawn = true;
		}

		if (waitingForRespawn && !theLevelManager.respawnCoActive) {
			waitingForRespawn = false;
			theBoss.SetActive (false);
			rightPlatforms.SetActive (false);
			leftPlatforms.SetActive (false);

			currentTimeBetweenDrops = timeBetweenDrops;
			dropCount = currentTimeBetweenDrops;
			platformCount = waitForPlatforms;
			theBoss.transform.position = rightPoint.position;
			currentHealth = startingHealth;
			bossRight = true;
			theCamera.followTarget = true;
		}

		if (bossActive) {
			theBoss.SetActive (true);
			theCamera.followTarget = false;
			theCamera.transform.position = Vector3.Lerp (theCamera.transform.position, new Vector3 (transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z), theCamera.smoothing * Time.deltaTime);
			if (dropCount > 0) {
				dropCount -= Time.deltaTime;
			} else {
				dropSawPoint.position = new Vector3 (Random.Range(leftPoint.position.x, rightPoint.position.x), dropSawPoint.position.y, dropSawPoint.position.z);
				Instantiate (dropSaw, dropSawPoint.position, dropSawPoint.rotation);
				dropCount = currentTimeBetweenDrops;
			}
			if (bossRight) {
				if (platformCount > 0) {
					platformCount -= Time.deltaTime;
				} else {
					platformCount = waitForPlatforms;
					rightPlatforms.SetActive (true);
					leftPlatforms.SetActive (false);
				}
			} else {
				if (platformCount > 0) {
					platformCount -= Time.deltaTime;
				} else {
					platformCount = waitForPlatforms;
					leftPlatforms.SetActive (true);
					rightPlatforms.SetActive (false);
				}
			}

			if (takeDamage) {
				currentHealth -= 1;

				if (currentHealth <= 0) {
					theCamera.followTarget = true;
					levelExit.SetActive (true);
					gameObject.SetActive (false);
				}

				if (bossRight) {
					theBoss.transform.position = leftPoint.position;
				} else {
					theBoss.transform.position = rightPoint.position;
				}

				bossRight = !bossRight;
				rightPlatforms.SetActive (false);
				leftPlatforms.SetActive (false);
				platformCount = waitForPlatforms;

				currentTimeBetweenDrops = currentTimeBetweenDrops / 2f;

				takeDamage = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			bossActive = true;
		}
	}
}

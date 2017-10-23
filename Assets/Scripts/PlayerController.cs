using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	private float activeMoveSpeed;
	public Rigidbody2D myRigidbody;

	public float jumpSpeed;


	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;

	public bool isGrounded;
	private Animator myAnim;

	public Vector3 respawnPosition;

	public LevelManager theLevelManager;

	public GameObject stompBox;

	public float knockbackForce;
	public float knockbackLength;
	private float knockbackCounter;

	public float invincibilityLength;
	private float invincibilityCounter;

	public AudioSource jumpSound;
	public AudioSource hurtSound;

	private bool onPlatform;
	public float onPlatformSpeedModifier;

	public bool canMove;

	public Button leftButton;

	public bool uiMoveLeft;
	public bool uiMoveRight;
	public bool uiJump;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		respawnPosition = transform.position;
		theLevelManager = FindObjectOfType<LevelManager> ();
		activeMoveSpeed = moveSpeed;
		canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);

		if (knockbackCounter <= 0 && canMove) {

			if (onPlatform) {
				activeMoveSpeed = moveSpeed * onPlatformSpeedModifier;
			} else {
				activeMoveSpeed = moveSpeed;
			}

			if (Input.GetAxisRaw ("Horizontal") > 0f || uiMoveRight) {
				myRigidbody.velocity = new Vector3 (activeMoveSpeed, myRigidbody.velocity.y, 0f);
				this.transform.localScale = new Vector3 (1f, 1f, 1f);
			} else if (Input.GetAxisRaw ("Horizontal") < 0f || uiMoveLeft) {
				myRigidbody.velocity = new Vector3 (-activeMoveSpeed, myRigidbody.velocity.y, 0f);
				this.transform.localScale = new Vector3(-1f, 1f, 1f);
			} else {
				myRigidbody.velocity = new Vector3 (0, myRigidbody.velocity.y, 0f);
			}

			if(isGrounded){
				if(Input.GetButtonDown("Jump") || uiJump){
					Jump ();
				}
			}
		}

		if (knockbackCounter > 0) {
			knockbackCounter -= Time.deltaTime;
			myRigidbody.velocity = new Vector3 ( transform.localScale.x > 0 ? -knockbackForce : knockbackForce , knockbackForce, 0f);
		}

		if (invincibilityCounter <= 0) {
			if(theLevelManager.invincible){
				theLevelManager.invincible = false;
			}
		} else {
			invincibilityCounter -= Time.deltaTime;
		}

		myAnim.SetFloat ("Speed", Mathf.Abs(myRigidbody.velocity.x));
		myAnim.SetBool ("Grounded", isGrounded);

		if (myRigidbody.velocity.y < 0f) {
			stompBox.SetActive (true);
		} else {
			stompBox.SetActive (false);
		}
	}

	public void Jump(){
		if (isGrounded) {
			myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, jumpSpeed, 0f);
			jumpSound.Play ();
		}
	}

	public void Knockback(){
		hurtSound.Play ();
		theLevelManager.invincible = true;
		knockbackCounter = knockbackLength;
		invincibilityCounter = invincibilityLength;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "KillPlane") {
			//gameObject.SetActive (false);
			//transform.position = respawnPosition;
			theLevelManager.Respawn();
		}

		if (other.tag == "Checkpoint") {
			respawnPosition = other.transform.position;
		}

	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform") {
			transform.parent = other.transform;
			onPlatform = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "MovingPlatform") {
			transform.parent = null;
			onPlatform = false;
		}
	}
}

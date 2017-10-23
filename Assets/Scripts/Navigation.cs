using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

	private PlayerController thePlayer;
	private bool jumpPressed;
	// Use this for initialization
	void Start () {
		thePlayer = FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LeftKeyDown(){
		thePlayer.uiMoveLeft = true;
	}

	public void LeftKeyUp(){
		thePlayer.uiMoveLeft = false;
	}

	public void RightKeyDown(){
		thePlayer.uiMoveRight = true;
	}

	public void RightKeyUp(){
		thePlayer.uiMoveRight = false;
	}

	public void JumpKeyDown(){
		thePlayer.uiJump = true;
	}

	public void JumpKeyUp(){
		thePlayer.uiJump = false;
	}
}

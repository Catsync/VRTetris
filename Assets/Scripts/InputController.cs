using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public bool isActive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			KeyboardInput ();
		}
	}


	void KeyboardInput()
	{
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.UpArrow)) {
			Controllers.Game.currentShape.RotateShapeClockwise ();
		} else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.DownArrow)) {
			Controllers.Game.currentShape.RotateShapeCounterClockwise ();
		} else if (Input.GetKeyDown (KeyCode.Q) || Input.GetKeyDown (KeyCode.LeftArrow)) {
			Controllers.Game.currentShape.MoveHorizontal (Vector2.left);
		} else if (Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.RightArrow)) {
			Controllers.Game.currentShape.MoveHorizontal (Vector2.right);
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			Controllers.Game.currentShape.DropShape ();
		}
	}

}

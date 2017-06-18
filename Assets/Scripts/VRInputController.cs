using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputController : MonoBehaviour {

	private SteamVR_TrackedController _controller;

	public float HorizontalThreshold = 5.0f;
	public float VerticalThreshold = 20.0f;
	public float RotateThreshold = 30.0f;


	private bool triggerClicked = false;
	private Transform target = null;
	private Vector3 lastPos;
	private Vector3 lastEulerAngles;

	private void OnEnable()
	{
		_controller = GetComponent<SteamVR_TrackedController>();
		_controller.TriggerClicked += HandleTriggerClicked;
		_controller.TriggerUnclicked += HandleTriggerUnclicked;
	}

	private void OnDisable()
	{
		_controller.TriggerClicked -= HandleTriggerClicked;
		_controller.TriggerUnclicked -= HandleTriggerUnclicked;
	}


	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
		triggerClicked = true;
		target = Controllers.Game.currentShape.transform;
		lastEulerAngles = transform.eulerAngles;
		lastPos = transform.position;
	}

	private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		triggerClicked = false;
		target = null;
	}

	void Update()
	{
		if (triggerClicked && target == Controllers.Game.currentShape.transform) {
			//Vector3 posDelta = transform.position - lastPos;
			Vector3 angleDelta = (transform.eulerAngles - lastEulerAngles);
			Vector3 updateDelta = new Vector3 (0, 0, 0);

			// Horizontal Movement
			float yDelta = (angleDelta.y >= 0) ? angleDelta.y : 360 + angleDelta.y;

			if(yDelta > HorizontalThreshold && yDelta < 90) {
				// Move right
				Controllers.Game.currentShape.MoveHorizontal (Vector2.right);
				updateDelta.y = angleDelta.y;
			} else if(yDelta < 360 - HorizontalThreshold && yDelta > 90) {
				// Move left
				Controllers.Game.currentShape.MoveHorizontal (Vector2.left);
				updateDelta.y = angleDelta.y;
			}

			// Rotational Movement
			float zDelta = (angleDelta.z >= 0) ? angleDelta.z : 360 + angleDelta.z;

			if (zDelta > RotateThreshold && zDelta < 90) {
				// Rotate cw
				Controllers.Game.currentShape.RotateShapeClockwise ();
				updateDelta.z = angleDelta.z;
			} else if (zDelta < 360 - RotateThreshold && zDelta > 90) {
				Controllers.Game.currentShape.RotateShapeCounterClockwise ();
				updateDelta.z = angleDelta.z;
			}


			// Drop
			float xDelta = (angleDelta.x >= 0) ? angleDelta.x : 360 + angleDelta.x;

			if (xDelta > VerticalThreshold && xDelta < 90) {
				// Drop
				Controllers.Game.currentShape.DropShape ();
				updateDelta.x = angleDelta.x;
			}

			lastEulerAngles += updateDelta;
			Debug.Log ("Angle Delta: " + angleDelta);



		}
	}
}

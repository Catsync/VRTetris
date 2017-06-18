using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputTest : MonoBehaviour {

	private SteamVR_TrackedController _controller;

	private bool triggerClicked = false;
	private Transform target = null;
	private float targetDistance;
	private Vector3 lastPos;
	private Vector3 lastEulerAngles;

	private void OnEnable()
	{
		_controller = GetComponent<SteamVR_TrackedController>();
		_controller.TriggerClicked += HandleTriggerClicked;
		_controller.TriggerUnclicked += HandleTriggerUnclicked;
//		_controller.PadClicked += HandlePadClicked;
	}

	private void OnDisable()
	{
		_controller.TriggerClicked -= HandleTriggerClicked;
		_controller.TriggerUnclicked -= HandleTriggerUnclicked;
//		_controller.PadClicked -= HandlePadClicked;
	}


	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
		Debug.Log ("Trigger clicked");
		triggerClicked = true;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			//Debug.Log ("Raycast hit " + hit.transform.name);
			target = hit.transform;
			targetDistance = Vector3.Distance (transform.position, target.transform.position);
			//lastPos = transform.position;
			lastEulerAngles = transform.eulerAngles;
		}
	}

	private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		Debug.Log ("Trigger released");
		triggerClicked = false;
		target = null;
	}

	void Update()
	{
		if (triggerClicked && target != null) {
			// Move target transform along with controller transform delta
			//Vector3 delta = transform.position - lastPos;
			//target.position += delta;

			// Target moves to point in front of controller
			Vector3 pointAtPos = transform.position + (transform.forward * targetDistance);
			target.position = pointAtPos;

			// Target mimics controller rotation
			Vector3 angleDelta = (transform.eulerAngles - lastEulerAngles);
//			angleDelta = new Vector3 (angleDelta.x % 180, angleDelta.y % 180, angleDelta.z % 180);
//			target.eulerAngles += angleDelta;
			target.Rotate (angleDelta, Space.World);
			lastEulerAngles = transform.eulerAngles;

			//lastPos = transform.position;
		}
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    public float sensitivity = 10f;
	public float yRotationLimit = 88f;

    private GameObject dummy;

	private bool isGrabbing = false;

	private Vector2 rotation = Vector2.zero;
	const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
	const string yAxis = "Mouse Y";

	private void Awake() {
		//TODO: implement check to see if there's already a real player

		dummy = transform.Find("Dummy").gameObject;
		dummy.SetActive(true);
	}

    void Start() {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
    }

	void Update() {
		rotate();

		if(Input.GetKeyDown(KeyCode.Mouse0)) {
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit, 5f)) {
				var grabbable = hit.collider.GetComponent<IGrabbable>();

				if(grabbable != null) {
					grabbable.OnGrab(gameObject);
					isGrabbing = true;
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.Mouse0) && isGrabbing) {
			isGrabbing = false;
			var grabbable = GetComponentInChildren<IGrabbable>();
			grabbable.OnRelease(gameObject);
		}

		if(Input.GetKeyDown(KeyCode.Mouse1)) {
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit, 5f)) {
				var clickable = hit.collider.GetComponent<IClickable>();

				if(clickable != null) {
					clickable.OnClick();
				}
			}
		}
	}

	void rotate() {
		rotation.x += Input.GetAxis(xAxis) * sensitivity;
		rotation.y += Input.GetAxis(yAxis) * sensitivity;
		rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
		var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

		transform.localRotation = xQuat * yQuat;
	}
}

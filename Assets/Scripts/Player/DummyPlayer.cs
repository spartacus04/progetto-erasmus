using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject VR;
	public float sensitivity = 10f;
	public float yRotationLimit = 88f;

	private GameObject dummy;

	private bool isGrabbing = false;
	private GameObject grabbedObject = null;
	private float grabbedDistance = 0f;

	private Vector2 rotation = Vector2.zero;
	const string xAxis = "Mouse X";
	const string yAxis = "Mouse Y";
	private void Awake()
	{
		//TODO: implement check to see if there's already a real player
		if (UnityEngine.XR.XRSettings.enabled)
		{
			gameObject.SetActive(false);
		}
		else
		{
			VR.SetActive(false);
			
		}


		dummy = transform.Find("Dummy").gameObject;
		dummy.SetActive(true);
	}

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		rotate();
		grab();
		click();
	}

	void rotate()
	{
		rotation.x += Input.GetAxis(xAxis) * sensitivity;
		rotation.y += Input.GetAxis(yAxis) * sensitivity;
		rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
		var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

		transform.localRotation = xQuat * yQuat;
	}

	void grab()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
			{
				var grabbable = hit.collider.GetComponent<IGrabbable>();

				if (grabbable != null)
				{
					grabbedObject = hit.collider.gameObject;
					grabbable.OnGrab();
					isGrabbing = true;
					grabbedObject.transform.parent = transform;
				}
			}
		}

		if (Input.GetKeyUp(KeyCode.Mouse0) && isGrabbing)
		{
			grabbedObject.GetComponent<IGrabbable>().OnRelease();
			grabbedObject.transform.parent = null;
			grabbedObject = null;
			isGrabbing = false;
		}

		if (isGrabbing && Input.mouseScrollDelta.y != 0)
		{
			grabbedDistance = Vector3.Distance(transform.position, grabbedObject.transform.position) + 0.1f * Input.mouseScrollDelta.y;

			if (grabbedDistance < 0.5f) grabbedDistance = 0.5f;
			if (grabbedDistance > 1.5f) grabbedDistance = 1.5f;

			grabbedObject.transform.localPosition = new Vector3(
				grabbedObject.transform.localPosition.x,
				grabbedObject.transform.localPosition.y,
				grabbedDistance
			);
		}
	}

	void click()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
			{
				var clickable = hit.collider.GetComponent<IClickable>();

				if (clickable != null)
				{
					clickable.OnClick();
				}
			}
		}
	}
}

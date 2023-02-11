using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableGeneric : MonoBehaviour, IGrabbable
{
	private Rigidbody rb;
	private bool isGrabbed = false;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		if(!isGrabbed) return;
		
		transform.rotation = Quaternion.identity;
	}

	public void OnGrab(GameObject parent)
	{
		transform.parent = parent.transform;
		Destroy(rb);

		isGrabbed = true;
	}

	public void OnRelease(GameObject parent)
	{
		transform.parent = null;
		rb = gameObject.AddComponent<Rigidbody>();

		rb.isKinematic = false;
		transform.rotation = Quaternion.identity;

		isGrabbed = false;
	}
}

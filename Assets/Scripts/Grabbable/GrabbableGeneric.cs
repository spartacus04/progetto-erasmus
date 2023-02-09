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
		
		rb.velocity = Vector3.zero;
	}

	public void OnGrab(GameObject parent)
	{
		transform.parent = parent.transform;
		rb.useGravity = false;
	}

	public void OnRelease(GameObject parent)
	{
		transform.parent = null;
		rb.useGravity = true;
	}
}

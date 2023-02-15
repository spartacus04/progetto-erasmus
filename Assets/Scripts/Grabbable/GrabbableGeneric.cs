using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableGeneric : MonoBehaviour, IGrabbable
{
	public Material hologramMaterial;

	private Rigidbody rb;
	private bool isGrabbed = false;

	private Vector3 lastPosition;
	private Vector3 lastHologramPos;
	private GameObject hologram;

	private bool isSnapped = false;

	void Start() {
		rb = GetComponent<Rigidbody>();
		hologram = Instantiate(gameObject);

		Destroy(hologram.GetComponent<GrabbableGeneric>());
		Destroy(hologram.GetComponent<Rigidbody>());
		Destroy(hologram.GetComponent<Collider>());

		hologram.GetComponent<Renderer>().material = hologramMaterial;
		hologram.SetActive(false);
	}

	void Update() {
		if(!isGrabbed) return;
		
		transform.rotation = Quaternion.identity;

		if(transform.position != lastPosition) {
			lastPosition = transform.position;

			var point = TiledGrid.getNearestPoint(transform.position);

			if(point != lastHologramPos && Vector3.Distance(point, transform.position) <= 0.5f) {
				lastHologramPos = point;
				hologram.transform.position = point;
				hologram.SetActive(true);
			}
			else if(Vector3.Distance(point, transform.position) > 0.5f) {
				hologram.SetActive(false);
			}
		}
	}

	public void OnGrab(GameObject parent)
	{
		transform.parent = parent.transform;
		Destroy(rb);

		isGrabbed = true;
		isSnapped = false;
	}

	public void OnRelease(GameObject parent)
	{
		transform.parent = null;
		rb = gameObject.AddComponent<Rigidbody>();

		rb.isKinematic = false;
		transform.rotation = Quaternion.identity;

		isGrabbed = false;

		if(hologram.activeSelf) {
			transform.position = hologram.transform.position;
			isSnapped = true;
			rb.isKinematic = true;
		}

		hologram.SetActive(false);
	}
}

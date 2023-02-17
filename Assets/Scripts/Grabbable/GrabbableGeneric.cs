using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrabbableGeneric : MonoBehaviour, IGrabbable
{
	public Material hologramMaterial;

	protected Rigidbody rb;
	protected bool isGrabbed = false;

	protected Vector3 lastPosition;
	protected Vector3 lastHologramPos;
	protected GameObject hologram;

	protected bool isSnapped = false;

	public virtual void Start() {
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

			if(TiledGrid.machines.ContainsKey(hologram.transform.position)) {
				hologram.SetActive(false);
			}
		}
	}

	public virtual void OnGrab(GameObject parent)
	{
		TiledGrid.machines.Remove(transform.position);
		transform.parent = parent.transform;
		Destroy(rb);

		isGrabbed = true;
		isSnapped = false;
	}

	public virtual void OnRelease(GameObject parent)
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
			TiledGrid.machines.Add(transform.position, GetComponent<Machine>());
		}

		hologram.SetActive(false);
	}
}

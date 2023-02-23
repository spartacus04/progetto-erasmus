using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrabbableGeneric : MonoBehaviour, IGrabbable
{
	public Material hologramMaterial;

	protected Rigidbody rb;
	protected Machine machine = null;

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

		if(hologram.TryGetComponent(out MeshRenderer meshRenderer)) {
			meshRenderer.material = hologramMaterial;
		} else {
			foreach(var child in hologram.GetComponentsInChildren<MeshRenderer>())
				child.material = hologramMaterial;
		}

		if(!hologram.TryGetComponent(out machine)) {
			machine = null;
		}

		Debug.Log("Machine: " + machine);
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

			if(TiledGrid.gridPoints.ContainsKey(point)) {
				var gridCoord = TiledGrid.gridPoints[point];

				if(TiledGrid.machines.ContainsKey(gridCoord)) {
					hologram.SetActive(false);
				}
			}
		}
	}

	public virtual void OnGrab(GameObject parent)
	{
		if(TiledGrid.gridPoints.ContainsKey(transform.position) && TiledGrid.machines.ContainsKey(TiledGrid.gridPoints[transform.position]))
			TiledGrid.machines.Remove(TiledGrid.gridPoints[transform.position]);

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

			if(machine != null)
				if(!TiledGrid.machines.ContainsKey(TiledGrid.gridPoints[transform.position]))
				TiledGrid.machines.Add(TiledGrid.gridPoints[transform.position], machine);
		}

		hologram.SetActive(false);
	}
}

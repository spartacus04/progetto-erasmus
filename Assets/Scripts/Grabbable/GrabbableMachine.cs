using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(Machine), typeof(Rigidbody), typeof(XRGrabInteractable))]
public class GrabbableMachine : MonoBehaviour, IGrabbable
{
	public Material hologramMaterial;

	[HideInInspector]
	public bool isGrabbed = false;

	private Rigidbody rb;
	private Machine machine;

	private GameObject hologram;
	private XRGrabInteractable grabInteractable;

	private Quaternion initialRotation;

	public void Start() {
		rb = GetComponent<Rigidbody>();
		machine = GetComponent<Machine>();

		hologram = Instantiate(gameObject);

		Destroy(hologram.GetComponent<GrabbableMachine>());
		Destroy(hologram.GetComponent<XRGrabInteractable>());
		Destroy(hologram.GetComponent<Rigidbody>());
		Destroy(hologram.GetComponent<Collider>());
		Destroy(hologram.GetComponentInChildren<TextMeshProUGUI>());

		if(hologram.TryGetComponent<FurnaceWrapper>(out var fw))
			Destroy(fw);

		if(hologram.TryGetComponent<SawWrapper>(out var sw))
			Destroy(sw);

		Destroy(hologram.GetComponent<Machine>());
		hologram.GetComponentsInChildren<SpriteRenderer>().ForEach(s => Destroy(s));
		
		hologram.SetActive(false);

		GameManager.ApplyMaterialRecursively(hologram, hologramMaterial);

		var grabInteractable = GetComponent<XRGrabInteractable>();
		grabInteractable.selectEntered.AddListener(grabEvent);
		grabInteractable.selectExited.AddListener(releaseEvent);

		initialRotation = transform.rotation;
	}

	public void Update() {
		if(!isGrabbed) return;

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		transform.rotation = initialRotation;

		var (dist, coords) = Grid.nearestGridPoint(transform.position);

		var pos = Grid.grid[coords.x, coords.y];

		if(dist > .3f) {
			hologram.SetActive(false);
		} else {
			hologram.transform.position = pos.position;

			if(Grid.machines[coords.x, coords.y] == null)
				hologram.SetActive(true);
			else
				hologram.SetActive(false);
		}
	}

	public void OnGrab()
	{
		rb.isKinematic = false;
		isGrabbed = true;
		machine.clearContents();

		rb.constraints = RigidbodyConstraints.FreezeAll;

		try {
			Grid.machines[machine.snappedPos.x, machine.snappedPos.y] = null;
		}
		catch { }
	}

	public void OnRelease()
	{
		isGrabbed = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.isKinematic = false;
        
		if(hologram.activeSelf) {
			var (_, coords) = Grid.nearestGridPoint(transform.position);

			if(Grid.machines[coords.x, coords.y] == null){
				machine.ApplyPosition(coords);
			   	transform.rotation = initialRotation; 
			}
		}

		hologram.SetActive(false);
	}

	public void grabEvent(SelectEnterEventArgs args) => OnGrab();
	public void releaseEvent(SelectExitEventArgs args) => OnRelease();
}
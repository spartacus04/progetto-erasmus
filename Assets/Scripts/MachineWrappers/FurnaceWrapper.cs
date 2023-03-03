using UnityEngine;

public class FurnaceWrapper : MonoBehaviour {
	public Material onMaterial;
	private Material offMaterial;

	private Furnace furnace;

	private MeshRenderer meshRenderer;

	private void Start() {
		furnace = GetComponent<Furnace>();
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		offMaterial = meshRenderer.material;
	}

	public void Update() {
		if(furnace.ticks > 0) {
			meshRenderer.material = onMaterial;
		} else {
			meshRenderer.material = offMaterial;
		}
	}
}
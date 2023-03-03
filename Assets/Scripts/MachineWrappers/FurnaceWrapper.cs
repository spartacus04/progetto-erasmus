using UnityEngine;

public class FurnaceWrapper : MonoBehaviour {
	public Material onMaterial;
	private Material offMaterial;

	private Furnace furnace;

	private void Start() {
		furnace = GetComponent<Furnace>();
		offMaterial = GetComponent<MeshRenderer>().material;
	}

	public void Update() {
		if(furnace.ticks > 0) {
			GetComponent<MeshRenderer>().material = onMaterial;
		} else {
			GetComponent<MeshRenderer>().material = offMaterial;
		}
	}
}
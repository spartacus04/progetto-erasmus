using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;
using System.Linq;


//TODO: aggiornare lo script per non usare scale ma dimensione collider
public class TiledGrid : MonoBehaviour
{
	[HideInInspector]
	public static TiledGrid Instance { get; private set; }

	public int units = 10;
	private float internalUnit = 1f;

	[HideInInspector]
	public static List<Vector3> gridPoints = new List<Vector3>();
	[HideInInspector]
	public static Dictionary<Vector3, IMachine> machines = new Dictionary<Vector3, IMachine>();

	private float timer = 0;

	void Awake() {
		if(Instance != null && Instance != this)
			Destroy(gameObject);
		else if(Instance == null)
			Instance = this;
	}

	void Start() {
		getSnaps();
	}

	void getSnaps() {
		if(transform.localScale.x < transform.localScale.z)
			internalUnit = transform.localScale.x / units;
		else
			internalUnit = transform.localScale.z / units;

		for(float x = -transform.localScale.x / 2; x < transform.localScale.x / 2 - internalUnit; x += internalUnit)
			for(float z = -transform.localScale.z / 2; z < transform.localScale.z / 2 - internalUnit; z += internalUnit)
				gridPoints.Add(new Vector3(z + internalUnit / 2f, 0, x + internalUnit /2f));	// Sembra un bug ma non lo è, devo invertire x e z per avere la griglia giusta

		GameObject b = new GameObject("Base");
		b.transform.position = transform.position;

		foreach(Vector3 point in gridPoints) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = b.transform;

			cube.transform.localPosition = point;
			cube.transform.localScale *= 0.01f;
		}
	}

	void Update() {
		if(timer >= TICK_RATE) {
			timer = 0;
			machines.Values.ToList().ForEach(m => m.onTick());
		} else {
			timer += Time.deltaTime;
		}

	}

	public static Vector3 getNearestPoint(Vector3 point) {
		Vector3 nearestPoint = Vector3.zero;
		float nearestDistance = Mathf.Infinity;

		foreach(Vector3 gridPoint in gridPoints) {
			float distance = Vector3.Distance(point, gridPoint + Instance.transform.position);

			if(distance < nearestDistance) {
				nearestDistance = distance;
				nearestPoint = gridPoint + Instance.transform.position;
			}
		}

		return nearestPoint;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;
using System.Linq;


//TODO: aggiornare lo script per non usare scale ma dimensione collider
//TODO: aggiornare lo script per associare i macchinari a delle coordinate 2d e poi associare le coordinate 2d a delle coordinate 3d
[RequireComponent(typeof(BoxCollider))]
public class TiledGrid : MonoBehaviour
{
	[HideInInspector]
	public static TiledGrid Instance { get; private set; }

	public int units = 10;
	private float internalUnit = 1f;

	[HideInInspector]
	public static Dictionary<Vector2Int, Machine> machines = new Dictionary<Vector2Int, Machine>();
	[HideInInspector]
	public static Dictionary<Vector3, Vector2Int> gridPoints = new Dictionary<Vector3, Vector2Int>();

	private float timer = 0;

	private BoxCollider coll;

	void Awake() {
		if(Instance != null && Instance != this)
			Destroy(gameObject);
		else if(Instance == null)
			Instance = this;
	}

	void Start() {
		coll = GetComponent<BoxCollider>();


		getSnaps();
	}

	void getSnaps() {
		if(coll.size.x < coll.size.y)
			internalUnit = coll.size.z / units;
		else
			internalUnit = coll.size.z / units;

		var unitsInY = coll.size.z / internalUnit;
		var unitsInX = coll.size.x / internalUnit;

		var offsetY = (coll.size.z % internalUnit) / 2;
		var offsetX = (coll.size.x % internalUnit) / 2;

		for(int x = 0; x < unitsInX; x++)
			for(int y = 0; y < unitsInY; y++) {
				// scambio x e y perchè altrimenti non funziona
				var point = new Vector3(x * internalUnit + offsetX - coll.size.x / 2, 0, y * internalUnit + offsetY - coll.size.z / 2);
				point = new Vector3(point.z, 0, point.x);
				var gridPoint = new Vector2Int(y, x);

				gridPoints.Add(point, gridPoint);
			}

		foreach(KeyValuePair<Vector3, Vector2Int> entry in gridPoints) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = transform;

			cube.transform.localPosition = new Vector3(entry.Key.z, 0, entry.Key.x);
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

		foreach(KeyValuePair<Vector3, Vector2Int> gridPoint in gridPoints) {
			float distance = Vector3.Distance(point, gridPoint.Key + Instance.transform.position);

			if(distance < nearestDistance) {
				nearestDistance = distance;
				nearestPoint = gridPoint.Key + Instance.transform.position;
			}
		}

		return nearestPoint;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Glob;

public class Grid : MonoBehaviour
{
	static Grid instance = null;

	public int width = 10;
	public int height = 5;
	public float unit = 1f;

	public static Transform [,] grid;

	public static Machine [,] machines;

	private float timer = 0;

	public void Start() {
		instance = this;

		var halfwidth = width / 2;
		var halfheight = height / 2;

		grid = new Transform[width, height];
		machines = new Machine[width, height];

		for(int x = -halfwidth; x < halfwidth; x++)
			for(int y = -halfheight; y < halfheight; y++) {
				var point = new Vector3(x * unit, 0, y * unit);
				var gridPoint = new Vector2Int(x, y);

				var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gameObject.transform.parent = transform;
				gameObject.transform.localPosition = point;
				gameObject.transform.localScale *= 0.01f;

				grid[x + halfwidth, y + halfheight] = gameObject.transform;
			}
	}

	public static (float dist, Vector2Int coords) nearestGridPoint(Vector3 point) {
		var distance = float.MaxValue;
		var coords = new Vector2Int();

		foreach(Transform child in instance.transform) {
			var childDistance = Vector3.Distance(child.position, point);

			if(childDistance < distance) {
				distance = childDistance;

				var (x, y) = grid.indexOf(child);
				coords = new Vector2Int(x, y);
			}
		}

		return (distance, coords);
	}

	public void Update() {
		if(timer >= TICK_RATE) {
			timer = 0;

			for(int x = 0; x < width; x++)
				for(int y = 0; y < height; y++)
					if(machines[x, y] != null) {
						// print($"Ticking machine at {x}, {y}");
						machines[x, y].onTick();
					}
		} else {
			timer += Time.deltaTime;
		}
	}
}
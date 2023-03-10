using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Glob;

public abstract class Machine : MonoBehaviour {
	[HideInInspector]
	public Vector2Int snappedPos = new Vector2Int(-1, -1);

	//[HideInInspector]
	public Item[] inventory;
	[HideInInspector]
	public Fluid[] fluids;

    public abstract void clearContents();
	public abstract void onTick();

	public abstract bool allowFluids { get; }

	public Transform armTransform;

	public abstract void inventoryOperation(InteractionType type, ref Item current);

	public virtual void fluidOperation(InteractionType type, ref Fluid current) { }

	public void ApplyPosition(Vector2Int pos) {
		if (snappedPos.x != -1 || snappedPos.y != -1)
			Grid.machines[snappedPos.x, snappedPos.y] = null;

		snappedPos = pos;

		Grid.machines[pos.x, pos.y] = this;
		transform.position = Grid.grid[pos.x, pos.y].position;

		GetComponent<Rigidbody>().isKinematic = true;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;

public class VirtualTank : Machine
{
	public bool isInputMode = true;
	public override bool allowFluids => true;

	public override void clearContents() { 
		isInputMode = true;
	}

	public override void onTick() {
		var machinesCoords = new List<Vector2Int>();
		// get nearby machines

		// if input mode, pull fluids to machines

		// if output mode, push fluids to machines
	}

	public override void inventoryOperation(InteractionType type, ref Item current) { }
}

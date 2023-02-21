using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Glob;

public abstract class Machine : MonoBehaviour {
	public Item[] inventory;
	public Fluid[] fluids;

    public abstract void clearContents();
	public abstract void onTick();

	public abstract bool allowFluids { get; }

	public abstract void inventoryOperation(InteractionType type, ref Item current);

	public virtual void fluidOperation(InteractionType type, ref Fluid current) { }
}
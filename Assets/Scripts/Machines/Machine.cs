using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Machine : MonoBehaviour {
	public Item[] inventory;
	public Fluid[] fluids;

    public abstract void clearContents();
	public abstract void onTick();

	public virtual bool addInput(Item input) {
		return false;
	}

	public virtual Item getOutput() {
		return null;
	}

	public virtual bool addInput(Fluid input) {
		return false;
	}
	
	public virtual Fluid getOutputFluid() {
		return null;
	}
}
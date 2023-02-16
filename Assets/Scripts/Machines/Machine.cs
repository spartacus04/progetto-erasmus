using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Machine : MonoBehaviour {
	public Item[] inventory;

    public abstract void clearContents();
	public abstract void onTick();
	public abstract bool addInput(Item input);
	public abstract Item getOutput();
}
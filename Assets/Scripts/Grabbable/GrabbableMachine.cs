using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Machine))]
public class GrabbableMachine : GrabbableGeneric
{
	public override void OnGrab(GameObject parent)
	{
		base.OnGrab(parent);
		if(machine != null)
		machine.clearContents();
	}
}

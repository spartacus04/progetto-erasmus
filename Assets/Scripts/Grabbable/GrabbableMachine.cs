using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableMachine : GrabbableGeneric
{
	private IMachine machine;
	public override void Start() {
		base.Start();
	}

	public override void OnGrab(GameObject parent)
	{
		base.OnGrab(parent);
		machine.clearContents();
	}
}

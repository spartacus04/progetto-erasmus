using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableMachine : GrabbableGeneric
{
	private Machine machine;
	public override void Start() {
		machine = GetComponent<Machine>();
		base.Start();
	}

	public override void OnGrab(GameObject parent)
	{
		base.OnGrab(parent);
		machine.clearContents();
	}
}
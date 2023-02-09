using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public interface IGrabbable
{
	void OnGrab(GameObject parent);
	void OnRelease(GameObject parent);
}
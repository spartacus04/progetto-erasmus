using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
	public Joint child;

	public void Start() {
		Destroy(GetComponent<Rigidbody>());
	}

	public void RotateJoint(float angle)
	{
		transform.Rotate(new Vector3(angle, 0, 0));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
	public Joint root;
	public Joint end;
	public Transform target;

	public float threshold = 0.05f;
	public float rate = 5f;

	public int steps = 20;

	float calculateSlope(Joint joint) {
		float deltaTheta = 0.01f;
		float dist1 = distance(end.transform.position, target.transform.position);

		joint.RotateJoint(deltaTheta);

		float dist2 = distance(end.transform.position, target.transform.position);

		joint.RotateJoint(-deltaTheta);

		return (dist2 - dist1) / deltaTheta;
	}

	private void Update() {
		for(int i = 0; i < steps; i++) {
			if(distance (end.transform.position, target.position) < threshold) break;

			var current = root;
			while(current != null) {
				float slope = calculateSlope(current);
				current.RotateJoint(-slope * rate);

				current = current.child;
			}
		}
	}


	public float distance(Vector3 a, Vector3 b)
	{
		return Vector3.Distance(a, b);
	}
}

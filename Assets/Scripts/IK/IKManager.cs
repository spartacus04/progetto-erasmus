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

	public bool canMove = false;

	float calculateSlope(Joint joint) {
		float deltaTheta = 0.01f;
		float dist1 = dist(end.transform.position, target.transform.position);

		joint.RotateJoint(deltaTheta);

		float dist2 = dist(end.transform.position, target.transform.position);

		joint.RotateJoint(-deltaTheta);

		return (dist2 - dist1) / deltaTheta;
	}

	private void Update() {
		if(!canMove) return;

		for(int i = 0; i < steps; i++) {
			if(dist(end.transform.position, target.position) < threshold) break;

			var current = root;
			while(current != null) {
				float slope = calculateSlope(current);
				current.RotateJoint(-slope * rate);

				current = current.child;
			}
		}
	}

	public float dist(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
}

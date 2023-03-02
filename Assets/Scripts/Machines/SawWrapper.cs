using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Furnace))]
public class SawWrapper : MonoBehaviour
{
	private Furnace furnace;
	public Transform start;
	public Transform end;
	public Transform obj;

    void Start()
    {
		furnace = GetComponent<Furnace>();
    }

    void Update()
    {
        // interpolate position between start and end based on tickrate
		obj.position = Vector3.Lerp(start.position, end.position, (float)furnace.ticks / (float)furnace.requiredTicks);
    }
}

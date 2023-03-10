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

	private SpriteRenderer startSR;
	private SpriteRenderer endSR;
	private SpriteRenderer objSR;

    void Start()
    {
		furnace = GetComponent<Furnace>();

		startSR = start.GetComponent<SpriteRenderer>();
		endSR = end.GetComponent<SpriteRenderer>();
		objSR = obj.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		SetIcons();

		if(furnace.requiredTicks == 0) return;
        // interpolate position between start and end based on tickrate
		obj.position = Vector3.Lerp(start.position, end.position, (float)furnace.ticks / (float)furnace.requiredTicks);
    }

	void SetIcons() {
		if(furnace.inventory[0] != null)
			startSR.sprite = furnace.inventory[0].icon;
		else
			startSR.sprite = null;

		if(furnace.inventory[1] != null)
			endSR.sprite = furnace.inventory[1].icon;
		else
			endSR.sprite = null;

		if(furnace.ticks != 0)
			objSR.sprite = furnace.inventory[0].icon;
		else
			objSR.sprite = null;
	}
}

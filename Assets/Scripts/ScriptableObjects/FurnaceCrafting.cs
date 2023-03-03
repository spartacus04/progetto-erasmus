using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnaceCrafting", menuName = "Craftings/Furnace", order = 1)]
public class FurnaceCrafting : ScriptableObject
{
	public Item input;
	public Item output;

	public int inputCount;
	public int outputCount;

	public int ticks;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Unit/Item", order = 1)]
public class Item : ScriptableObject
{
	public new string name;

	public string description;
	public Texture icon;

	public int maxStackSize = 64;
	[HideInInspector]
	public int amount = 0;
}

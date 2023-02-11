using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "erasmus/item", order = 1)]
public class Item : ScriptableObject
{
	public new string name;
	public string description;
	public Sprite icon;

}

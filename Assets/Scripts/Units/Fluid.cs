using UnityEngine;

[CreateAssetMenu(fileName = "Fluid", menuName = "Unit/Fluid", order = 0)]
public class Fluid : ScriptableObject {
	public new string name;

	public string description;
	public Sprite texture;

	[HideInInspector]
	public int quantity;
}
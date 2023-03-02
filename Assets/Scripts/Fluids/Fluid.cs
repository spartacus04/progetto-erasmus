using UnityEngine;

[CreateAssetMenu(fileName = "Fluid", menuName = "Unit/Fluid", order = 0)]
public class Fluid : ScriptableObject {
	public int id;
	public new string name;

	public string description;
	public Sprite icon;

	public int quantity;
}
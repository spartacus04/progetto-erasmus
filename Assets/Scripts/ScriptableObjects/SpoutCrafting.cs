using UnityEngine;

[CreateAssetMenu(fileName = "SpoutCrafting", menuName = "Craftings/Spout", order = 0)]
public class SpoutCrafting : ScriptableObject {
	public Item input;
	public Item output;

	public Fluid fluid;

	public int inputCount;
	public int outputCount;

	public int fluidCount;

	public int ticks;
}
using UnityEngine;

[CreateAssetMenu(fileName = "MixerCrafting", menuName = "Craftings/Mixer", order = 0)]
public class MixerCrafting : ScriptableObject {
	public Item[] input;
	public Item output;

	public Fluid fluidInput;
	public Fluid fluidOutput;

	public int[] inputCount;
	public int outputCount;
	public int fluidInputCount;
	public int fluidOutputCount;

	public int ticks;
}
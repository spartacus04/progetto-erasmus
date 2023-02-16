using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;

public class FurnaceContainer : MonoBehaviour, IMachine
{
	public Item input;
	public Item output;
	
	public int inputCount;
	public int outputCount;

	public List<FurnaceCrafting> recipes;


	private int ticks = 0;

	public void onTick() {
		if(inputCount < 1 && input == null) return;

		recipes.ForEach(recipe => {
			if(recipe.input == input && recipe.inputCount <= inputCount) {
				if(recipe.ticks <= ticks) {
					inputCount -= recipe.inputCount;
					outputCount += recipe.outputCount;
					output = recipe.output;

					ticks = 0;
				} else {
					ticks++;
				}
			}
		});
	}

	public void clearContents() {
		input = null;
		output = null;
		inputCount = 0;
		outputCount = 0;
	}

	public bool addInput(Item input, int count) {
		if(input == null && inputCount <= 0) {
			this.input = input;
			inputCount += count;
		} else if(this.input == input) {
			inputCount += count;
		} else {
			return false;
		}

		return true;
	}

	public (Item item, int count) getOutput() {
		if(outputCount > 0) {
			return (output, outputCount);
		} else {
			return (null, 0);
		}
	}
}

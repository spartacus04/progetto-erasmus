using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;

public class FurnaceContainer : MonoBehaviour
{
	public Item input;
	public Item output;
	
	public int inputCount;
	public int outputCount;

	public List<FurnaceCrafting> recipes;


	private int ticks = 0;
	private float timer = 0;

	private void Update() {
		if(timer >= TICK_RATE) {
			timer = 0;
			Tick();
		} else {
			timer += Time.deltaTime;
		}
	}

	private void Tick() {
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
}

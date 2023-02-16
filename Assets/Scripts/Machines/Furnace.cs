using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;

public class Furnace : Machine
{
	public List<FurnaceCrafting> recipes;

	private int ticks = 0;

	private FurnaceCrafting currentRecipe;

	void Awake() {
		inventory = new Item[2];
	}

	public override void onTick() {
		if(inventory[0] == null) return;

		recipes.ForEach(recipe => {
			if(recipe.input.id == inventory[0].id &&
				inventory[0].amount >= recipe.inputCount &&
				inventory[1].id == recipe.output.id ||
				inventory[1] == null
			) {
				if(recipe.ticks <= ticks) {
					inventory[0].amount -= recipe.inputCount;

					if(inventory[1] != null) {
						inventory[1].amount += recipe.outputCount;
					} else {
						inventory[1] = recipe.output;
						inventory[1].amount = recipe.outputCount;
					}

					ticks = 0;
				} else {
					ticks++;
				}
			}
			else {
				ticks = 0;
			}
		});
	}

	public override void clearContents() {
		inventory.Empty();
	}

	public override bool addInput(Item input) {
		if(input == null) {
			input = inventory[0];

			return true;
		} else if(input.id == inventory[0].id) {
			inventory[0].amount += input.amount;

			return true;
		}

		return false;
	}

	public override Item getOutput() {
		var temp = inventory[1];
		inventory[1] = null;
		return temp;
	}
}

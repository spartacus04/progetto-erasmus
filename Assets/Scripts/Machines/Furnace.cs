using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glob;
using System;

public class Furnace : Machine
{
	public List<FurnaceCrafting> recipes;

	[HideInInspector]
	public int ticks = 0;

	public override bool allowFluids => false;

	[HideInInspector]
	public int requiredTicks = 0;

	private int currentRecipe = -1;

	void Awake() {
		inventory = new Item[2];
	}

	public override void onTick() {
		recipes.ForEachIndexed((recipe, i)=> {
			if(inventory[0] == null) return;


			if(recipe.input.name == inventory[0].name &&
				inventory[0].amount >= recipe.inputCount &&
				(inventory[1] == null ||
				inventory[1].name == recipe.output.name)
			) {
				if(inventory[1] != null && inventory[1].amount > inventory[1].maxStackSize) return;

				if(currentRecipe == -1) {
					currentRecipe = i;
				}

				requiredTicks = recipe.ticks;

				if(recipe.ticks <= ticks) {
					inventory[0].amount -= recipe.inputCount;

					if(inventory[1] != null) {
						inventory[1].amount += recipe.outputCount;
					} else {
						inventory[1] = Instantiate(recipe.output);
						inventory[1].amount = recipe.outputCount;
					}

					if(inventory[0].amount <= 0) {
						inventory[0] = null;
					}

					ticks = 0;
					currentRecipe = -1;
				} else {
					ticks++;
				}
			}
			else {
				if(currentRecipe == -1 || currentRecipe == i) {
					ticks = 0;
					requiredTicks = 0;
				}
			}
		});
	}

	public override void clearContents() {
		inventory[0] = null;
		inventory[1] = null;
	}

	public override void inventoryOperation(InteractionType type, ref Item current) {
		switch(type) {
			case InteractionType.PUSH:
				if(inventory[0] == null) {
					inventory[0] = Instantiate(current);
					current = null;
				} else if(current.name == inventory[0].name) {
					inventory[0].amount += current.amount;

					if(inventory[0].amount > inventory[0].maxStackSize) {
						current.amount = inventory[0].amount - inventory[0].maxStackSize;
						inventory[0].amount = inventory[0].maxStackSize;
					} else {
						current = null;
					}
				}

				break;
			case InteractionType.PULL:
				if(current == null) {
					current = Instantiate(inventory[1]);

					if(current.amount > current.maxStackSize) {
						inventory[1].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					}
					else {
						inventory[1] = null;
					}
				} else if(current.name == inventory[1].name) {
					current.amount += inventory[1].amount;

					if(current.amount > current.maxStackSize) {
						inventory[1].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					} else {
						inventory[1] = null;
					}
				}

				break;
		}
	}
}
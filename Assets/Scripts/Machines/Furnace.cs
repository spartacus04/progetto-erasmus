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

	void Awake() {
		inventory = new Item[2];
	}

	public override void onTick() {
		if(inventory[0] == null) return;

		recipes.ForEach(recipe => {
			if(recipe.input.id == inventory[0].id &&
				inventory[0].amount >= recipe.inputCount &&
				(inventory[1] == null ||
				inventory[1].id == recipe.output.id)
			) {
				if(inventory != null && (inventory[1].amount > inventory[1].maxStackSize)) return;

				requiredTicks = recipe.ticks;

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
				requiredTicks = 0;
			}
		});
	}

	public override void clearContents() {
		inventory.Empty();
	}

	public override void inventoryOperation(InteractionType type, ref Item current) {
		switch(type) {
			case InteractionType.PUSH:
				if(inventory[0] == null) {
					inventory[0] = current;
					current = null;
				} else if(current.id == inventory[0].id) {
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
					current = inventory[0];

					if(current.amount > current.maxStackSize) {
						inventory[0].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					}
					else {
						inventory[0] = null;
					}
				} else if(current.id == inventory[0].id) {
					current.amount += inventory[0].amount;

					if(current.amount > current.maxStackSize) {
						inventory[0].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					} else {
						inventory[0] = null;
					}
				}

				break;
		}
	}
}
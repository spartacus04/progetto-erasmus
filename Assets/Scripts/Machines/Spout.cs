using UnityEngine;
using System.Collections.Generic;
using static Glob;

public class Spout : Machine {
	public override bool allowFluids => true;

	public List<SpoutCrafting> recipes;

	private int ticks = 0;

	void Start() {
		inventory = new Item[2];
		fluids = new Fluid[1];
	}

	public override void onTick() {
		if(inventory[0] == null) return;

		recipes.ForEach(recipe => {
			if(recipe.input.name == inventory[0].name &&
				inventory[0].amount >= recipe.inputCount &&
				(inventory[1] == null ||
				inventory[1].name == recipe.output.name)
			) {
				if(inventory[1] != null && (inventory[1].amount > inventory[1].maxStackSize)) return;

				if(recipe.ticks <= ticks) {
					inventory[0].amount -= recipe.inputCount;
					fluids[0].quantity -= recipe.fluidCount;

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

	public override void clearContents()
	{
		inventory[0] = null;
		inventory[1] = null;
		fluids[0] = null;
	}

	public override void inventoryOperation(InteractionType type, ref Item current)
	{
		switch(type) {
			case InteractionType.PUSH:
				if(inventory[0] == null) {
					inventory[0] = current;
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
					current = inventory[0];

					if(current.amount > current.maxStackSize) {
						inventory[0].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					}
					else {
						inventory[0] = null;
					}
				} else if(current.name == inventory[0].name) {
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

	public override void fluidOperation(InteractionType type, ref Fluid current)
	{
		if(type != InteractionType.PUSH) return;

		if(fluids[0] == null) {
			fluids[0] = current;
			current = null;
		} else if(current.name == fluids[0].name) {
			fluids[0].quantity += current.quantity;

			if(fluids[0].quantity > DEFAULT_TANK_CAPACITY) {
				current.quantity = fluids[0].quantity - DEFAULT_TANK_CAPACITY;
				fluids[0].quantity = DEFAULT_TANK_CAPACITY;
			} else {
				current = null;
			}
		}
	}
}
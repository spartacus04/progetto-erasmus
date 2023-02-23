using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Glob;

public class Mixer : Machine {

	public List<MixerCrafting> recipes;
	public override bool allowFluids => true;
	private int ticks = 0;

	void Start() {
		inventory = new Item[3];
		fluids = new Fluid[2];
	}

	public override void onTick() {
		if(inventory[0] == null || inventory[1]) return;

		recipes.ForEach(recipe => {
			if((recipe.input[0].id == inventory[0].id &&
				inventory[0].amount >= recipe.inputCount[0] &&
				recipe.input[1].id == inventory[1].id &&
				inventory[1].amount >= recipe.inputCount[1] &&
				(inventory[2] == null || 
				inventory[2].id == recipe.output.id) && 
				(fluids[1] == null ||
				fluids[1].id == recipe.fluidOutput.id)) 
				||
				(recipe.input[0].id == inventory[1].id &&
				inventory[1].amount >= recipe.inputCount[1] &&
				recipe.input[1].id == inventory[0].id &&
				inventory[0].amount >= recipe.inputCount[0] &&
				(inventory[2] == null ||
				inventory[2].id == recipe.output.id) &&
				(fluids[1] == null ||
				fluids[1].id == recipe.fluidOutput.id))
			) {
				if(inventory[2] != null && (inventory[2].amount > inventory[2].maxStackSize)) return;
				if(fluids[1] != null && (fluids[1].quantity > DEFAULT_TANK_CAPACITY)) return;

				if(recipe.ticks <= ticks) {
					if(inventory[0].id == inventory[0].id) {
						inventory[0].amount -= recipe.inputCount[0];
						inventory[1].amount -= recipe.inputCount[1];
					} else {
						inventory[0].amount -= recipe.inputCount[1];
						inventory[1].amount -= recipe.inputCount[0];
					}

					if(fluids[1] != null) {
						fluids[1].quantity += recipe.fluidOutputCount;
					} else {
						fluids[1] = recipe.fluidOutput;
						fluids[1].quantity = recipe.fluidOutputCount;
					}

					if(inventory[2] != null) {
						inventory[2].amount += recipe.outputCount;
					} else {
						inventory[2] = recipe.output;
						inventory[2].amount = recipe.outputCount;
					}

					ticks = 0;
				} else {
					ticks++;
				}
			} else {
				ticks = 0;
			}
		});
	}

	public override void clearContents() {
		inventory.Empty();
	}

	public override void inventoryOperation(Glob.InteractionType type, ref Item current)
	{
		switch(type) {
			case InteractionType.PUSH:
				if(inventory[0] == null) {
					if(inventory[1] != null && inventory[1].id == current.id) {
						inventory[1].amount += current.amount;

						if(inventory[1].amount > inventory[1].maxStackSize) {
							current.amount = inventory[1].amount - inventory[1].maxStackSize;
							inventory[1].amount = inventory[1].maxStackSize;
						} else {
							current = null;
						}
					} else {
						inventory[0] = current;
						current = null;
					}
				} else if(current.id == inventory[0].id) {
					inventory[0].amount += current.amount;

					if(inventory[0].amount > inventory[0].maxStackSize) {
						current.amount = inventory[0].amount - inventory[0].maxStackSize;
						inventory[0].amount = inventory[0].maxStackSize;
					} else {
						current = null;
					}
				}

				if(inventory[1] == null) {
					if(inventory[0] != null && inventory[0].id == current.id) {
						inventory[0].amount += current.amount;

						if(inventory[0].amount > inventory[0].maxStackSize) {
							current.amount = inventory[0].amount - inventory[0].maxStackSize;
							inventory[0].amount = inventory[0].maxStackSize;
						} else {
							current = null;
						}
					} else {
						inventory[1] = current;
						current = null;
					}
				} else if(current.id == inventory[1].id) {
					inventory[1].amount += current.amount;

					if(inventory[1].amount > inventory[1].maxStackSize) {
						current.amount = inventory[1].amount - inventory[1].maxStackSize;
						inventory[1].amount = inventory[1].maxStackSize;
					} else {
						current = null;
					}
				}

				break;
			case InteractionType.PULL:
				if(current == null) {
					current = inventory[2];

					if(current.amount > current.maxStackSize) {
						inventory[2].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					}
					else {
						inventory[2] = null;
					}
				} else if(current.id == inventory[2].id) {
					current.amount += inventory[2].amount;

					if(current.amount > current.maxStackSize) {
						inventory[2].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					} else {
						inventory[2] = null;
					}
				}


				break;
		}
	}

	public override void fluidOperation(InteractionType type, ref Fluid current)
	{
		switch(type) {
			case InteractionType.PUSH:
				if(fluids[0] == null) {
					fluids[0] = current;
					current = null;
				} else if(current.id == fluids[0].id) {
					fluids[0].quantity += current.quantity;

					if(fluids[0].quantity > DEFAULT_TANK_CAPACITY) {
						current.quantity = fluids[0].quantity - DEFAULT_TANK_CAPACITY;
						fluids[0].quantity = DEFAULT_TANK_CAPACITY;
					} else {
						current = null;
					}
				}

				break;
			case InteractionType.PULL:
				if(current == null) {
					current = fluids[1];

					if(current.quantity > DEFAULT_TANK_CAPACITY) {
						fluids[1].quantity = current.quantity - DEFAULT_TANK_CAPACITY;
						current.quantity = DEFAULT_TANK_CAPACITY;
					}
					else {
						fluids[1] = null;
					}
				} else if(current.id == fluids[1].id) {
					current.quantity += fluids[1].quantity;

					if(current.quantity > DEFAULT_TANK_CAPACITY) {
						fluids[1].quantity = current.quantity - DEFAULT_TANK_CAPACITY;
						current.quantity = DEFAULT_TANK_CAPACITY;
					} else {
						fluids[1] = null;
					}
				}

				break;
		}
	}
}
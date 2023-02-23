using UnityEngine;
using static Glob;

public class Drain : Machine {
	public override bool allowFluids => true;

	void Start() {
		inventory = new Item[2];
		fluids = new Fluid[1];
	}

	public override void onTick() {

	}

	public override void clearContents()
	{
		inventory.Empty();
		fluids.Empty();
	}

	public override void inventoryOperation(InteractionType type, ref Item current)
	{
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

	public override void fluidOperation(InteractionType type, ref Fluid current)
	{
		if(type != InteractionType.PULL) return;
		
		if(current == null) {
			current = fluids[0];

			if(current.quantity > DEFAULT_TANK_CAPACITY) {
				fluids[0].quantity = current.quantity - DEFAULT_TANK_CAPACITY;
				current.quantity = DEFAULT_TANK_CAPACITY;
			}
			else {
				fluids[0] = null;
			}
		} else if(current.id == fluids[0].id) {
			current.quantity += fluids[0].quantity;

			if(current.quantity > DEFAULT_TANK_CAPACITY) {
				fluids[0].quantity = current.quantity - DEFAULT_TANK_CAPACITY;
				current.quantity = DEFAULT_TANK_CAPACITY;
			} else {
				fluids[0] = null;
			}
		}
	}
}
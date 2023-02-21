using UnityEngine;
using static Glob;

public class Mixer : Machine {

	public override bool allowFluids => true;

	//TODO: aggiungere supporto ai fluidi

	void Start() {
		inventory = new Item[3];
		fluids = new Fluid[2];
	}

	public override void onTick() {

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

				break;
		}
	}
}
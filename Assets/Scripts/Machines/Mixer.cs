using UnityEngine;

public class Mixer : Machine {
	// implement 
	public int maximumFluids = 3000;
	//TODO: aggiungere supporto ai fluidi

	void Start() {
		inventory = new Item[3];
	}

	public override void onTick() {

	}

	public override void clearContents() {
		inventory.Empty();
	}

	public override bool addInput(Item input) {
		if(inventory[0] == null) {
			inventory[0] = input;
			return true;
		} else if(inventory[1] == null) {
			inventory[1] = input;
			return true;
		}

		if(inventory[0].id == input.id) {
			inventory[0].amount += input.amount;
			return true;
		} else if(inventory[1].id == input.id) {
			inventory[1].amount += input.amount;
			return true;
		}

		return false;
	}

	public override Item getOutput() {
		var temp = inventory[2];
		inventory[2] = null;
		return temp;
	}
}
using UnityEngine;
using System.Collections.Generic;
using static Glob;

public class Drain : Machine {
	public override bool allowFluids => true;

	public bool destroyExcessFluids = false;

	public List<SpoutCrafting> recipes;

	private int ticks = 0;

	public SpriteRenderer[] spriteRenderers;

	public MeshRenderer fluidRenderer;


	void Start() {
		inventory = new Item[2];
		fluids = new Fluid[1];
	}

	public void Update() {
		if(inventory[0] != null)
			spriteRenderers[0].sprite = inventory[0].icon;
		else
			spriteRenderers[0].sprite = null;

		if(inventory[1] != null)
			spriteRenderers[1].sprite = inventory[1].icon;
		else
			spriteRenderers[1].sprite = null;

		if(fluids[0] != null) {
			// set same texture
			fluidRenderer.gameObject.SetActive(true);
			fluidRenderer.material.mainTexture = fluids[0].texture.texture;
		} else {
			fluidRenderer.gameObject.SetActive(false);
		}
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
				if(fluids[0] != null && (fluids[0].quantity > DEFAULT_TANK_CAPACITY) && !destroyExcessFluids) return;

				if(recipe.ticks <= ticks) {
					inventory[0].amount -= recipe.inputCount;

					if(fluids[0] != null) {
						fluids[0].quantity += recipe.fluidCount;
					} else {
						fluids[0] = recipe.fluid;
						fluids[0].quantity = recipe.fluidCount;
					}

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
		} else if(current.name == fluids[0].name) {
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
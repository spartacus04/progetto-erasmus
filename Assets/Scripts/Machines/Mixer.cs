using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Glob;

public class Mixer : Machine {
	public List<MixerCrafting> recipes;
	public override bool allowFluids => true;
	private int ticks = 0;
	private Animator animator;
	public SpriteRenderer[] sprites;

	private int currentRecipe = -1;

	void Start() {
		inventory = new Item[3];
		fluids = new Fluid[2];

		animator = GetComponent<Animator>();
	}

	public void Update() {
		if(inventory[0] != null)
			sprites[0].sprite = inventory[0].icon;
		else
			sprites[0].sprite = null;

		if(inventory[1] != null)
			sprites[1].sprite = inventory[1].icon;
		else
			sprites[1].sprite = null;

		if(inventory[2] != null)
			sprites[2].sprite = inventory[2].icon;
		else
			sprites[2].sprite = null;
	}

	public override void onTick() {
		if(inventory[0] == null || inventory[1] == null) return;

		recipes.ForEachIndexed((recipe, i) => {
			if((recipe.input[0].name == inventory[0].name &&
				inventory[0].amount >= recipe.inputCount[0] &&
				recipe.input[1].name == inventory[1].name &&
				inventory[1].amount >= recipe.inputCount[1] &&
				(inventory[2] == null || 
				inventory[2].name == recipe.output.name) && 
				(fluids[1] == null ||
				fluids[1].name == recipe.fluidOutput.name)) 
				||
				(recipe.input[0].name == inventory[1].name &&
				inventory[1].amount >= recipe.inputCount[1] &&
				recipe.input[1].name == inventory[0].name &&
				inventory[0].amount >= recipe.inputCount[0] &&
				(inventory[2] == null ||
				inventory[2].name == recipe.output.name) &&
				(fluids[1] == null ||
				fluids[1].name == recipe.fluidOutput.name))
			) {
				if(inventory[2] != null && (inventory[2].amount > inventory[2].maxStackSize)) return;
				if(fluids[1] != null && (fluids[1].quantity > DEFAULT_TANK_CAPACITY)) return;

				if(currentRecipe == -1) {
					currentRecipe = i;
				}

				StartCoroutine(startAnim());

				if(recipe.ticks <= ticks) {
					if(inventory[0].name == inventory[0].name) {
						inventory[0].amount -= recipe.inputCount[0];
						inventory[1].amount -= recipe.inputCount[1];
					} else {
						inventory[0].amount -= recipe.inputCount[1];
						inventory[1].amount -= recipe.inputCount[0];
					}

					if(recipe.fluidInput != null) {
						if(fluids[1] != null) {
							fluids[1].quantity += recipe.fluidOutputCount;
						} else {
							fluids[1] = Instantiate(recipe.fluidOutput);
							fluids[1].quantity = recipe.fluidOutputCount;
						}
					}

					if(recipe.output != null) {
						if(inventory[2] != null) {
							inventory[2].amount += recipe.outputCount;
						} else {
							inventory[2] = Instantiate(recipe.output);
							inventory[2].amount = recipe.outputCount;
						}
					}

					ticks = 0;
				} else {
					ticks++;
				}
			} else {
				if(currentRecipe == -1 || currentRecipe == i) {
					ticks = 0;
					StartCoroutine(stopAnim());
				}
			}
		});
	}

	public override void clearContents() {
		inventory[0] = null;
		inventory[1] = null;
		inventory[2] = null;

		fluids[0] = null;
		fluids[1] = null;
	}

	public override void inventoryOperation(Glob.InteractionType type, ref Item current)
	{
		switch(type) {
			case InteractionType.PUSH:
				if(current == null) return;

				if(inventory[0] == null) {
					if(inventory[1] != null && inventory[1].name == current.name) {
						inventory[1].amount += current.amount;

						if(inventory[1].amount > inventory[1].maxStackSize) {
							current.amount = inventory[1].amount - inventory[1].maxStackSize;
							inventory[1].amount = inventory[1].maxStackSize;
						} else {
							current = null;
						}
					} else {
						inventory[0] = Instantiate(current);
						current = null;
					}

					return;
				} else if(current.name == inventory[0].name) {
					inventory[0].amount += current.amount;

					if(inventory[0].amount > inventory[0].maxStackSize) {
						current.amount = inventory[0].amount - inventory[0].maxStackSize;
						inventory[0].amount = inventory[0].maxStackSize;
					} else {
						current = null;
					}

					return;
				}

				if(inventory[1] == null) {
					if(inventory[0] != null && inventory[0].name == current.name) {
						inventory[0].amount += current.amount;

						if(inventory[0].amount > inventory[0].maxStackSize) {
							current.amount = inventory[0].amount - inventory[0].maxStackSize;
							inventory[0].amount = inventory[0].maxStackSize;
						} else {
							current = null;
						}
					} else {
						inventory[1] = Instantiate(current);
						current = null;
					}
				} else if(current.name == inventory[1].name) {
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
				if(inventory[2] == null) return;


				if(current == null) {
					current = Instantiate(inventory[2]);

					if(current.amount > current.maxStackSize) {
						inventory[2].amount = current.amount - current.maxStackSize;
						current.amount = current.maxStackSize;
					}
					else {
						inventory[2] = null;
					}
				} else if(current.name == inventory[2].name) {
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
				} else if(current.name == fluids[0].name) {
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
				} else if(current.name == fluids[1].name) {
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

	#region anim
	
	IEnumerator startAnim() {
		StopCoroutine(stopAnim());

		float initalSpeed = animator.GetFloat("speed");
		float t = 0;
		float speed = 0;
		while(t < 1) {
			t += Time.deltaTime;
			speed = Mathf.Lerp(initalSpeed, 1, t);
			animator.SetFloat("speed", speed);
			yield return null;
		}

		animator.SetFloat("speed", 1);
	}

	IEnumerator stopAnim() {
		StopCoroutine(startAnim());
		
		float initalSpeed = animator.GetFloat("speed");
		float t = 0;
		float speed = 0;

		while(t < 1) {
			t += Time.deltaTime;
			speed = Mathf.Lerp(initalSpeed, 0, t);
			animator.SetFloat("speed", speed);
			yield return null;
		}

		animator.SetFloat("speed", 0);
	}

	#endregion
}
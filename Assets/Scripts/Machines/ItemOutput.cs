using UnityEngine;
using static Glob;

public class ItemOutput : Machine
{
	public Item item;

	public int genRate = 5;

	private int ticks = 0;

	public override bool allowFluids => false;

	private SpriteRenderer spriteRenderer;

	public override void clearContents() { }

	public void Start() {
		inventory = new Item[1];
		inventory[0] = Instantiate(item);

		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		spriteRenderer.sprite = inventory[0].icon;
	}

	public override void inventoryOperation(InteractionType type, ref Item current)
	{
		if(type == InteractionType.PULL) {
			if(current == null) {
				current = Instantiate(inventory[0]);

				if(current.amount > current.maxStackSize) {
					inventory[0].amount = current.amount - current.maxStackSize;
					current.amount = current.maxStackSize;
				}
				else {
					inventory[0].amount = 0;
				}
			} else if(current.name == inventory[0].name) {
				current.amount += inventory[0].amount;

				if(current.amount > current.maxStackSize) {
					inventory[0].amount = current.amount - current.maxStackSize;
					current.amount = current.maxStackSize;
				}
				else {
					inventory[0].amount = 0;
				}
			}
		}
	}

	public override void onTick() { 
		if (ticks++ >= genRate) {
			ticks = 0;

			inventory[0].amount++;
		}
	}
}
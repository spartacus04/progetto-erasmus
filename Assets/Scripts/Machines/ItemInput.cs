using UnityEngine;
using static Glob;
using TMPro;

public class ItemInput : Machine
{
	public Item item;

	public int count = 0; 

	public override bool allowFluids => false;

	private SpriteRenderer spriteRenderer;

	private TextMeshProUGUI text;

	public override void clearContents() { }

	public void Start() {
		inventory = new Item[1];
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		spriteRenderer.sprite = item.icon;

		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public override void inventoryOperation(InteractionType type, ref Item current)
	{
		if(type == InteractionType.PUSH) { 
			if(current != null && current.name == item.name) {
				count += current.amount;
				current = null;
			}
		}
	}

	private void Update() {
		text.text = $"{count}/{item.maxStackSize}";
	}

	public override void onTick() { }
}
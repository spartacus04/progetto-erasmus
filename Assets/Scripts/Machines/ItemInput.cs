using UnityEngine;
using static Glob;
using TMPro;

public class ItemInput : Machine
{
	public Item item;

	private int count = 0; 

	public override bool allowFluids => false;

	private SpriteRenderer spriteRenderer;

	private TextMeshProUGUI text;
	public GameObject winScreen;

	public override void clearContents() { 
		count = 0;
	}

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

		if(count >= 8) {
			winScreen.SetActive(true);
		}
	}

	private void Update() {
		text.text = $"{count}/8";
	}

	public override void onTick() { }
}
using UnityEngine;
using System.Collections.Generic;
using static Glob;
using System.Linq;

public class MechanicalArm : Machine, IClickable
{
    public List<Vector2Int> input;

    public List<Vector2Int> output;

	public override bool allowFluids => false;

	private bool isSelecting = false;


    void Start() {
        inventory = new Item[1];
    }

    public override void onTick()
	{

	}

	public override void clearContents()
	{
        inventory.Empty();

		if(isSelecting) OnClick();

		input = new List<Vector2Int>();
		output = new List<Vector2Int>();
	}

	public override void inventoryOperation(InteractionType type, ref Item current) { }

	public void OnClick()
	{
		if(!isSelecting) {
			for (int x = -2; x <= 2; x++)
			{
				for (int y = -2; y <= 2; y++)
				{
					var machine = Grid.machines[x + snappedPos.x, y + snappedPos.y];

					if (machine != null && machine.gameObject != gameObject && !(machine is MechanicalArm)) {
						machine.gameObject.AddComponent<Selectable>();

						if(input.Contains(new Vector2Int(x + snappedPos.x, y + snappedPos.y))) {
							machine.GetComponent<Selectable>().Apply(1);
						}
						else if(output.Contains(new Vector2Int(x + snappedPos.x, y + snappedPos.y))) {
							machine.GetComponent<Selectable>().Apply(2);
						}
						else {
							machine.GetComponent<Selectable>().Apply(0);
						}
					}
				}
			}

			input = new List<Vector2Int>();
			output = new List<Vector2Int>();

			isSelecting = true;
		}
		else {
			var machines = FindObjectsOfType<Selectable>().ToList();

			machines.ForEach(m => {
				var pos = m.GetComponent<Machine>().snappedPos;

				if(m.mode == 1) {
					input.Add(pos);
				}
				else if(m.mode == 2) {
					output.Add(pos);
				}

				m.Remove();
			});

			machines.ForEach(m => Destroy(m));

			isSelecting = false;
		}
	}
}
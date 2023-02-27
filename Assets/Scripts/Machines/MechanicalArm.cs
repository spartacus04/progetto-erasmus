using UnityEngine;
using System.Collections.Generic;
using static Glob;
using System.Linq;

public class MechanicalArm : Machine, IClickable
{
	public Material material;

    public Vector2Int input;

    public List<Vector2Int> output;

    void Start() {
        inventory = new Item[1];
    }

    public override void onTick()
	{

	}

	public override bool allowFluids => false;

	public override void clearContents()
	{
        inventory.Empty();
	}

	public override void inventoryOperation(InteractionType type, ref Item current) { }

	public void OnClick()
	{
		var machines = new List<GameObject>();

		for (int x = -2; x <= 2; x++)
		{
			for (int y = -2; y <= 2; y++)
			{
				var machine = Grid.machines[x + snappedPos.x, y + snappedPos.y];
				if (machine != null && machine.gameObject != gameObject)
					machines.Add(machine.gameObject);
			}
		}

		machines.ForEach(x => Debug.Log(x));

		machines.ForEach(x => GameManager.ApplyMaterialRecursively(x, material));
	}
}
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Glob;
using System.Linq;

public class MechanicalArm : Machine, IClickable
{
	[HideInInspector]
    public List<Vector2Int> input;

	[HideInInspector]
    public List<Vector2Int> output;

	[HideInInspector]
	public override bool allowFluids => false;

	[HideInInspector]
	private bool isSelecting = false;

	[HideInInspector]
	public int inp = 0;

	[HideInInspector]
	public int outp = 0;

	[HideInInspector]
	private int tickcount = 0;

	public int requiredTicks = 5;

	public SpriteRenderer spriteRenderer;

	public Transform armBase;

	public Transform defaultArmTransform;

	private IKManager ikManager;

	private static MechanicalArm selectedArm;

    void Start() {
        inventory = new Item[1];

		ikManager = GetComponent<IKManager>();
    }

	void Update() {
		if(inventory[0] != null)
			spriteRenderer.sprite = inventory[0].icon;
		else
			spriteRenderer.sprite = null;
	}

    public override void onTick()
	{
		// filter out input if null
		input = input.Where(x => Grid.machines[x.x, x.y] != null).ToList();
		output = output.Where(x => Grid.machines[x.x, x.y] != null).ToList();

		if(output.Count == 0 || input.Count == 0) return;

		if(inventory[0] == null) {
			var machin = Grid.machines[input[inp].x, input[inp].y];

			if(tickcount == 0) {
				StartCoroutine(getObj(machin.armTransform));
			}

			if(tickcount == requiredTicks) {
				tickcount = 0;

				if(machin != null) {
					machin.inventoryOperation(InteractionType.PULL, ref inventory[0]);
				}

				inp++;

				if(inp >= input.Count) inp = 0;
			} else {
				tickcount++;
			}

			return;
		}

		var machine = Grid.machines[output[outp].x, output[outp].y];

		if(tickcount == 0) {
			StartCoroutine(getObj(machine.armTransform));
		}

		if(tickcount == requiredTicks) {
			tickcount = 0;

			if(machine != null) {
				machine.inventoryOperation(InteractionType.PUSH, ref inventory[0]);
			}

			outp++;

			if(outp >= output.Count) outp = 0;
		} else {
			tickcount++;
		}
	}

	public override void clearContents()
	{
        inventory[0] = null;

		if(isSelecting) OnClick();

		input = new List<Vector2Int>();
		output = new List<Vector2Int>();

		inp = 0;
		outp = 0;

		tickcount = 0;
	}

	public override void inventoryOperation(InteractionType type, ref Item current) { }

	public void OnClick()
	{
		if(selectedArm != null && selectedArm != this) return;

		if(!isSelecting) {
			for (int x = -2; x <= 2; x++)
			{
				for (int y = -2; y <= 2; y++)
				{
					try{
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
					} catch (IndexOutOfRangeException) { }
					catch(Exception e) {
						Debug.LogError(e);
					}
				}
			}

			input = new List<Vector2Int>();
			output = new List<Vector2Int>();

			isSelecting = true;
			selectedArm = this;
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
			selectedArm = null;
		}
	}

	#region anim
	IEnumerator getObj(Transform coords) {
		ikManager.target = defaultArmTransform;
		ikManager.canMove = true;

		yield return new WaitForSeconds(TICK_RATE);

		ikManager.canMove = false;

		// rotate base towards coords in 1 tick
		var startRotation = armBase.rotation;

		// interpolate quaternion
		float t = 0;

		while(t < TICK_RATE) {
			t += Time.deltaTime;
			armBase.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t / TICK_RATE);
			yield return null;
		}

		armBase.rotation = Quaternion.identity;

		yield return new WaitForSeconds(TICK_RATE);

		// rotate base towards coords in 1 tick
		var target = Quaternion.LookRotation(armBase.position - coords.position, Vector3.up);
		var baseRotation = armBase.rotation;

		// interpolate quaternion
		t = 0;

		while(t < TICK_RATE) {
			t += Time.deltaTime;
			armBase.rotation = Quaternion.Lerp(baseRotation, target, t / TICK_RATE);
			armBase.rotation = Quaternion.Euler(0, armBase.rotation.eulerAngles.y, 0);
			yield return null;
		}

		armBase.rotation = target;
		armBase.rotation = Quaternion.Euler(0, armBase.rotation.eulerAngles.y, 0);

		ikManager.target = coords;
		ikManager.canMove = true;

		yield return new WaitForSeconds(TICK_RATE);

		ikManager.canMove = false;
	}

	#endregion
}